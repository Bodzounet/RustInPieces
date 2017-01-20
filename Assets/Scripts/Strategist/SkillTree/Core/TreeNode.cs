using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace SkillTree
{
    [RequireComponent(typeof(UI_TreeNode))]
    public abstract class TreeNode : MonoBehaviour
    {
        public delegate void LevelChanged(int newLevel, e_LevelOperation op);
        public event LevelChanged OnLevelChanged;

        public delegate void StateChanged(e_NodeState newState);
        public event StateChanged OnStateChanged;

        // set when the TreeManager create the node;
        private int _uniqueID;
        public int UniqueID
        {
            get
            {
                return _uniqueID;
            }
            set
            {
                _uniqueID = value;
            }
        }

        public enum e_LevelOperation
        {
            ADD = 0,
            REMOVE = 1
        }

        public enum e_NodeState
        {
            locked,
            unlocked
        }

        Action[,] switchOnModifyNode;

        protected virtual void Awake()
        {
            switchOnModifyNode = new Action[,]
            {
                {new Action(From0To1), new Action(From1To2), new Action(From2To3) },
                {new Action(From1To0), new Action(From2To1), new Action(From3To2) },
            };
        }

        private CurrenciesManager _currenciesManager;
        public CurrenciesManager CurrenciesManager
        {
            get
            {
                if (_currenciesManager == null)
                    _currenciesManager = GetComponentInParent<StrategistManager>().currenciesManager;
                return _currenciesManager;
            }

            set
            {
                _currenciesManager = value;
            }
        }

        private int _level = 0;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
#if UNITY_EDITOR
                if (value < 0 || value > MaxLevel)
                    Debug.LogError("This node : " + this.gameObject.name + " cannot reach this level : " + value);
#endif
                e_LevelOperation op = _level < value ? e_LevelOperation.ADD : e_LevelOperation.REMOVE;
                RaiseEventOptions reo = new RaiseEventOptions();
                reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

                RaiseEventStrategist.UpdateTreeNodeStateData utnsd = new RaiseEventStrategist.UpdateTreeNodeStateData();
                utnsd.nodeID = UniqueID;
                utnsd.newNodeLevel = value;
                utnsd.op = op;
                utnsd.pid = GetComponentInParent<StrategistManager>().GetStrategistPhotonView.viewID;

                PhotonNetwork.RaiseEvent(EventCode.STRAT_MODIFY_NODE, utnsd, true, reo);

                //for (int i = 0; i < Mathf.Abs(value - _level); i++)
                //{
                //    if (op == e_LevelOperation.ADD)
                //    {
                //        _currenciesManager.currencies[CurrenciesManager.e_Currencies.Gold].UseCurrency(pricePerLevel[_level]);
                //    }
                //    else
                //    {
                //        _currenciesManager.currencies[CurrenciesManager.e_Currencies.Gold].AddCurrency((int)(pricePerLevel[value] * refundPercentage));
                //    }
                //}


                //ModifyNode(value, op);
                //_level = value;

                //if (OnLevelChanged != null)
                //    OnLevelChanged(value, op);
            }
        }

        public void ChangeLevel(int newLevel, e_LevelOperation op)
        {
            for (int i = 0; i < Mathf.Abs(newLevel - _level); i++)
            {
                if (op == e_LevelOperation.ADD)
                {
                    CurrenciesManager.currencies[CurrenciesManager.e_Currencies.Gold].UseCurrency(pricePerLevel[_level]);
                    ModifyNode(_level, op);
                    _level++;
                }
                else
                {
                    CurrenciesManager.currencies[CurrenciesManager.e_Currencies.Gold].AddCurrency((int)(pricePerLevel[newLevel] * refundPercentage));
                    _level--;
                    ModifyNode(_level, op);
                }

                if (OnLevelChanged != null)
                    OnLevelChanged(_level, op);
            }
        }

        private e_NodeState state = e_NodeState.locked;
        public e_NodeState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state == value)
                    return;
                state = value;
                if (OnStateChanged != null)
                    OnStateChanged(value);
            }
        }

        public int nodeBranch; // the rank in the tree (branch 0, 1, 2 or 3 for the elite)
        [HideInInspector]
        public int[] pricePerLevel;
        public float refundPercentage; // from 0 to 1, not from 0 to 100.

        public int EffectiveLevel // for example, a node on the branch 2 has an effective level of 2 but a real level of 0.
        {
            get
            {
                return nodeBranch + Level;
            }
        }

        public int MaxLevel
        {
            get
            {
                if (nodeBranch == 0)
                    return 3;
                if (nodeBranch == 1)
                    return 2;
                return 1;
            }
        }

        public void ResetNode()
        {
            Level = 0;
            State = e_NodeState.locked;
        }

        /// <summary>
        /// Create a child of this script and override the FromXToY functions you need. (let just the other empty or with a throw inside)
        /// </summary>
        /// <param name="newLevel"></param>
        /// <param name="operation"></param>
        public void ModifyNode(int newLevel, e_LevelOperation operation)
        {
            switchOnModifyNode[(int)operation, newLevel]();
        }

        protected abstract void From0To1();
        protected abstract void From1To0();
        protected abstract void From1To2();
        protected abstract void From2To1();
        protected abstract void From2To3();
        protected abstract void From3To2();
    }
}