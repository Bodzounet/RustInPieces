using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Node_GreedyHeroes : SkillTree.TreeNode
{
    public float goldGainFactorOnMinions = 1.25f;
    public int additionnalGainPerTic = 2;

    TeamFactory _tf;

    void Start()
    {
        _tf = FindObjectsOfType<TeamFactory>().Single(x => GetComponentInParent<StrategistManager>().Team == x.MyTeam);
    }

    protected override void From0To1()
    {
        _tf.AddCallBack("HERAULT", CB_MinionsGivesMoreGold);
        _tf.AddCallBack("MAGIC", CB_MinionsGivesMoreGold);
        _tf.AddCallBack("MEDIC", CB_MinionsGivesMoreGold);
        _tf.AddCallBack("MELEE", CB_MinionsGivesMoreGold);
        _tf.AddCallBack("RANGE", CB_MinionsGivesMoreGold);
        _tf.AddCallBack("TANK", CB_MinionsGivesMoreGold);
    }

    protected override void From1To0()
    {
        _tf.RemoveCallBack("HERAULT", CB_MinionsGivesMoreGold);
        _tf.RemoveCallBack("MAGIC", CB_MinionsGivesMoreGold);
        _tf.RemoveCallBack("MEDIC", CB_MinionsGivesMoreGold);
        _tf.RemoveCallBack("MELEE", CB_MinionsGivesMoreGold);
        _tf.RemoveCallBack("RANGE", CB_MinionsGivesMoreGold);
        _tf.RemoveCallBack("TANK", CB_MinionsGivesMoreGold);
    }

    protected override void From1To2()
    {
        GetComponentInParent<GoldGain>().amount += additionnalGainPerTic;
    }

    protected override void From2To1()
    {
        GetComponentInParent<GoldGain>().amount -= additionnalGainPerTic;
    }

    protected override void From2To3()
    {
        throw new NotImplementedException();
    }

    protected override void From3To2()
    {
        throw new NotImplementedException();
    }

    GameObject CB_MinionsGivesMoreGold(GameObject go)
    {
        var v = go.GetComponent<UnitEntity>();
        v.GoldWorth = Mathf.FloorToInt(v.GoldWorth * goldGainFactorOnMinions);
        return go;
    }
}
