using UnityEngine;
using System.Collections.Generic;

namespace SkillTreeBuilder
{
    public class PoolSkillHolder : MonoBehaviour
    {
        List<Transform> originalChildren = new List<Transform>();

        public int rank;

        public Transform[] CurrentChildren
        {
            get
            {
                List<Transform> children = new List<Transform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    children.Add(transform.GetChild(i));
                }
                return children.ToArray();
            }
        }

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                originalChildren.Add(transform.GetChild(i));
            }
        }

        public void ResetSkillHolder()
        {
            foreach (Transform t in originalChildren)
            {
                t.SetParent(this.transform);
                t.SetAsLastSibling();
            }
        }
    }
}