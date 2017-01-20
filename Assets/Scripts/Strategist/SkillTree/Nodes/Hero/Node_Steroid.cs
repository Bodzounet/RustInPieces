using UnityEngine;
using System.Collections;
using System;

public class Node_Steroid : SkillTree.TreeNode
{
    SpellBarUIManager spellBar;

    void Start()
    {
        spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
    }

    protected override void From0To1()
    {
        spellBar.AddSpell("SteroidLvl1UI");
    }

    protected override void From1To0()
    {
        spellBar.RemoveSpell("SteroidLvl1UI");
    }

    protected override void From1To2()
    {
        spellBar.RemoveSpell("SteroidLvl1UI");
        spellBar.AddSpell("SteroidLvl2UI");
    }

    protected override void From2To1()
    {
        spellBar.RemoveSpell("SteroidLvl2UI");
        spellBar.AddSpell("SteroidLvl1UI");
    }

    protected override void From2To3()
    {
        spellBar.RemoveSpell("SteroidLvl2UI");
        spellBar.AddSpell("SteroidLvl3UI");
    }

    protected override void From3To2()
    {
        spellBar.RemoveSpell("SteroidLvl3UI");
        spellBar.AddSpell("SteroidLvl2UI");
    }
}
