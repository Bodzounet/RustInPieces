using UnityEngine;
using System.Collections;

public class MGuardian_Active_2 : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        _baseSpell.Caster.GetComponent<Entity>().addStateTime(Entity.e_EntityState.BLOCK, 2);
    }

    protected override void OnDispel()
    {
    }
}
