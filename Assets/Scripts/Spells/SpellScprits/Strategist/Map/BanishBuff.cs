using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Core_MinionManager;

public class BanishBuff : Spells.ST_Buff
{
    protected override void CancelEffect()
    {
        var e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.OnDoDamage -= _BuffEffect;
    }

    protected override void DoEffect()
    {
        var e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.OnDoDamage += _BuffEffect;
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }

    void _BuffEffect(Entity e, float f)
    {
        Vector3 dest = GameObject.FindObjectsOfType<HeadQuarter>().Single(x => x.team == e.Team).transform.position;
        e.transform.position = dest;
        CancelEffect();
    }
}
