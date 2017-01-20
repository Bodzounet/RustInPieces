using UnityEngine;
using System.Collections;
using System;

public class PatrolAI : IState
{
    [SerializeField]
    private Transform[] wayPoints;

    private int lane;

    private int _currentWPTarget;

    void OnSpeedChange(float bef, float aft)
    {
        _agent.speed = aft;
    }

    protected override void Awake()
    {
        base.Awake();
        _currentWPTarget = 0;
        _agent.autoBraking = false;
    }

    void Start()
    {
        _entity.addCallbackStat(Entity.e_StatType.SPEED, OnSpeedChange);
        OnSpeedChange(0, _entity.getStat(Entity.e_StatType.SPEED));
    }

    public override void updateState()
    {
        if (wayPoints.Length > 0)
        {
            _agent.Resume();
            _agent.SetDestination(wayPoints[_currentWPTarget].position);
            if (_anim != null && _anim.GetBool("IsWalking") == false)
                _anim.SetBool("IsWalking", true);
            if (Vector3.Distance(wayPoints[_currentWPTarget].position, transform.position) <= 2f)
            {
                _currentWPTarget++;
                if (_currentWPTarget == wayPoints.Length)
                    _currentWPTarget = 0;
            }
        }
    }

    public override bool isTrigger()
    {
        return true;
    }

    public Transform[] WayPoint
	{
		get
		{
			return wayPoints;
		}
		set
		{
			wayPoints = value;
		}
	}

    public int Lane
    {
        get
        {
            return lane;
        }

        set
        {
            lane = value;
        }
    }
}
