using UnityEngine;
using System.Collections;
using System;

public class ECaster_Passive : Spells.ST_Buff
{
    private Entity _casterEntity;

    private int _stacks;
    public int Stacks
    {
        get { return (_stacks); }
        set
        {
            _stacks = value;
            if (_stacks == 3)
            {
                _casterEntity.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.ADD, 50, _casterEntity);
                _stacks = 0;
            }
        }
    }

    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
    }

    protected override void OnDispel()
    {
    }
}
