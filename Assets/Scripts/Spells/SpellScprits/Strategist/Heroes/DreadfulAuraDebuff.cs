using UnityEngine;
using System.Collections;
using System;

public class DreadfulAuraDebuff : Spells.ST_Buff
{
    public int speedMalus;
    public int atkMalus;

    protected override void CancelEffect()
    {
        Entity caster = _baseSpell.Caster.GetComponent<Entity>();
        Entity target = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        target.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.ADD, speedMalus, caster);
        target.modifyStat(Entity.e_StatType.RANGE_ATT, Entity.e_StatOperator.ADD, atkMalus, caster);
        target.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.ADD, atkMalus, caster);
        target.modifyStat(Entity.e_StatType.MAGIC_ATT, Entity.e_StatOperator.ADD, atkMalus, caster);
    }

    protected override void DoEffect()
    {
        Entity caster = _baseSpell.Caster.GetComponent<Entity>();
        Entity target = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        target.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.SUBTRACT, speedMalus, caster);
        target.modifyStat(Entity.e_StatType.RANGE_ATT, Entity.e_StatOperator.SUBTRACT, atkMalus, caster);
        target.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.SUBTRACT, atkMalus, caster);
        target.modifyStat(Entity.e_StatType.MAGIC_ATT, Entity.e_StatOperator.SUBTRACT, atkMalus, caster);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }
}
