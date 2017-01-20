using UnityEngine;
using System.Collections;
using System;

public class IdleAI : IState
{
    public override bool isTrigger()
    {
        return true;
    }

    public override void updateState()
    {

    }
}
