using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace SkillTree
{
    public class Node_Wizard : SkillTree.TreeNode
    {
        private UnitsInfo.e_UnitType unit = UnitsInfo.e_UnitType.MAGIC;

        Core_MinionManager.CardManager _cardManager;
        private StrategistManager _sm;
        private TeamFactory _tf;

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
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Mage_FrostShieldCast_Infos");
            }

            _tf.AddCallBack("WIZARD", CB_GiveShieldSpell);
        }

        protected override void From2To1()
        {
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().RemoveSpell("Minions_Mage_FrostShieldCast");
            }

            _tf.RemoveCallBack("WIZARD", CB_GiveShieldSpell);
        }

        protected override void From2To3()
        {
            CardManager.AddCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Mage_BasicSlow_Attack_Infos");
            }

            _tf.AddCallBack("WIZARD", CB_GiveSlowingAttack);
        }

        protected override void From3To2()
        {
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().RemoveSpell("Minions_Mage_BasicSlow_Attack");
            }

            _tf.RemoveCallBack("WIZARD", CB_GiveSlowingAttack);
        }

        private GameObject CB_GiveSlowingAttack(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Mage_BasicSlow_Attack_Infos");
            return go;
        }

        private GameObject CB_GiveShieldSpell(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Mage_FrostShieldCast_Infos");
            return go;
        }

    }
}