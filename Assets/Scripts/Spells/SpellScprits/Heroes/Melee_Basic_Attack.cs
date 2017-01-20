using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee_Basic_Attack : Spells.ST_Projectile
{
    [SerializeField]
    GameObject _particles;

    List<GameObject> _enemiesHit = new List<GameObject>();
    Entity _casterEntity;
    float _damages;
    bool _focusUpdated;


    protected override void Start()
    {
        base.Start();
        _focusUpdated = false;
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.MELEE_ATT), Entity.e_AttackType.MELEE);
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
                Instantiate(_particles, this.transform.position, this.transform.rotation);
            }
            if (!_focusUpdated)
            {
                float focus = _casterEntity.getStat(Entity.e_StatType.FOCUS);
                float focusRandom = (Random.Range(0, 20) * focus / 100) - (focus / 10);

                _casterEntity.modifyStat(Entity.e_StatType.FOCUS_STACKS, Entity.e_StatOperator.ADD, focus + focusRandom, _casterEntity);
                _focusUpdated = true;
            }
        }
    }
}
