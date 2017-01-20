using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SkillTreeBuilder
{
    public class SkillTreeBuilderManager : MonoBehaviour
    {
        public Dictionary<int, PoolSkillHolder> poolSkillHolders = new Dictionary<int, PoolSkillHolder>();
        public Dictionary<int, TreeSkillHolder> treeSkillHolders = new Dictionary<int, TreeSkillHolder>();

        private Transform[] _allNodes;

        private BuildingRules _br;

        public PopUp _popup;

        void Awake()
        {
            foreach (var v in this.GetComponentsInChildren<PoolSkillHolder>())
            {
                poolSkillHolders[v.rank] = v;
            }

            foreach (var v in this.GetComponentsInChildren<TreeSkillHolder>())
            {
                treeSkillHolders[v.rank] = v;
            }

            _allNodes = this.GetComponentsInChildren<NodeData>().Select(x => x.transform).ToArray();

            _br = GetComponent<BuildingRules>();
        }

        public void ResetSkillTreeBuilder()
        {
            foreach (var v in poolSkillHolders)
            {
                v.Value.ResetSkillHolder();
                _br.ResetSkills();
            }
        }

        public Transform GetSpecificNode(int rank, int id)
        {
            NodeData node = _allNodes.Select(x => x.GetComponent<NodeData>()).SingleOrDefault(y => y.rank == rank && y.nodeId == id);
            if (node == null)
                return null;
            return node.transform;
        }
    }
}