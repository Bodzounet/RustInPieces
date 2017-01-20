using UnityEngine;
using System.Collections;
using System;

public class MGuardian_Passive : Spells.ST_Buff
{
    Transform _parent;
    Transform _heroModel;
    Entity _casterEntity;

    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _heroModel = _baseSpell.Caster.transform.Find("Model");

        _parent = transform.parent;
        _parent.parent = _heroModel;
        _parent.transform.localPosition = new Vector3(0, 0, 0);

        _baseSpell.OnTriggerEnterCallback += RaiseDefence;
        _baseSpell.OnTriggerExitCallback.AddListener(LowerDefence);
    }

    protected override void OnDispel()
    {
    }

    public void RaiseDefence(GameObject collidingObject)
    {
        HeroEntity collidingEntity = collidingObject.GetComponent<HeroEntity>();

        if (collidingEntity != null && collidingEntity.Team == _casterEntity.Team)
        {
            _casterEntity.addStatBoostValue(Entity.e_StatType.DEFENSE, 0.25f);
        }
    }

    public void LowerDefence(GameObject collidingObject)
    {
        HeroEntity collidingEntity = collidingObject.GetComponent<HeroEntity>();

        if (collidingEntity != null && collidingEntity.Team == _casterEntity.Team)
        {
            _casterEntity.addStatBoostValue(Entity.e_StatType.DEFENSE, -0.25f);
        }
    }
}
