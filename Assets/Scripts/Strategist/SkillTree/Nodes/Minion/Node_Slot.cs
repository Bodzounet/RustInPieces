using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_Slot : TreeNode
    {
        private Core_MinionManager.MinionPanelManager _mpm;
        public Core_MinionManager.MinionPanelManager Mpm
        {
            get
            {
                if (_mpm == null)
                    _mpm = GetComponentInParent<StrategistManager>().minionPanelManager;
                return _mpm;
            }

            set
            {
                _mpm = value;
            }
        }

        protected override void From0To1()
        {
            Mpm.AddSlot();
            Mpm.AddSlot();
        }

        protected override void From1To0()
        {
            Mpm.RemoveSlot();
            Mpm.RemoveSlot();
        }

        protected override void From1To2()
        {
            Mpm.AddSlot();
            Mpm.AddSlot();
            Mpm.AddSlot();
        }

        protected override void From2To1()
        {
            Mpm.RemoveSlot();
            Mpm.RemoveSlot();
            Mpm.RemoveSlot();
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