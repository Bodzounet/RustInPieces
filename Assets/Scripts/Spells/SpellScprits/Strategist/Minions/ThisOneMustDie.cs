using UnityEngine;
using System.Collections;
using System;

public class ThisOneMustDie : Spells.ST_Buff
{
    public float percentageDgmIncrease = 0.5f;

    protected override void CancelEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().DamageAmplification.Add(Spells.BuffKeys.THIS_ONE_MUST_DIE, _Debuff);
    }

    protected override void DoEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().DamageAmplification.Remove(Spells.BuffKeys.THIS_ONE_MUST_DIE);
    }

    protected override void OnDispel()
    {
        Debug.Log("am i needed ?");
    }

    private float _Debuff(float dmg, Entity.e_AttackType atkType)
    {
        return dmg * (1 + percentageDgmIncrease);
    }
}
