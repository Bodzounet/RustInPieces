using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class OnRampage : Spells.ST_Buff
{
    public StatBooster[] boost;

    private StrategistManager _sm;
    private TeamFactory _tm;

    protected override void Start()
    {
        base.Start();
        _sm = _baseSpell.Caster.GetComponent<StrategistManager>();
        _tm = GameObject.FindObjectsOfType<TeamFactory>().Single(x => x.MyTeam == _sm.Team);
        CurrentState = e_State.ENABLED;
    }

    protected override void CancelEffect()
    {
        foreach (var v in _sm.minionManager.Minions)
        {
            Entity e = v.GetComponent<Entity>();

            foreach (var w in boost)
            {
                e.modifyStat(w.stat, Entity.e_StatOperator.SUBTRACT, w.boostValue, null);
            }
        }

        foreach (var v in (System.Enum.GetValues(typeof(UnitsInfo.e_UnitType)) as UnitsInfo.e_UnitType[]).Where(x => x != UnitsInfo.e_UnitType.NONE))
        {
            _tm.RemoveCallBack(v.ToString(), UpdateNewMinions);
        }
    }

    protected override void DoEffect()
    {
        foreach (var v in _sm.minionManager.Minions)
        {
            Entity e = v.GetComponent<Entity>();

            foreach (var w in boost)
            {
                e.modifyStat(w.stat, Entity.e_StatOperator.ADD, w.boostValue, null);
            }
        }

        foreach (var v in (System.Enum.GetValues(typeof(UnitsInfo.e_UnitType)) as UnitsInfo.e_UnitType[]).Where(x => x != UnitsInfo.e_UnitType.NONE))
        {
            _tm.AddCallBack(v.ToString(), UpdateNewMinions);
        }
    }

    protected override void OnDispel()
    {
        Debug.Log("am i needed ?");
    }

    [System.Serializable]
    public struct StatBooster
    {
        public Entity.e_StatType stat;
        public float boostValue;
    }

    private GameObject UpdateNewMinions(GameObject go)
    {
        Entity e = go.GetComponent<Entity>();
        foreach (var w in boost)
        {
            e.modifyStat(w.stat, Entity.e_StatOperator.ADD, w.boostValue, null);
        }

        return go;
    }
}
