using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Node_RebirthSwiftness : SkillTree.TreeNode
{
    public GameObject speedBufflvl1;
    public GameObject speedBufflvl2;

    protected override void From0To1()
    {
        foreach (var v in PlayersInfos.Instance.heroList.Select(x => x.GetComponent<Entity>()).Where(x => x.Team == GetComponentInParent<StrategistManager>().Team))
        {
            v.OnRespawn += CB_OnRespawnLvl1;
        }
    }

    protected override void From1To0()
    {
        foreach (var v in PlayersInfos.Instance.heroList.Select(x => x.GetComponent<Entity>()).Where(x => x.Team == GetComponentInParent<StrategistManager>().Team))
        {
            v.OnRespawn -= CB_OnRespawnLvl1;
        }
    }

    protected override void From1To2()
    {
        foreach (var v in PlayersInfos.Instance.heroList.Select(x => x.GetComponent<Entity>()).Where(x => x.Team == GetComponentInParent<StrategistManager>().Team))
        {
            v.OnRespawn -= CB_OnRespawnLvl1;
            v.OnRespawn += CB_OnRespawnLvl2;
        }
    }

    protected override void From2To1()
    {
        foreach (var v in PlayersInfos.Instance.heroList.Select(x => x.GetComponent<Entity>()).Where(x => x.Team == GetComponentInParent<StrategistManager>().Team))
        {
            v.OnRespawn -= CB_OnRespawnLvl2;
            v.OnRespawn += CB_OnRespawnLvl1;
        }
    }

    protected override void From2To3()
    {
        throw new NotImplementedException();
    }

    protected override void From3To2()
    {
        throw new NotImplementedException();
    }

    public void CB_OnRespawnLvl1(GameObject go)
    {
        go.GetComponent<Spells.BuffManager>().AddBuff(speedBufflvl1, transform.root.gameObject, go);
    }

    public void CB_OnRespawnLvl2(GameObject go)
    {
        go.GetComponent<Spells.BuffManager>().AddBuff(speedBufflvl2, transform.root.gameObject, go);
    }
}
