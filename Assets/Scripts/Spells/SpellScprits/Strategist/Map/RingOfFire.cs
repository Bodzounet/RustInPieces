using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class RingOfFire : Spells.ST_Aura
{
    public int damagesWhenCrossing = 250;

    protected override void Awake()
    {
        base.Awake();
        _baseSpell.OnEndCallback.RemoveListener(OnEnd);
        _baseSpell.OnEndCallback.AddListener(() => Destroy(transform.root.gameObject));
    }

    protected override void Start()
    {
        base.Start();
        foreach (var v in Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, 1 << LayerMask.GetMask("Entity", "HeroEntity", "MinionEntity")))
        {
            v.gameObject.AddComponent<RingOfFireImmunity>();
        }
    }

    protected override void GOIsInInfluenceRadius(GameObject go)
    {
        Entity e = go.GetComponent<Entity>();
        if (go.GetComponent<RingOfFireImmunity>() != null)
            return;

        if (e is HeroEntity)
        {
            e.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, damagesWhenCrossing, null);
        }
        else
        {
            e.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, damagesWhenCrossing / 2, null);
        }
    }

    protected override void GOIsNoMoreInInfluenceRadius(GameObject go)
    {
        if (go.GetComponent<RingOfFireImmunity>() != null)
            return;

        Entity e = go.GetComponent<Entity>();
        if (e is HeroEntity)
        {
            e.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, damagesWhenCrossing, null);
        }
        else
        {
            e.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, damagesWhenCrossing / 2, null);
        }
    }
}
    