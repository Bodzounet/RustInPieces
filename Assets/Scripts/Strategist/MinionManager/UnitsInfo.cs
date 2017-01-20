using UnityEngine;
using System.Collections;

/// <summary>
/// Network uses strings to instanciate stuff, but using an enum is far more userfriendly, so we need this class, and some tools to switch from one to another
/// </summary>
public static class UnitsInfo 
{
    /// <summary>
    /// List here the name of all units you have created.
    /// IT ALSO HAS TO BE THE NAME OF THE PREFAB (choose a nice convention plz^^)
    /// Don't remove NONE.
    /// </summary>
    public enum e_UnitType
    {
        MELEE = 100,
        RANGE = 250,
        MAGIC = 300,
        MEDIC = 200,
        TANK = 1000,
        HERAULT = 450,
        HERO = 50,
        NONE = 1
    }

    public static string TypeToString(e_UnitType type)
    {
        return System.Enum.GetName(typeof(e_UnitType), type);
    }

    public static e_UnitType StringToType(string name)
    {
        return (e_UnitType)System.Enum.Parse(typeof(e_UnitType), name, false);
    }
}
