using UnityEngine;
using System.Collections;
using System;

public class Warrior_Active_1 : Spells.ST_Buff
{
    [SerializeField]
    private GameObject _warriorActive1Debuff;

    protected override void CancelEffect()
    {
    }

    protected override void DoEffect()
    {
    }

    protected override void OnDispel()
    {
    }

    public void ApplyDebuff(GameObject target)
    {
        _baseSpell.Caster.GetComponent<Spells.BuffManager>().AddBuff(_warriorActive1Debuff, _baseSpell.Caster, target);
    }
}
