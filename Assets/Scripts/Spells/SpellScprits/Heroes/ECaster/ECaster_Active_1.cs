using UnityEngine;
using System.Collections;
using System;

public class ECaster_Active_1 : Spells.ST_Projectile
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    [SerializeField]
    GameObject _particles;

    float _damages;
    Entity _casterEntity;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.MAGIC);
        transform.parent.position += Vector3.up * 2;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();

        if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
        {
            Spells.BuffManager _buffManager = _baseSpell.Caster.GetComponent<Spells.BuffManager>();
            ECaster_Passive passive = _buffManager.getBuff<ECaster_Passive>() as ECaster_Passive;

            collidingEntity.doDamages(_damages, Entity.e_AttackType.MAGIC, _casterEntity);
            Instantiate(_particles, this.transform.position, this.transform.rotation);
            passive.Stacks++;
            OnEndCallback();
        }
    }
}
