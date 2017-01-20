using UnityEngine;
using System.Collections;
using System;

namespace SkillTree
{
    public class Node_TowerShield : TreeNode
    {
        SpellBarUIManager spellBar;

        void Start()
        {
            spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
        }

        protected override void From0To1()
        {
            spellBar.AddSpell("TowerShieldLvl1UI");
        }

        protected override void From1To0()
        {
            spellBar.RemoveSpell("TowerShieldLvl1UI");
        }

        protected override void From1To2()
        {
            spellBar.RemoveSpell("TowerShieldLvl1UI");
            spellBar.AddSpell("TowerShieldLvl2UI");
        }

        protected override void From2To1()
        {
            spellBar.RemoveSpell("TowerShieldLvl2UI");
            spellBar.AddSpell("TowerShieldLvl1UI");
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