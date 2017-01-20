using UnityEngine;
using System.Collections;
using System;

public class MGuardian_Active_3 : Spells.ST_BuffOverTime
{
    [SerializeField]
    float _distance;

    Transform _heroModel;
    CharacterController _heroCharacterController;
    Vector3 _movement;

    protected override void DoEffect()
    {
        _heroCharacterController = _baseSpell.Caster.GetComponent<CharacterController>();
        _heroModel = _baseSpell.Caster.transform.Find("Model");
        _movement = _heroModel.forward.normalized * _distance * _ticInterval;

        base.DoEffect();
    }

    protected override void ContinuousEffect()
    {
        _heroCharacterController.Move(_movement);
    }

    protected override void CancelEffect()
    {
        base.CancelEffect();
    }

    protected override void OnDispel()
    {
    }
}
