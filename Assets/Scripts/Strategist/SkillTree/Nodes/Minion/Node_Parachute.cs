using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_Parachute : TreeNode
    {
        SpellBarUIManager spellBar;

        void Start()
        {
            spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
        }

        protected override void From0To1()
        {
            spellBar.AddSpell("ParachuteLvl1UI");
        }

        protected override void From1To0()
        {
            spellBar.RemoveSpell("ParachuteLvl1UI");
        }

        protected override void From1To2()
        {
            spellBar.AddSpell("ParachuteLvl2UI");
            spellBar.RemoveSpell("ParachuteLvl1UI");
        }

        protected override void From2To1()
        {
            spellBar.RemoveSpell("ParachuteLvl2UI");
            spellBar.AddSpell("ParachuteLvl1UI");
        }

        protected override void From2To3()
        {
            spellBar.AddSpell("ParachuteLvl3UI");
            spellBar.RemoveSpell("ParachuteLvl2UI");
        }

        protected override void From3To2()
        {
            spellBar.RemoveSpell("ParachuteLvl3UI");
            spellBar.AddSpell("ParachuteLvl2UI");
        }
    }
}