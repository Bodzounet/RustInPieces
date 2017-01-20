using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IItem
{
    Dictionary<Entity.e_StatType, float> StatBoosts
    {
        get;
        set;
    }

    int Cost
    {
        get;
        set;
    }

    int ResellCost
    {
        get;
        set;
    }

    string Name
    {
        get;
        set;
    }

    string Description
    {
        get;
        set;
    }

    int Id
    {
        get;
        set;
    }
}
