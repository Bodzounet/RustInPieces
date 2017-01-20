using UnityEngine;
using System.Collections;
using System;

public class HealingTowerLvl2 : Spells.ST_BuffOverTime
{
    public int healPerSecond = 5;

    protected override void ContinuousEffect()
    {
        foreach (var v in _baseSpell.TriggerTargets)
        {
            if (v != this.gameObject)
            {
                v.GetComponent<Entity>().modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.ADD, healPerSecond, null);
            }
        }
    }

    protected override void OnDispel()
    {
        // no need
    }
}
