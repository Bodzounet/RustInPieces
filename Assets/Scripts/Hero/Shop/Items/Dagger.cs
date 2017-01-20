using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dagger : IItem
{
    int _cost;
    int _id;
    int _resellCost;
    string _name;
    string _description;
    Dictionary<Entity.e_StatType, float> _statBoosts;

    public Dagger()
    {
        _cost = 180;
        _resellCost = _cost * 50 / 100;
        _name = "Dagger";
        _description = "A vicious dagger. +0.2 AS. +10 Focus.";
        _statBoosts = new Dictionary<Entity.e_StatType, float>();
        _statBoosts[Entity.e_StatType.ATTACK_SPEED] = 0.2f;
        _statBoosts[Entity.e_StatType.FOCUS] = 10f;
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
