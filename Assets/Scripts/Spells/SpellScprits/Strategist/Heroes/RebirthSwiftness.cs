using UnityEngine;
using System.Collections;
using System;

public class RebirthSwiftness : Spells.ST_Buff
{
    public float speedBoost;

    protected override void CancelEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.SUBTRACT, speedBoost, null);
    }

    protected override void DoEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().modifyStat(Entity.e_StatType.SPEED, Entity.e_StatOperator.ADD, speedBoost, null);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }
}
