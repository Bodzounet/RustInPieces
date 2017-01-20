using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SkillTree
{
    public class TreeManager : MonoBehaviour
    {
        public delegate void ChangeBranchLevel(int newLevel);
        public event ChangeBranchLevel OnChangeBranchLevel;

        public enum e_CanAddPointToNodeReturn
        {
            isAlreadyLevelMax,
            TreeLevelIsTooLow,
            hasNotEnoughGold,
            HasNotKillELiteMob,

            OK
        }

        public enum e_CanUnlockNextLevelReturn
        {
            isAlreadyLevelMax,
            hasNotEnoughGear,

            OK
        }

        public List<TreeNode> nodes;

        private int maxLevel = 2;

        private int _level = 0; // 0 : base level
        public int Level
        {
            get
            {
                return _level;
            }
            private set
            {
                _level = value;

                foreach (TreeNode node in nodes)
                {
                    if (node.nodeBranch >= value)
                    {
                        node.State = TreeNode.e_NodeState.unlocked;
                    }
                }

                if (OnChangeBranchLevel != null)
                    OnChangeBranchLevel(value);

            }
        }

        public int[] gearPerLevel; // gears required to unlock the next tier gearPerLevel[0] -> from level 0 to level 1, etc...

        private CurrenciesManager _currenciesManager;

        void Awake()
        {
            _currenciesManager = GetComponentInParent<StrategistManager>().currenciesManager;
        }

        void Start()
        {
            if (!GetComponentInParent<StrategistManager>().GetStrategistPhotonView.isMine)
                return;

            RaiseEventStrategist.BuildTreeData data = new RaiseEventStrategist.BuildTreeData();

            int treeSize = (int)PhotonNetwork.player.customProperties["TreeSize"];

            data.treeId = new int[treeSize];
            data.treeRank = new int[treeSize];

            for (int i = 0; i < treeSize; i++)
            {
                data.treeId[i] = (int)PhotonNetwork.player.customProperties["Tree" + i.ToString() + "id"];
                data.treeRank[i] = (int)PhotonNetwork.player.customProperties["Tree" + i.ToString() + "rank"];
            }

            data.pid = GetComponentInParent<StrategistManager>().GetStrategistPhotonView.viewID;

            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(EventCode.STRAT_CREATE_TREE, data, true, reo);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                UnlockEliteLevel();
        }

        public e_CanUnlockNextLevelReturn canUnlockNextLevel()
        {
            if (Level >= maxLevel)
                return e_CanUnlockNextLevelReturn.isAlreadyLevelMax;
            if (_currenciesManager.currencies[CurrenciesManager.e_Currencies.Gears].Amount < gearPerLevel[Level])
                return e_CanUnlockNextLevelReturn.hasNotEnoughGear;
            return e_CanUnlockNextLevelReturn.OK;
        }

        public void UnlockNextLevel()
        {
            if (canUnlockNextLevel() != e_CanUnlockNextLevelReturn.OK)
                return; // you should have used the checking fct before, you bad programmer...
            _currenciesManager.currencies[CurrenciesManager.e_Currencies.Gears].UseCurrency(gearPerLevel[Level]);
            Level++;
        }

        public void UnlockEliteLevel()
        {
            maxLevel = 3;
        }

        public e_CanAddPointToNodeReturn canAddPointToNode(TreeNode node)
        {
            if (!(node.Level < node.MaxLevel))
                return e_CanAddPointToNodeReturn.isAlreadyLevelMax;
            if (node.EffectiveLevel > Level || node.State == TreeNode.e_NodeState.locked)
                return e_CanAddPointToNodeReturn.TreeLevelIsTooLow;
            if (!_currenciesManager.currencies[CurrenciesManager.e_Currencies.Gold].HasEnoughCurrency(node.pricePerLevel[node.Level]))
                return e_CanAddPointToNodeReturn.hasNotEnoughGold;
            return e_CanAddPointToNodeReturn.OK;
        }

        public bool canRemovePointToNode(TreeNode node)
        {
            return node.Level > 0;
        }

        public void ResetTree()
        {
            foreach (TreeNode node in nodes)
            {
                node.ResetNode();
            }
            Level = 0;
        }

        public void BuildTree(NodeData_Serializable[] tree)
        {
            //int treeSize = (int)PhotonNetwork.player.customProperties["TreeSize"];
            //NodeData_Serializable[] networkTree = new NodeData_Serializable[treeSize];
            //for (int i = 0; i < treeSize; i++)
            //{
            //    networkTree[i] = new NodeData_Serializable();
            //    networkTree[i].id = (int)PhotonNetwork.player.customProperties["Tree" + i.ToString() + "id"];
            //    networkTree[i].rank = (int)PhotonNetwork.player.customProperties["Tree" + i.ToString() + "rank"];
            //}

            Dictionary<int, Transform> skillHolders = this.GetComponentsInChildren<NodeHolder>().ToDictionary(x => x.rank, y => y.transform);

            foreach (var v in tree)
            {
                //Debug.Log("node : " + OpCode_SkillNodes.opCodeToNode[v.id].prefabName);

                GameObject node = Factory.CreateInstanceOf(OpCode_SkillNodes.opCodeToNode[v.id].prefabName);
                node.transform.SetParent(skillHolders[v.rank]);

                TreeNode tn = node.GetComponent<TreeNode>();

                tn.CurrenciesManager = _currenciesManager;
                tn.UniqueID = v.id;
                tn.pricePerLevel = OpCode_SkillNodes.opCodeToNode[v.id].goldPerRank.Clone() as int[];
                node.transform.localScale = Vector3.one;
                node.SendMessage("Init");

                nodes.Add(node.GetComponent<TreeNode>());
            }
        }
    }
}