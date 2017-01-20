using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManageSynchronize : Singleton<ManageSynchronize>
{

    public static float SYNCHRO_TIMEOUT = 0.8f;
    public static float SYNCHRO_COOLDOWN = 1.0f;

    private Dictionary<int, List<SyncHeroValues>> _heroValues;
    private Dictionary<int, List<SyncStrategistValues>> _strategistValues;
    private uint _id = 0;

    public void addHeroValues(SyncHeroValues values)
    {
    }

    public void addStrategistValues(SyncStrategistValues values)
    {

    }

    void onEventStratValues(object content, int senderId)
    {

    }

    public uint GetLastId()
    {
        return _id;
    }
}