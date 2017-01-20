using UnityEngine;
using System.Collections;

public class Minions_Healer_Passiv : Spells.ST_Aura
{
    [SerializeField]
    private float _regenAmount;

    private Entity _casterEntity = null;
    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
    }

    protected override void GOIsInInfluenceRadius(GameObject go)
    {
        Entity target = go.GetComponent<Entity>();
        if (_casterEntity == null)
            _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        if (target != null && target.Team == _casterEntity.Team)
        {
            target.modifyStat(Entity.e_StatType.REGEN_HP, Entity.e_StatOperator.ADD, _regenAmount, _casterEntity);
        }
    }

    protected override void GOIsNoMoreInInfluenceRadius(GameObject go)
    {
        Entity target = go.GetComponent<Entity>();
        if (target != null && target.Team == _casterEntity.Team)
        {
            target.modifyStat(Entity.e_StatType.REGEN_HP, Entity.e_StatOperator.SUBTRACT, _regenAmount, _casterEntity);
        }
    }
}