using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGuardian_Active_Ult : Spells.ST_Projectile
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
    }

    protected override void DoAction(GameObject collidingObject)
    {
        if (!_enemiesHit.Contains(collidingObject))
        {
            Entity collidingEntity = collidingObject.GetComponent<Entity>();

            _enemiesHit.Add(collidingObject);
            if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
            {
                collidingEntity.doDamages(_damages, Entity.e_AttackType.MELEE, _baseSpell.Caster.GetComponent<Entity>());
                collidingEntity.addStateTime(Entity.e_EntityState.STUN, 3);
                Instantiate(_particles, collidingObject.transform.position, collidingObject.transform.rotation);
            }
        }
    }
}
