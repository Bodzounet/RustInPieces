using UnityEngine;
using System.Collections;

public class Minions_Healer_HealStorm : Spells.ST_AOE {

    private Entity _casterEntity;

    [SerializeField]
    private float _baseHeal;
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
        //Debug.Log("caster is" + _casterEntity.gameObject.name);
        float finalHeal = _casterEntity.getPercentageOf(_ratioType, _ratio) + _baseHeal;
        foreach (GameObject go in _baseSpell.TriggerTargets)
        {
            //Debug.Log("INSIDE AOE "+go.name);

            Entity entity = go.GetComponent<Entity>();
            if (entity != null && entity.Team == _casterEntity.Team)
            {
                entity.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.ADD, finalHeal, _casterEntity);
            }
        }
    }
}
