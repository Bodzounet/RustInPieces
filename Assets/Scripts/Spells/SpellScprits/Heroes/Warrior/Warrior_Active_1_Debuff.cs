using UnityEngine;
using System.Collections;
using System;

public class Warrior_Active_1_Debuff : Spells.ST_BuffOverTime
{
    [SerializeField]
    private float _damages;
    private float _damagesPerTic;
    private Entity _target;

    protected override void ContinuousEffect()
    {
        if (_target == null)
        {
            _target = GetComponentInParent<Entity>();
            _damagesPerTic = _damages / (_baseSpell.LifeTime / _ticInterval);
        }
        _target.doDamages(_damagesPerTic, Entity.e_AttackType.MELEE, _baseSpell.Caster.GetComponent<Entity>());
    }

    protected override void OnDispel()
    {
    }
}
