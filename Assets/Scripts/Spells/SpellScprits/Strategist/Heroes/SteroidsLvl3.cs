using UnityEngine;
using System.Collections;
using System;

public class SteroidsLvl3 : Spells.ST_Buff
{
    public int atkBoost;
    public int defBoost;

    protected override void CancelEffect()
    {
        Entity caster = _baseSpell.Caster.GetComponent<Entity>();
        Entity target = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        target.modifyStat(Entity.e_StatType.RANGE_ATT, Entity.e_StatOperator.SUBTRACT, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.SUBTRACT, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.MAGIC_ATT, Entity.e_StatOperator.SUBTRACT, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.DEFENSE, Entity.e_StatOperator.SUBTRACT, defBoost, caster);
    }

    protected override void DoEffect()
    {
        Entity caster = _baseSpell.Caster.GetComponent<Entity>();
        Entity target = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        target.modifyStat(Entity.e_StatType.RANGE_ATT, Entity.e_StatOperator.ADD, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.ADD, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.MAGIC_ATT, Entity.e_StatOperator.ADD, atkBoost, caster);
        target.modifyStat(Entity.e_StatType.DEFENSE, Entity.e_StatOperator.ADD, defBoost, caster);
    }

    protected override void OnDispel()
    {
    }
}
