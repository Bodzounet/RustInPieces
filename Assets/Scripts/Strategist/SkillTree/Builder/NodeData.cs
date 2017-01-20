using UnityEngine;
using System.Collections;

namespace SkillTreeBuilder
{
    public class NodeData: MonoBehaviour
    {
        public int rank = -1;
        public int nodeId = -1;
    }
}

[System.Serializable]
public struct NodeData_Serializable
{
    public int rank;
    public int id;

    public NodeData_Serializable(int rank_, int id_)
    {
        rank = rank_;
        id = id_;
    }
}