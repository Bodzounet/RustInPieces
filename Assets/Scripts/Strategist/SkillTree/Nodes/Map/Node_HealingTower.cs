using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_HealingTower : TreeNode
    {
        StrategistManager _sm;
        Transform _towerParent;

        public GameObject buffLvl1;
        public GameObject buffLvl2;
        public GameObject buffLvl3;

        string _currentBuffId;

        void Start()
        {
            _sm = transform.root.GetComponent<StrategistManager>();
            _towerParent = GameObject.Find("Towers" + (_sm.Team == e_Team.TEAM1 ? "1" : "2")).transform;
        }

        protected override void From0To1()
        {
            _AddBuff(buffLvl1);
        }

        protected override void From1To0()
        {
            _RemoveBuff(buffLvl1);
        }

        protected override void From1To2()
        {
            _RemoveBuff(buffLvl1);
            _AddBuff(buffLvl2);
        }

        protected override void From2To1()
        {
            _RemoveBuff(buffLvl2);
            _AddBuff(buffLvl1);
        }

        protected override void From2To3()
        {
            _RemoveBuff(buffLvl2);
            _AddBuff(buffLvl3);
        }

        protected override void From3To2()
        {
            _RemoveBuff(buffLvl3);
            _AddBuff(buffLvl2);
        }

        private void _AddBuff(GameObject buff)
        {
            for (int i = 0; i < _towerParent.childCount; i++)
            {
                var v = _towerParent.GetChild(i).GetComponent<Spells.BuffManager>().AddBuff(buff, _sm.gameObject, _towerParent.GetChild(i).gameObject);
                _currentBuffId = v.BuffId;
            }
        }

        private void _RemoveBuff(GameObject buff)
        {
            for (int i = 0; i < _towerParent.childCount; i++)
            {
                _towerParent.GetChild(i).GetComponent<Spells.BuffManager>().DeleteBuff(_currentBuffId);
            }
        }
    }
}