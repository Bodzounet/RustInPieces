using UnityEngine;
using System.Collections;

public class Minions_Tank_Passive : Spells.ST_Buff
{
    Entity _casterEntity;

    [SerializeField]
    float _dammage = 10;

    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _casterEntity.addBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.TANK_PASSIVE_REFLECT, reflect);
    }

    protected override void OnDispel()
    {
    }

    float reflect(float before, float after)
    {
        if (before > after)
            return after - _dammage;
        else
            return after;
    }
    
}

