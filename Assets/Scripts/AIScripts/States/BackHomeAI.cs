using UnityEngine;
using System.Collections;
using System;

public class BackHomeAI : IState
{
    public GameObject home;

    public override bool isTrigger()
    {
        float dist = _agent.remainingDistance;
        if (dist <= 0.5)
        {
            _anim.SetBool("IsWalking", false);
            return false;
        }
        return true;
    }

    public override void updateState()
    {
        _agent.Resume();
        _agent.SetDestination(home.transform.position);
        if (_anim != null)
            _anim.SetBool("IsWalking", true);
    }

}
