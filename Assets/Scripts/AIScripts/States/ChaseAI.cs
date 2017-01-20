using UnityEngine;
using System.Collections;
using System;

public class ChaseAI : IState
{
    [SerializeField]
    private float fov = 180f;
    [SerializeField]
    private float dov = 7.5f;

    private SphereCollider col;
    private GameObject _target = null;
    private e_Team _team;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<SphereCollider>();
        if (col.radius < dov)
            col.radius = dov;
    }

    void OnRemove()
    {
        _agent.Stop();
    }

    public void setSpecificTarget(GameObject target)
    {
        _target = target;
    }

    public override void updateState()
    {
        _agent.Resume();
        _agent.SetDestination(_target.transform.position);
        if (_anim != null)
            _anim.SetBool("IsWalking", true);
      
    }

    public override bool isTrigger()
    {
        UnitInfo info = _manager.getBestEnemy(_entity.Team, fov, dov);
        if (info == null)
        {
            _target = null;
            return false;
        }
        _target = info.go;
        return true;
    }
}
