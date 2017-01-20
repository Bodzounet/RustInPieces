using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Spells.BuffManager))]

public class HeroEntity : Entity 
{
    public event Void_D_Int OnGoldChanged;
    public event Void_D_KDA OnKDAChanged;
    public event Void_D_Void UnlockingLastSlot;


    private KDA _kda = new KDA();
    public KDA KDA
    {
        get { return (_kda); }
        set
        {
            _kda = value;
            if (OnKDAChanged != null)
            {
                OnKDAChanged.Invoke(_kda);
            }
        }
    }
    
    public int Kills
    {
        get { return _kda.Kills; }
        set
        {
            _kda.Kills = value;
            if (OnKDAChanged != null)
            {
                OnKDAChanged.Invoke(_kda);
            }
        }
    }

    public int Deaths
    {
        get { return _kda.Deaths; }
        set
        {
            _kda.Deaths = value;
            if (OnKDAChanged != null)
            {
                OnKDAChanged.Invoke(_kda);
            }
        }
    }

    public int Assists
    {
        get { return _kda.Assists; }
        set
        {
            _kda.Assists = value;
            if (OnKDAChanged != null)
            {
                OnKDAChanged.Invoke(_kda);
            }
        }
    }

    [SerializeField]
    private string _iconName;
    public string IconName
    {
        get { return (_iconName); }
        set { _iconName = value; }
    }

    [SerializeField]
    private int _gold;
    public int Gold
    {
        get { return (_gold); }
        set
        {
            _gold = value;
            if (OnGoldChanged != null)
            {
                OnGoldChanged.Invoke(_gold);
            }
        }
    }

    bool _lastSlotUnlocked = false;
    public bool LastSlotUnlocked
    {
        get { return (_lastSlotUnlocked); }
        set
        {
            _lastSlotUnlocked = value;
            if (value == true && UnlockingLastSlot != null)
            {
                UnlockingLastSlot.Invoke();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}