using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warrior_Basic_Attack : Spells.ST_Projectile
{
    [SerializeField]
    GameObject _particles;

    List<GameObject> _enemiesHit = new List<GameObject>();
    Transform _parent;
    Transform _heroModel;
    Entity _casterEntity;
    float _damages;
    bool _focusUpdated;

    Warrior_Active_1 _warriorActive1Buff;

    protected override void Start()
    {
        base.Start();
        _focusUpdated = false;
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.MELEE_ATT), Entity.e_AttackType.MELEE);

        _heroModel = _baseSpell.Caster.transform.Find("Model");

        _parent = transform.parent;
        _parent.parent = _heroModel.transform;
        _parent.transform.localPosition = new Vector3(0, 0, 2);
        _parent.transform.localEulerAngles = Vector3.zero;

        _warriorActive1Buff = _baseSpell.Caster.GetComponent<Spells.BuffManager>().getBuff<Warrior_Active_1>() as Warrior_Active_1;
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
                if (_warriorActive1Buff != null)
                {
                    _warriorActive1Buff.ApplyDebuff(collidingObject);
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
}
