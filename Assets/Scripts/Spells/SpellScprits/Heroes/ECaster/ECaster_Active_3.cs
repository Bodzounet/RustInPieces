using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ECaster_Active_3 : Spells.ST_Projectile
{
    [SerializeField]
    float _coneAngle;

    [SerializeField]
    float _maxDistance;

    [SerializeField]
    float _baseDamage;

    [SerializeField]
    float _damageRatio;

    [SerializeField]
    GameObject _particles;

    Entity _casterEntity;
    float _damages;
    Transform _heroModel;
    List<GameObject> _enemiesHit;

    Spells.BuffManager _buffManager;
    ECaster_Passive _passive;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.GetFinalDamage(_baseDamage, _damageRatio, Entity.e_AttackType.MAGIC);
        _heroModel = _baseSpell.Caster.transform.Find("Model");
        _enemiesHit = new List<GameObject>();
        transform.parent.position += Vector3.up * 2;

        _buffManager = _baseSpell.Caster.GetComponent<Spells.BuffManager>();
        _passive = _buffManager.getBuff<ECaster_Passive>() as ECaster_Passive;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        if (!_enemiesHit.Contains(collidingObject))
        {
            Entity collidingEntity = collidingObject.GetComponent<Entity>();

            _enemiesHit.Add(collidingObject);
            if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
            {
                Vector3 targetDir = collidingObject.transform.position - _heroModel.position;
                targetDir.Normalize();

                float dot = Vector3.Dot(targetDir, _heroModel.forward);
                float angleWithTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angleWithTarget <= _coneAngle && Vector3.Distance(_heroModel.position, collidingObject.transform.position) <= _maxDistance)
                {
                    Instantiate(_particles, this.transform.position, this.transform.rotation);
                    collidingEntity.doDamages(_damages, Entity.e_AttackType.MAGIC, _casterEntity);
                    _passive.Stacks++;
                }
            }
        }
    }
}
