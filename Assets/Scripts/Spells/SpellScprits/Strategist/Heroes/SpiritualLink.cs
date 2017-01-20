using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class SpiritualLink : Spells.ST_Instant
{
    public GameObject buff;

    protected override void DoAction()
    {
        e_Team team = _baseSpell.Caster.GetComponent<StrategistManager>().Team;

        var linkedEntities = PlayersInfos.Instance.heroList.Where(x => x.GetComponent<Entity>().Team == team).Select(y => y.GetComponent<Entity>());
        Debug.Log("linkedEntities.Count : " + linkedEntities.Count());

        foreach (var v in linkedEntities)
        {
            Debug.Log("current Heros : " + (v as HeroEntity).EntityName);

            var b = v.GetComponent<Spells.BuffManager>().AddBuff(buff, _baseSpell.Caster, v.gameObject) as SpiritualLinkBuff;
            b.links = linkedEntities.Where(x => x != v).ToArray();
            foreach (var w in b.links)
            {
                Debug.Log((w as HeroEntity).EntityName);
            }
        }
    }
}
