using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_Herald : SkillTree.TreeNode
    {
        private UnitsInfo.e_UnitType unit = UnitsInfo.e_UnitType.HERAULT;

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
            throw new NotImplementedException();
        }

        protected override void From2To1()
        {
            throw new NotImplementedException();
        }

        protected override void From2To3()
        {
            throw new NotImplementedException();
        }

        protected override void From3To2()
        {
            throw new NotImplementedException();
        }
    }
}