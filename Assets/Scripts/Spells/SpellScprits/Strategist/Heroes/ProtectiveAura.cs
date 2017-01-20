using UnityEngine;
using System.Collections;
using System;

public class ProtectiveAura : Spells.ST_Aura
{
    public int shieldValue = 1000;

    public GameObject buff;
    [HideInInspector]
    public string buffId;

    e_Team team;

    private ShieldHolder _shieldHolder;

    protected override void Start()
    {
        transform.root.SetParent(_baseSpell.SpellTargets[0].transform);

        _baseSpell.OnEndCallback.AddListener(() => Destroy(transform.parent.gameObject));

        buffId = buff.GetComponentInChildren<Spells.ST_Buff>(true).BuffId;
        team = _baseSpell.Caster.GetComponent<StrategistManager>().Team;

        _shieldHolder = new ShieldHolder();
        _shieldHolder.ShieldValueRemaining = shieldValue;
        _shieldHolder.OnNoMoreShield += () =>
        {
            _baseSpell.StopCoroutine("Co_LifeTime");
            _baseSpell.End();
        };
    }

    protected override void GOIsInInfluenceRadius(GameObject go)
    {
        Entity e = go.GetComponent<Entity>();
        if (e.Team == team)
        {
            ProtectiveAuraBuff b = go.GetComponent<Spells.BuffManager>().AddBuff(buff, _baseSpell.Caster, go) as ProtectiveAuraBuff;
            b.shieldHolder = _shieldHolder;
        }
    }

    protected override void GOIsNoMoreInInfluenceRadius(GameObject go)
    {
        var bm = go.GetComponent<Spells.BuffManager>();
        if (bm.HasBuff(buffId))
            bm.DeleteBuff(buffId);
    }

    public class ShieldHolder
    {
        public delegate void NoMoreShield();
        public event NoMoreShield OnNoMoreShield;

        private float _shieldValueRemaining;
        public float ShieldValueRemaining
        {
            get { return _shieldValueRemaining; }
            set
            {
                _shieldValueRemaining = value;
                if (value <= 0)
                {
                    if (OnNoMoreShield != null)
                        OnNoMoreShield();
                }
            }
        }
    }
}
