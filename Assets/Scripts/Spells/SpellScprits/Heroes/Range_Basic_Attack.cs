using UnityEngine;
using System.Collections;

public class Range_Basic_Attack : Spells.ST_Projectile
{
    [SerializeField]
    GameObject _particles;

    Entity _casterEntity;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        transform.parent.position += Vector3.up * 2;
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();
        float damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.RANGE_ATT), Entity.e_AttackType.RANGE);

        if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
        {
            float focus = _casterEntity.getStat(Entity.e_StatType.FOCUS);
            float focusRandom = (Random.Range(0, 20) * focus / 100) - (focus / 10);

            _casterEntity.modifyStat(Entity.e_StatType.FOCUS_STACKS, Entity.e_StatOperator.ADD, focus + focusRandom, _casterEntity);
            collidingEntity.doDamages(damages, Entity.e_AttackType.RANGE, _casterEntity);
            Instantiate(_particles, transform.position, transform.rotation);
            OnEndCallback();
        }
    }
}
