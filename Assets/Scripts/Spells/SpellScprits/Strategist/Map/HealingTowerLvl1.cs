using UnityEngine;
using System.Collections;

public class HealingTowerLvl1 : Spells.ST_BuffOverTime
{
    public int healPerSecond = 5;

    protected override void ContinuousEffect()
    {
        foreach (var v in _baseSpell.TriggerTargets)
        {
            Entity e = v.GetComponent<Entity>();

            if (v != this.gameObject && e is UnitEntity)
            {
                e.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.ADD, healPerSecond, null);
            }
        }
    }

    protected override void OnDispel()
    {
        // no need
    }
}
