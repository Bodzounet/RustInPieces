using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cleaver : IItem
{
    int _cost;
    int _resellCost;
    int _id;
    string _name;
    string _description;
    Dictionary<Entity.e_StatType, float> _statBoosts;

    public Cleaver()
    {
        _cost = 250;
        _resellCost = _cost * 50 / 100;
        _name = "Cleaver";
        _description = "A strong cleaver. +18 PA. +10 Defense Pen.";
        _statBoosts = new Dictionary<Entity.e_StatType, float>();
        _statBoosts[Entity.e_StatType.MELEE_ATT] = 18f;
        _statBoosts[Entity.e_StatType.DEFENSE_PEN] = 10f;
        
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
