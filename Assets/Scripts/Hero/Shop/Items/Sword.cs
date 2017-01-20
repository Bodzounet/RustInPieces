﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : IItem
{
    int _cost;
    int _id;
    int _resellCost;
    string _name;
    string _description;
    Dictionary<Entity.e_StatType, float> _statBoosts;

    public Sword()
    {
        _cost = 100;
        _resellCost = _cost * 50 / 100;
        _name = "Sword";
        _description = "A simple sword, useful for beginners. +10 PA.";
        _statBoosts = new Dictionary<Entity.e_StatType, float>();
        _statBoosts[Entity.e_StatType.MELEE_ATT] = 10;
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
        set {_id = value; }
    }
}
