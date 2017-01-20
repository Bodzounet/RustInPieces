using UnityEngine;
using System.Collections;
using System;

public class Poacher_Active_1 : Spells.ST_Buff
{
    [SerializeField]
    float _attackSpeedBoost;

    protected override void CancelEffect()
    {
        HeroEntity heroEntity = _baseSpell.Caster.GetComponent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.ATTACK_SPEED, Entity.e_StatOperator.SUBTRACT, _attackSpeedBoost, heroEntity);
    }

    protected override void DoEffect()
    {
        HeroEntity heroEntity = _baseSpell.Caster.GetComponent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.ATTACK_SPEED, Entity.e_StatOperator.ADD, _attackSpeedBoost, heroEntity);
    }

    protected override void OnDispel()
    {
    }
}
