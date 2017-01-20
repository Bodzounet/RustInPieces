using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeToGame :  Singleton<SkillTreeToGame>  {

	private List<NodeData_Serializable> treeNodes;
	public e_Team team;

	public List<NodeData_Serializable> TreeNodes
	{
		get
		{
			return treeNodes;
		}

		set
		{
			treeNodes = value;
		}
	}





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
