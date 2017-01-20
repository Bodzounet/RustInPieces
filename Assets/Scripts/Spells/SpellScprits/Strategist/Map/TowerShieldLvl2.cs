using UnityEngine;
using System.Collections;
using System;

public class TowerShieldLvl2 : Spells.ST_Buff
{
    public float shieldProvided;
    public float explosionDamage;
    public float explosionRadius;

    public float backFireDamages;

    protected override void CancelEffect()
    {
        var targets = Physics.OverlapSphere(transform.root.position, explosionRadius, 1 << LayerMask.NameToLayer("HeroEntity") | 1 << LayerMask.NameToLayer("MinionEntity") | 1 << LayerMask.NameToLayer("Entity"));
        foreach (var v in targets)
        {
            v.GetComponent<Entity>().doDamages(explosionDamage, Entity.e_AttackType.NEUTRAL, null);
        }
        _baseSpell.SpellTargets[0].GetComponent<Entity>().removeBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.TOWER_SHIELD_LEVEL_2);
    }

    protected override void DoEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.addShieldValue(Entity.e_AttackType.NEUTRAL, shieldProvided);
        e.addBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.TOWER_SHIELD_LEVEL_2, _BuffEffect);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }

    float _BuffEffect(float before, float after)
    {
        if (before - after < 0)
        {
            Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
            e.Hitter.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, backFireDamages, e);
        }
        return after;
    }
}
