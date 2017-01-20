using UnityEngine;
using System.Collections;
using System;

public class Archer_Passive : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        HeroEntity heroEntity = _baseSpell.Caster.GetComponent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.FOCUS, Entity.e_StatOperator.ADD, 10, heroEntity);
    }

    protected override void OnDispel()
    {
    }
}
