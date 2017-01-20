using UnityEngine;
using System.Collections;
using System;

public class RootTrapLvl3 : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.setRemainingStateTime(Entity.e_EntityState.STUN, 0);
    }

    protected override void DoEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.addStateTime(Entity.e_EntityState.STUN, _baseSpell.LifeTime);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }
}