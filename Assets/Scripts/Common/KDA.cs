using UnityEngine;
using System.Collections;

public class KDA
{
    public KDA()
    {
        _kills = 0;
        _deaths = 0;
        _assists = 0;
    }

    int _kills;
    public int Kills
    {
        get { return _kills; }
        set { _kills = value; }
    }

    int _deaths;
    public int Deaths
    {
        get { return _deaths; }
        set { _deaths = value; }
    }

    int _assists;
    public int Assists
    {
        get { return _assists; }
        set { _assists = value; }
    }
}