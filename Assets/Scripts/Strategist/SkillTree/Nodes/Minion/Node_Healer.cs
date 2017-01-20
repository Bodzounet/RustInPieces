﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace SkillTree
{
    public class Node_Healer : SkillTree.TreeNode
    {
        private UnitsInfo.e_UnitType unit = UnitsInfo.e_UnitType.MEDIC;

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
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Healer_Passiv_Infos");
            }

            _tf.AddCallBack("HEALER", CB_GiveHealerPassiveSpell);
        }

        protected override void From2To1()
        {
            CardManager.RemoveCard(unit);

            foreach (var v in _sm.minionManager.Minions)
            {
                v.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Healer_Passiv");
            }

            _tf.AddCallBack("HEALER", CB_GiveHealerPassiveSpell);
        }

        protected override void From2To3()
        {
            throw new NotImplementedException();
        }

        protected override void From3To2()
        {
            throw new NotImplementedException();
        }

        private GameObject CB_GiveHealerPassiveSpell(GameObject go)
        {
            go.GetComponent<Spells.SpellLauncher>().AddSpell("Minions_Healer_Passiv_Infos");
            return go;
        }
    }
}