using UnityEngine;
using System.Collections;

public class Minions_Healer_HeroShield : Spells.ST_Buff
{
    [SerializeField]
    private float _baseShieldValue;
    [SerializeField]
    private float _magicRatio;

    private Entity _casterEntity;
    private Entity _targetEntity;
    private float _shieldValue;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDispel()
    {
        CancelEffect();
    }

    float reduceDamages(float initDamages, Entity.e_AttackType type)
    {
        if (type == Entity.e_AttackType.MAGIC || type == Entity.e_AttackType.RANGE)
            initDamages *= 0.8f;
        return initDamages;
    }

    protected override void DoEffect()
    {
        _targetEntity = GetComponentInParent<Entity>();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        if (_targetEntity != null)
        {
            _shieldValue = _baseShieldValue + _casterEntity.getPercentageOf(Entity.e_AttackType.MAGIC, _magicRatio);
            _targetEntity.addShieldValue(Entity.e_AttackType.NEUTRAL, _shieldValue);
            _targetEntity.DamageReduction.Add(Spells.BuffKeys.HEALER_HEROSHIELD, reduceDamages);
        }
    }

    protected override void CancelEffect()
    {
        _targetEntity.addShieldValue(Entity.e_AttackType.NEUTRAL, _shieldValue * -1);
        _targetEntity.DamageReduction.Remove(Spells.BuffKeys.HEALER_HEROSHIELD);
    }
}
