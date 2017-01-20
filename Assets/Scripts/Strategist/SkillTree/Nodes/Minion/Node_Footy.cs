using UnityEngine;
using System.Collections;
using SkillTree;
using System.Linq;

namespace SkillTree
{
    public class Node_Footy : SkillTree.TreeNode
    {
        private UnitsInfo.e_UnitType unit = UnitsInfo.e_UnitType.MELEE;
        StrategistManager _sm;
        TeamFactory _tf;

        private int _goldWorth = 35; // still need to find a way to reteive it automatically

        Core_MinionManager.CardManager _cardManager;
        public Core_MinionManager.CardManager CardManager
        {
            get
            {
                if (_cardManager == null)
                    _cardManager = GetComponentInParent<StrategistManager>().minionPanelManager.cardManager;
                return _cardManager;
            }

            private set
            {
                _cardManager = value;
            }
        }

        void Start()
        {
            _sm = GetComponentInParent<StrategistManager>();
            _tf = GameObject.FindObjectsOfType<TeamFactory>().Single(x => x.MyTeam == _sm.Team);
        }

        protected override void From0To1()
        {
            CardManager.AddCard(unit);
        }

        protected override void From1To0()
        {
            CardManager.RemoveCard(unit);
        }

        protected override void From1To2()
        {
            CardManager.AddCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Melee_Splash_Attack_Infos");
            }

            _tf.AddCallBack("MELEE", CB_GiveSplashDmg);
        }

        protected override void From2To1()
        {
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().RemoveSpell("Minions_Melee_Splash_Attack");
            }

            _tf.RemoveCallBack("MELEE", CB_GiveSplashDmg);
        }

        protected override void From2To3()
        {
            CardManager.AddCard(unit);
            CardManager.AddCard(unit);

            foreach (var v in _sm.minionManager.Minions.Select(x => x.GetComponent<UnitEntity>()))
            {
                v.GoldWorth = 0;
            }

            _tf.AddCallBack("MELEE", CB_DontGiveGoldAnymore);
        }

        protected override void From3To2()
        {
            CardManager.RemoveCard(unit);
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions.Select(x => x.GetComponent<UnitEntity>()))
            {
                v.GoldWorth = _goldWorth;
            }

            _tf.RemoveCallBack("MELEE", CB_DontGiveGoldAnymore);
        }

        GameObject CB_GiveSplashDmg(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Melee_Splash_Attack_Infos");
            return go;
        }

        GameObject CB_DontGiveGoldAnymore(GameObject go)
        {
            go.GetComponent<UnitEntity>().GoldWorth = 0;
            return go;
        }
    }
}