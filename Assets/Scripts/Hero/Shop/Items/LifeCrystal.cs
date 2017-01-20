using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeCrystal : IItem
{
    int _cost;
    int _id;
    int _resellCost;
    string _name;
    string _description;
    Dictionary<Entity.e_StatType, float> _statBoosts;

    public LifeCrystal()
    {
        _cost = 150;
        _resellCost = _cost * 50 / 100;
        _name = "Life Crystal";
        _description = "A crystal infused with Life energy. +150 Life. +1.5 Regen HP / s.";
        _statBoosts = new Dictionary<Entity.e_StatType, float>();
        _statBoosts[Entity.e_StatType.HP_MAX] = 150f;
        _statBoosts[Entity.e_StatType.REGEN_HP] = 1.5f;
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
