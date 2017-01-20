using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Archer_Active_1 : Spells.ST_AOE
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    protected override void DoAction()
    {
        Entity casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        float damages = casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.RANGE);


        _baseSpell.TriggerTargets.Select(target => target.GetComponent<Entity>()).ForEach(targetEntity =>
        {
            if (targetEntity != null && targetEntity.Team != casterEntity.Team)
            {
                targetEntity.doDamages(damages, Entity.e_AttackType.RANGE, casterEntity);
            }
        });
    }
}
