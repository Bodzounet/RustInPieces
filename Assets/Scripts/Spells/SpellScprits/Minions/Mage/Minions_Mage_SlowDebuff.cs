using UnityEngine;
using System.Collections;

public class Minions_Mage_SlowDebuff : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
        Entity targetEntity = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        targetEntity.addStatBoostValue(Entity.e_StatType.SPEED, 0.15f);
    }

    protected override void DoEffect()
    {
        Entity targetEntity = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        targetEntity.addStatBoostValue(Entity.e_StatType.SPEED, -0.15f);
    }

    protected override void OnDispel()
    {
        CancelEffect();
    }
}