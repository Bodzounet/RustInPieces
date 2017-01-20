using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealStormAI : IState
{
    [SerializeField]
    private float fov = 180f;
    [SerializeField]
    private int spellId;

    private SphereCollider col;
    private Transform _target;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<SphereCollider>();
    }

    void Start()
    {
        float maxRangeSpell = _launcher.GetSpellMaxRangeByIndex(spellId);
        if (col.radius < maxRangeSpell)
            col.radius = maxRangeSpell;

    }

    public override void updateState()
    {
        _agent.Stop();
        if (_anim != null)
        {
            _anim.SetBool("IsWalking", false);
            _anim.Play("Attack");
        }
        transform.parent.transform.LookAt(_target);
        _launcher.Launch(_launcher.GetSpellIDByIndex(spellId), new Vector3(_target.position.x, 0, _target.transform.position.z));
    }


    public override bool isTrigger()
    {
        List<UnitInfo> infos = _manager.getAllies(_entity.Team, fov);
        if (infos == null
            || infos.Count == 0
            || _launcher.IsSpellInCooldown(spellId) == false
            || _launcher.HasSpellRessources(spellId) == false)
        {
            _target = null;
            return false;
        }
        UnitInfo bestTarget = null;
        float percent = 0.6f;
        foreach (var unit in infos)
        {
            float tmp = unit.entity.getStat(Entity.e_StatType.HP_CURRENT) / unit.entity.getStat(Entity.e_StatType.HP_MAX);
            if (tmp < percent && _launcher.IsSpellInRange(spellId, transform.position, unit.go.transform.position) == true)
            {
                bestTarget = unit;
                percent = tmp;
            }
        }
        if (bestTarget == null)
        {
            _target = null;
            return false;
        }
        _target = bestTarget.go.transform;
        return true;
    }
}
