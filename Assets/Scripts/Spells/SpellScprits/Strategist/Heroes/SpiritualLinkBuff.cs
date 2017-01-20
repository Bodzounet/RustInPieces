using UnityEngine;
using System.Collections;
using System;

public class SpiritualLinkBuff : Spells.ST_Buff
{
    [HideInInspector]
    public Entity[] links;

    protected override void CancelEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().removeBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.SPIRITUAL_LINK);
    }

    protected override void DoEffect()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().OnDeath += CB_BreakLink;
        _baseSpell.SpellTargets[0].GetComponent<Entity>().addBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.SPIRITUAL_LINK, CB_Buff);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }

    float CB_Buff(float before, float after)
    {
        float damagesTaken = before - after;

        Debug.Log("damages taken : " + damagesTaken);
        Debug.Log("estimated damages : " + (damagesTaken / (links.Length + 1)));

        if (damagesTaken > 0)
        {
            foreach (var v in links)
            {
                v.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, damagesTaken / (links.Length + 1), null, false);
            }
            return damagesTaken / (links.Length + 1);
        }
        return after;
    }

    void CB_BreakLink(GameObject go)
    {
        Debug.Log("breaker : " + go.GetComponent<HeroEntity>().EntityName);
        foreach (var v in links)
        {
            Debug.Log("breaking link with : " + (v as HeroEntity).EntityName);
            (v.GetComponent<Spells.BuffManager>().getBuff<SpiritualLinkBuff>() as SpiritualLinkBuff).Breaklink();
        }
        Breaklink();
    }

    public void Breaklink()
    {
        _baseSpell.SpellTargets[0].GetComponent<Entity>().OnDeath -= CB_BreakLink;
        CancelEffect();
    }
}
