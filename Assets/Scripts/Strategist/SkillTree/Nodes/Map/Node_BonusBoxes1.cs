using UnityEngine;
using System.Collections;
using System;

public class Node_BonusBoxes1 : SkillTree.TreeNode
{
    PowerUpHandler g;

    void Start()
    {
        g = transform.root.GetComponentInChildren<PowerUpHandler>();
    }

    protected override void From0To1()
    {
        g.levelBox = 1;    
    }

    protected override void From1To0()
    {
        g.levelBox = 0;
    }

    protected override void From1To2()
    {
        g.levelBox = 2;
    }

    protected override void From2To1()
    {
        g.levelBox = 1;
    }

    protected override void From2To3()
    {
        g.levelBox = 3;
    }

    protected override void From3To2()
    {
        g.levelBox = 2;
    }
}
