using UnityEngine;
using System.Collections;
using System;

public class DivineShieldLvl1 : Spells.ST_Instant
{
    public int ShieldAmount;

    protected override void DoAction()
    {
        var entity = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        entity.addShieldValue(Entity.e_AttackType.NEUTRAL, entity.getStat(Entity.e_StatType.HP_MAX) *ShieldAmount);
    }
}
