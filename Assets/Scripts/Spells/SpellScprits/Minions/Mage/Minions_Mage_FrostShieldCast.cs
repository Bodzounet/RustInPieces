using UnityEngine;
using System.Collections;


public class Minions_Mage_FrostShieldCast : Spells.ST_Instant
{
    [SerializeField]
    private GameObject frostShield;

    protected override void DoAction()
    {
        Spells.BuffManager buffManager = _baseSpell.SpellTargets[0].GetComponent<Spells.BuffManager>();

        buffManager.AddBuff(frostShield, _baseSpell.Caster, _baseSpell.SpellTargets[0].gameObject);
    }
}
