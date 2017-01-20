using UnityEngine;
using System.Collections;
using System;

public class ECaster_Active_Ult : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
        Entity casterEntity = _baseSpell.Caster.GetComponent<Entity>();

        casterEntity.DamageAmplification.Remove(Spells.BuffKeys.ECASTER_ULT);
    }

    protected override void DoEffect()
    {
        Entity casterEntity = _baseSpell.Caster.GetComponent<Entity>();

        casterEntity.DamageAmplification.Add(Spells.BuffKeys.ECASTER_ULT, DamageBuff);
    }

    protected override void OnDispel()
    {
    }

    public float DamageBuff(float baseDamage, Entity.e_AttackType attackType)
    {
        if (attackType == Entity.e_AttackType.MAGIC)
        {
            return (baseDamage * 2);
        }
        return (baseDamage);
    }
}
