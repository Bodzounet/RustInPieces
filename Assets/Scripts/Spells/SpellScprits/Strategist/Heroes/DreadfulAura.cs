using UnityEngine;
using System.Collections;
using System;

public class DreadfulAura : Spells.ST_Aura
{
    public GameObject debuff;
    [HideInInspector]
    public string buffId;

    e_Team team;

    protected override void Start()
    {
        transform.root.SetParent(_baseSpell.SpellTargets[0].transform);

        _baseSpell.OnEndCallback.AddListener(() => Destroy(transform.parent.gameObject));

        buffId = debuff.GetComponentInChildren<Spells.ST_Buff>(true).BuffId;
        team = _baseSpell.Caster.GetComponent<StrategistManager>().Team;
    }

    protected override void GOIsInInfluenceRadius(GameObject go)
    {
        Entity e = go.GetComponent<Entity>();
        if (e.Team != team)
        {
            go.GetComponent<Spells.BuffManager>().AddBuff(debuff, _baseSpell.Caster, go);
        }
    }

    protected override void GOIsNoMoreInInfluenceRadius(GameObject go)
    {
        Spells.BuffManager bm = go.GetComponent<Spells.BuffManager>();
        if (bm.HasBuff(buffId))
            bm.DeleteBuff(buffId);
    }
}
