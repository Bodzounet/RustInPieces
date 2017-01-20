using UnityEngine;
using System.Collections;
using System;

public class BackFire : Spells.ST_Buff
{
    public int damageIncrease = 1;
    public int timeBeforeCancellingBonus = 5;

    private int cumulativeBonus = 0;

    Entity me;

    protected override void CancelEffect()
    {
        me.OnHit -= Cb_OnHit;
        me.DamageAmplification.Remove(Spells.BuffKeys.TOWER_BACKFIRE);
    }

    protected override void DoEffect()
    {
        me = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        me.OnHit += Cb_OnHit;
        me.DamageAmplification.Add(Spells.BuffKeys.TOWER_BACKFIRE, Cb_BoostDamages);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }

    private void Cb_OnHit(float before, float after, Entity.e_StatType type)
    {
        if (before > after && type == Entity.e_StatType.HP_CURRENT)
        {
            StopCoroutine("Co_ResetBonus");
            cumulativeBonus += damageIncrease;
            StartCoroutine("Co_ResetBonus");
        }
    }

    private float Cb_BoostDamages(float baseDamages, Entity.e_AttackType at)
    {
        return baseDamages + cumulativeBonus;
    }

    private IEnumerator Co_ResetBonus()
    {
        yield return new WaitForSeconds(timeBeforeCancellingBonus);
        cumulativeBonus = 0;
    }
}
