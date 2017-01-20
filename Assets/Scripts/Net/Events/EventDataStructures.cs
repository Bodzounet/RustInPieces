using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SyncHeroValues
{
    public int photonId;
    public uint lastId;
    public int gold;
    public int statsSize;
    public List<float> entityStats;
    public int spellsSize;
    public List<float> spellsCooldown;
}

[System.Serializable]
public struct SyncStrategistValues
{
    public int photonId;
    public uint lastId;
    public int gold;
    public int gears;
    public int spellsSize;
    public List<float> spellsCooldown;
}

public struct HeroSpellLaunchedInfo
{
    public int casterPhotonId;
    public int index;
    public string spellId;
    public Spells.SpellInfo.e_CastType castType;
    public Vector3 casterPosition;
    public Vector3 casterRotation;
    public Vector3 spellPosition;
    public int target;
    public float chargeStart;
}

public struct HeroInfo
{
    public int HeroId;
}

public struct PowerUpInfo
{
    public int pwuId;
    public int pwuType;
}

public struct DoorInfo
{
    public int doorId;
}