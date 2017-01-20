using UnityEngine;
using System.Collections;
using System;

public class Warrior_Passive : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        HeroEntity heroEntity = transform.GetComponentInParent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.DEFENSE_PEN, Entity.e_StatOperator.ADD, 10, heroEntity);
    }

    protected override void OnDispel()
    {
    }
}
