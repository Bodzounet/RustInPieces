using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_RootTrap : TreeNode
    {
        SpellBarUIManager spellBar;

        void Start()
        {
            spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
        }

        protected override void From0To1()
        {
            spellBar.AddSpell("RootTrapLvl1UI");
        }

        protected override void From1To0()
        {
            spellBar.RemoveSpell("RootTrapLvl1UI");
        }

        protected override void From1To2()
        {
            spellBar.RemoveSpell("RootTrapLvl1UI");
            spellBar.AddSpell("RootTrapLvl2UI");
        }

        protected override void From2To1()
        {
            spellBar.RemoveSpell("RootTrapLvl2UI");
            spellBar.AddSpell("RootTrapLvl1UI");
        }

        protected override void From2To3()
        {
            spellBar.RemoveSpell("RootTrapLvl2UI");
            spellBar.AddSpell("RootTrapLvl3UI");
        }

        protected override void From3To2()
        {
            spellBar.RemoveSpell("RootTrapLvl3UI");
            spellBar.AddSpell("RootTrapLvl2UI");
        }
    }
}