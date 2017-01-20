using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ECaster_Active_2 : Spells.ST_Projectile
{
    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    [SerializeField]
    float _stunTime;

    List<GameObject> _enemiesHit = new List<GameObject>();
    Entity _casterEntity;
    bool _activated;
    float _damages;

    Spells.BuffManager _buffManager;
    ECaster_Passive _passive;

    protected override void Start()
    {
        base.Start();

        Invoke("Activation", 2f);
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.MAGIC);
        this.GetComponent<SphereCollider>().enabled = false;

        _buffManager = _baseSpell.Caster.GetComponent<Spells.BuffManager>();
        _passive = _buffManager.getBuff<ECaster_Passive>() as ECaster_Passive;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        if (_activated && !_enemiesHit.Contains(collidingObject))
        {
            Entity collidingEntity = collidingObject.GetComponent<Entity>();

            _enemiesHit.Add(collidingObject);
            if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
            {
                collidingEntity.doDamages(_damages, Entity.e_AttackType.MAGIC, _casterEntity);
                collidingEntity.addStateTime(Entity.e_EntityState.STUN, _stunTime);
                _passive.Stacks++;
            }
        }
    }

    void Activation()
    {
        _activated = true;
        this.GetComponent<SphereCollider>().enabled = true;
        Invoke("OnEndCallback", 0.1f);
    }
}
