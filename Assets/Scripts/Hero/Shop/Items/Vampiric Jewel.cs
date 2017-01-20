using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VampiricJewel : IItem
{
    int _cost;
    int _id;
    int _resellCost;
    string _name;
    string _description;
    Dictionary<Entity.e_StatType, float> _statBoosts;

    public VampiricJewel()
    {
        _cost = 150;
        _resellCost = _cost * 50 / 100;
        _name = "Vampiric Jewel";
        _description = "A life-stealing Jewel. +15 Life Drain.";
        _statBoosts = new Dictionary<Entity.e_StatType, float>();
        _statBoosts[Entity.e_StatType.LIFE_DRAIN] = 15f;
    }

    public Dictionary<Entity.e_StatType, float> StatBoosts
    {
        get { return (_statBoosts); }
        set {; }
    }


    public int Cost
    {
        get { return (_cost); }
        set {; }
    }

    public int ResellCost
    {
        get { return (_resellCost); }
        set {; }
    }

    public string Name
    {
        get { return (_name); }
        set {; }
    }

    public string Description
    {
        get { return (_description); }
        set {; }
    }

    public int Id
    {
        get { return (_id); }
        set { _id = value; }
    }
}
