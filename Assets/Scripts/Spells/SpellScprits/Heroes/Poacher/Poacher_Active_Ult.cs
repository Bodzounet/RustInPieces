using UnityEngine;
using System.Collections;
using System;

public class Poacher_Active_Ult : Spells.ST_Buff
{
    [SerializeField]
    float _lifeDrainBoost;

    [SerializeField]
    float _speedBoost;

    [SerializeField]
    float _meleeDamageBoost;

    protected override void CancelEffect()
    {
        HeroEntity heroEntity = _baseSpell.Caster.GetComponent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.LIFE_DRAIN, Entity.e_StatOperator.SUBTRACT, _lifeDrainBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.SUBTRACT, _speedBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.SUBTRACT, _meleeDamageBoost, heroEntity);
    }

    protected override void DoEffect()
    {
        HeroEntity heroEntity = _baseSpell.Caster.GetComponent<HeroEntity>();

        heroEntity.modifyStat(Entity.e_StatType.LIFE_DRAIN, Entity.e_StatOperator.ADD, _lifeDrainBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.ADD, _speedBoost, heroEntity);
        heroEntity.modifyStat(Entity.e_StatType.MELEE_ATT, Entity.e_StatOperator.ADD, _meleeDamageBoost, heroEntity);
    }

    protected override void OnDispel()
    {
    }
}
