using UnityEngine;
using System.Collections;
using System;

public class Warrior_Active_2 : Spells.ST_Buff
{
    [SerializeField]
    float _attackBoost;

    [SerializeField]
    float _attackSpeedBoost;

    protected override void CancelEffect()
    {
        HeroEntity heroEntity = transform.GetComponentInParent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.SUBTRACT, _attackBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.ATTACK_SPEED, Entity.e_StatOperator.SUBTRACT, _attackSpeedBoost, heroEntity);
    }

    protected override void DoEffect()
    {
        HeroEntity heroEntity = transform.GetComponentInParent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.ADD, _attackBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.ATTACK_SPEED, Entity.e_StatOperator.ADD, _attackSpeedBoost, heroEntity);
    }

    protected override void OnDispel()
    {
    }
}
