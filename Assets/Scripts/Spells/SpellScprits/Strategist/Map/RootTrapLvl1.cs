using UnityEngine;
using System.Collections;
using System;

public class RootTrapLvl1 : Spells.ST_Buff
{
    float _quantity;

    protected override void CancelEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.ADD, _quantity, null);
    }

    protected override void DoEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        _quantity = e.getStat(Entity.e_StatType.SPEED) / 2;
        e.modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.SUBTRACT, _quantity, null);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }
}
