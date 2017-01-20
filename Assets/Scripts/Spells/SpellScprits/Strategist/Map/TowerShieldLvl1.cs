using UnityEngine;
using System.Collections;
using System;

public class TowerShieldLvl1 : Spells.ST_Buff
{
    public float shieldProvided;
    public float explosionDamage;
    public float explosionRadius;

    protected override void CancelEffect()
    {
        var targets = Physics.OverlapSphere(transform.root.position, explosionRadius, 1 << LayerMask.NameToLayer("HeroEntity") | 1 << LayerMask.NameToLayer("MinionEntity") | 1 << LayerMask.NameToLayer("Entity"));
        foreach (var v in targets)
        {
            v.GetComponent<Entity>().doDamages(explosionDamage, Entity.e_AttackType.NEUTRAL, null);
        }
    }

    protected override void DoEffect()
    {
        Entity e = _baseSpell.SpellTargets[0].GetComponent<Entity>();
        e.addShieldValue(Entity.e_AttackType.NEUTRAL, shieldProvided);
    }

    protected override void OnDispel()
    {
        throw new NotImplementedException();
    }
}
