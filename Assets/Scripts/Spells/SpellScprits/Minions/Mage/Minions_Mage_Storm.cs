using UnityEngine;
using System.Collections;

public class Minions_Mage_Storm : Spells.ST_AOE {

    private Entity _casterEntity;

    [SerializeField]
    private float _baseDamages;
    [SerializeField]
    private Entity.e_AttackType _attackType;
    [SerializeField]
    private float _ratio;
    [SerializeField]
    private Entity.e_AttackType _ratioType;

    protected override void TriggerAOE()
    {
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        base.TriggerAOE();
    }

    protected override void DoAction()
    {
        float finalDamages = _casterEntity.buffDamages(_casterEntity.getPercentageOf(_ratioType, _ratio) + _baseDamages, _attackType);
        foreach (GameObject go in _baseSpell.TriggerTargets)
        {

            Entity entity = go.GetComponent<Entity>();
            if (entity != null && entity.Team != _casterEntity.Team)
            {
                entity.doDamages(finalDamages, _attackType, _casterEntity);
            }
        }
    }
}
