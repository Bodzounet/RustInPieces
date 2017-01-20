using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warrior_Active_Ult : Spells.ST_Projectile
{
    [SerializeField]
    float _coneAngle;

    [SerializeField]
    float _firstZoneDistance;

    [SerializeField]
    float _firstZoneBaseDamage;

    [SerializeField]
    float _firstZoneDamageRatio;

    [SerializeField]
    float _secondZoneDistance;

    [SerializeField]
    float _secondZoneBaseDamage;

    [SerializeField]
    float _secondZoneDamageRatio;

    [SerializeField]
    GameObject _particles;

    float _firstZoneDamage;
    float _secondZoneDamage;
    Transform _heroModel;
    Entity _casterEntity;
    List<GameObject> _enemiesHit = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _firstZoneDamage = _casterEntity.GetFinalDamage(_firstZoneDamage, _firstZoneDamageRatio, Entity.e_AttackType.MELEE);
        _secondZoneDamage = _casterEntity.GetFinalDamage(_secondZoneBaseDamage, _secondZoneDamageRatio, Entity.e_AttackType.MELEE);

        _heroModel = _baseSpell.Caster.transform.Find("Model");
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
                Vector3 targetDir = collidingObject.transform.position - _heroModel.position;
                targetDir.Normalize();

                float dot = Vector3.Dot(targetDir, _heroModel.forward);
                float angleWithTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angleWithTarget <= _coneAngle &&
                    Vector3.Distance(_heroModel.position, collidingObject.transform.position) <= _firstZoneDistance)
                {
                    collidingEntity.doDamages(_firstZoneDamage, Entity.e_AttackType.MELEE, _casterEntity);
                    Instantiate(_particles, this.transform.position, this.transform.rotation);
                }
                else if (angleWithTarget <= _coneAngle &&
                    Vector3.Distance(_heroModel.position, collidingObject.transform.position) <= _secondZoneDistance)
                {
                    collidingEntity.doDamages(_secondZoneDamage, Entity.e_AttackType.MELEE, _casterEntity);
                    Instantiate(_particles, this.transform.position, this.transform.rotation);
                }
            }
        }
    }
}
