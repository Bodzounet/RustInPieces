using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace SkillTree
{
    public class Node_Archer : SkillTree.TreeNode
    {
        private UnitsInfo.e_UnitType unit = UnitsInfo.e_UnitType.RANGE;

        StrategistManager _sm;
        TeamFactory _tf;

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
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Range_Bounce_Attack_Infos");
            }

            _tf.AddCallBack("RANGE", CB_GiveBouncingAttack);
        }

        protected override void From2To1()
        {
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().RemoveSpell("Minions_Range_Bounce_Attack");
            }

            _tf.RemoveCallBack("RANGE", CB_GiveBouncingAttack);
        }

        protected override void From2To3()
        {
            CardManager.AddCard(unit);
            CardManager.AddCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Range_BounceHero_Attack_Infos");
            }

            _tf.AddCallBack("RANGE", CB_GiveDoubleDmgOnHeroes);
        }

        protected override void From3To2()
        {
            CardManager.RemoveCard(unit);
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().RemoveSpell("Minions_Range_BounceHero_Attack");
            }

            _tf.RemoveCallBack("RANGE", CB_GiveDoubleDmgOnHeroes);
        }

        private GameObject CB_GiveBouncingAttack(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Range_Bounce_Attack_Infos");
            return go;
        }

        private GameObject CB_GiveDoubleDmgOnHeroes(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Range_BounceHero_Attack_Infos");
            return go;
        }
    }
}