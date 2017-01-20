using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_ThisOneMustDie : TreeNode
    {
        SpellBarUIManager spellBar;

        void Start()
        {
            spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
        }

        protected override void From0To1()
        {
            spellBar.AddSpell("ThisOneMustDieUI");
        }

        protected override void From1To0()
        {
            spellBar.RemoveSpell("ThisOneMustDieUI");
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
    }
}
