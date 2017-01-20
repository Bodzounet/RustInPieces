using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace SkillTree
{
    public class Node_BackFire : TreeNode
    {
        StrategistManager _sm;
        Transform _towerParent;

        public GameObject buff;

        string _currentBuffId;

        void Start()
        {
            _sm = transform.root.GetComponent<StrategistManager>();
            _towerParent = GameObject.Find("Towers" + (_sm.Team == e_Team.TEAM1 ? "1" : "2")).transform;
        }

        protected override void From0To1()
        {
            for (int i = 0; i < _towerParent.childCount; i++)
            {
                var v = _towerParent.GetChild(i).GetComponent<Spells.BuffManager>().AddBuff(buff, _sm.gameObject, _towerParent.GetChild(i).gameObject);
                _currentBuffId = v.BuffId;
            }
        }

        protected override void From1To0()
        {
            for (int i = 0; i < _towerParent.childCount; i++)
            {
                _towerParent.GetChild(i).GetComponent<Spells.BuffManager>().DeleteBuff(_currentBuffId);
            }
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