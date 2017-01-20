using UnityEngine;
using System.Collections;
using System;

public class Archer_Active_Ult_Proj : Spells.ST_Projectile
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    float _damages;
    Entity _casterEntity;

    protected override void Start()
    {
        base.Start();

        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.RANGE);
        transform.parent.position += Vector3.up * 2;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<HeroEntity>();

        if (collidingObject == _baseSpell.SpellTargets[0])
        {
            collidingEntity.doDamages(_damages, Entity.e_AttackType.RANGE, _casterEntity);
            OnEndCallback();
        }
    }
}
