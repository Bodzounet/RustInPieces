using UnityEngine;
using System.Collections;
using System;

public class ProtectiveAuraBuff : Spells.ST_Buff
{
    [HideInInspector]
    public ProtectiveAura.ShieldHolder shieldHolder;

    protected override void CancelEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().removeBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.PROTECTIVE_AURA);
    }

    protected override void DoEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().addBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.PROTECTIVE_AURA, CB_Buff);
    }

    protected override void OnDispel()
    {

    }

    float CB_Buff(float before, float after)
    {
        if (before < after)
        {
            float delta = (before - after);

            shieldHolder.ShieldValueRemaining += delta;
            return before;
        }
        return after;
    }
}
