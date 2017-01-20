using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class HurryUp : Spells.ST_Buff
{
    [Range(0, 1)]
    public float speedBoostPercentage; // 0 : 0% gain, 1 : 100% gain

    Core_MinionManager.HeadQuarter _hq;

    [SerializeField]
    private GameObject particles;

    protected override void Start()
    {
        base.Start();
        _hq = FindObjectsOfType<Core_MinionManager.HeadQuarter>().Single(x => x.team == _baseSpell.Caster.GetComponent<StrategistManager>().Team);

        CurrentState = e_State.ENABLED;
        Instantiate(particles, _hq.transform.position, new Quaternion(0, 0, 0, 0));
    }

    protected override void CancelEffect()
    {
        _hq.SpawningTime = _hq.BaseSpawningTime;
    }

    protected override void DoEffect()
    {
       _hq.SpawningTime -= speedBoostPercentage * _hq.BaseSpawningTime;
    }

    protected override void OnDispel()
    {
        Debug.Log("am i needed ?");
    }
}
