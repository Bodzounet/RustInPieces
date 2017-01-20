using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SkillTree
{
    public class Node_DivineShield : SkillTree.TreeNode
    {
        SpellBarUIManager spellBar;

        void Start()
        {
            spellBar = transform.root.GetComponentInChildren<SpellBarUIManager>();
        }

        protected override void From0To1()
        {
            spellBar.AddSpell("DivineShieldLvl1UI");
        }

        protected override void From1To0()
        {
            spellBar.RemoveSpell("DivineShieldLvl1UI");
        }

        protected override void From1To2()
        {
            spellBar.RemoveSpell("DivineShieldLvl1UI");
            spellBar.AddSpell("DivineShieldLvl2UI");
        }

        protected override void From2To1()
        {
            spellBar.RemoveSpell("DivineShieldLvl2UI");
            spellBar.AddSpell("DivineShieldLvl1UI");
        }

        protected override void From2To3()
        {
            spellBar.RemoveSpell("DivineShieldLvl2UI");
            spellBar.AddSpell("DivineShieldLvl3UI");
        }

        protected override void From3To2()
        {
            spellBar.RemoveSpell("DivineShieldLvl3UI");
            spellBar.AddSpell("DivineShieldLvl2UI");
        }
    }
}
