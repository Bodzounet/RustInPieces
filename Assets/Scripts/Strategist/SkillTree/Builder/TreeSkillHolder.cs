using UnityEngine;
using System.Collections.Generic;

namespace SkillTreeBuilder
{
    public class TreeSkillHolder : MonoBehaviour
    {
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
    }
}