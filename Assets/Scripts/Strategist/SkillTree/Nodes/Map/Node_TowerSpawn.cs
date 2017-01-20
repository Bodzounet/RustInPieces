using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SkillTree
{
    public class Node_TowerSpawn : TreeNode
    {
        private Transform _tower;
        private StrategistManager _sm;

        private List<Vector3> _originalSpawnPos = new List<Vector3>();

        void Start()
        {
            _sm = transform.root.GetComponent<StrategistManager>();

            var towersRoot = GameObject.Find("Towers" + (_sm.Team == e_Team.TEAM1 ? "1" : "2")).transform;
            List<Transform> allTowers = new List<Transform>();
            for (int i = 0; i < towersRoot.childCount; i++)
                allTowers.Add(towersRoot.GetChild(i));

            _tower = allTowers.OrderBy(x => Vector3.Distance(x.transform.position, _sm.hq.transform.position)).First();
        }

        protected override void From0To1()
        {
            if (_tower.gameObject == null)
                return;

            for (int i = 0; i < _sm.hq.transform.childCount; i++)
            {
                _originalSpawnPos.Add(_sm.hq.transform.GetChild(i).position);
                _sm.hq.transform.GetChild(i).position = _tower.position;
            }
            _tower.GetComponent<Entity>().OnDeath += CB_OnDeath;
        }

        protected override void From1To0()
        {
            if (_tower.gameObject == null)
                return;

            for (int i = 0; i < _sm.hq.transform.childCount; i++)
            {
                _sm.hq.transform.GetChild(i).position = _originalSpawnPos[i];
            }
            _originalSpawnPos.Clear();
            _tower.GetComponent<Entity>().OnDeath -= CB_OnDeath;
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

        private void CB_OnDeath(GameObject go)
        {
            From1To0();
        }
    }
}