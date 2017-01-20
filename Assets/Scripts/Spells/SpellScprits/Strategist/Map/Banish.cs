using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Banish : Spells.ST_Instant
{
    public GameObject buff;

    protected override void DoAction()
    {
        e_Team myTeam = _baseSpell.Caster.GetComponent<StrategistManager>().Team;
        foreach (var ally in PlayersInfos.Instance.heroList.Where(x => x.GetComponent<Entity>().Team == myTeam))
        {
            ally.GetComponent<Spells.BuffManager>().AddBuff(buff, _baseSpell.Caster, ally);
        }
    }
}
