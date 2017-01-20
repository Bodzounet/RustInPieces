using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Poacher_Active_3 : Spells.ST_Projectile
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    [SerializeField]
    float _stunDuration;

    [SerializeField]
    GameObject _particles;

    List<GameObject> _enemiesHit = new List<GameObject>();
    Entity _casterEntity;
    float _damages;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.MELEE);
        transform.parent.position += Vector3.up * 2;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        if (!_enemiesHit.Contains(collidingObject))
        {
            Entity collidingEntity = collidingObject.GetComponent<Entity>();

            _enemiesHit.Add(collidingObject);
            if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
            {
                collidingEntity.doDamages(_damages, Entity.e_AttackType.MELEE, _casterEntity);
                collidingEntity.addStateTime(Entity.e_EntityState.STUN, _stunDuration);
                Instantiate(_particles, this.transform.position, Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0, 90 ,0)));
            }
        }
    }
}
