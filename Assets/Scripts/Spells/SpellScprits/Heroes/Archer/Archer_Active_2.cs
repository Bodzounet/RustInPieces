using UnityEngine;
using System.Collections;
using System;

public class Archer_Active_2 : Spells.ST_Projectile
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    [SerializeField]
    GameObject _particles;

    float _damages;
    Entity _casterEntity;

    float _maxTimeCharge;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.RANGE);

        transform.parent.position += Vector3.up * 2;

        _maxTimeCharge = 2;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();
        float chargePercentage = Mathf.Clamp(_baseSpell.TimeCharged / _maxTimeCharge, 0, 1) / 2 + 0.5f;

        if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
        {
            collidingEntity.doDamages(_damages * chargePercentage, Entity.e_AttackType.RANGE, _casterEntity);
            Instantiate(_particles, this.transform.position, this.transform.rotation);
        }
    }
}
