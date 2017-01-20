using UnityEngine;
using System.Collections;

public class SpellStormAI : IState
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
        UnitInfo info = _manager.getBestEnemy(_entity.Team, fov, _launcher.GetSpellMaxRangeByIndex(spellId));
        if (info == null || _launcher.IsSpellInCooldown(spellId) == false || _launcher.IsSpellInRange(spellId, transform.position, info.go.transform.position) == true || _launcher.HasSpellRessources(spellId) == false)
        {
            _target = null;
            return false;
        }
        _target = info.go.transform;
        return true;
    }
}
