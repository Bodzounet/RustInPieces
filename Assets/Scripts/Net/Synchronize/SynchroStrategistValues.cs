using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SynchroStrategistValues : MonoBehaviour {

    //private Entity _entity;
    private CurrenciesManager _currManager;
    private Spells.SpellLauncher _launcher;
    private PhotonView _view;

    void Awake()
    {
        //_entity = GetComponent<Entity>();
        _currManager = GetComponent<CurrenciesManager>();
        _launcher = GetComponent<Spells.SpellLauncher>();
        _view = GetComponent<PhotonView>();
    }

	void Start() 
    {
        InvokeRepeating("SendStrategistInfos", 0, ManageSynchronize.SYNCHRO_COOLDOWN);
	}

    void SendStragistInfos()
    {
        SyncStrategistValues values = new SyncStrategistValues();
        values.photonId = _view.viewID;
        values.lastId = ManageSynchronize.Instance.GetLastId();
        values.gears = _currManager.currencies[CurrenciesManager.e_Currencies.Gears].Amount;
        values.gold = _currManager.currencies[CurrenciesManager.e_Currencies.Gold].Amount;
        values.spellsCooldown = new List<float>();
        for (int i = 0; i < _launcher.AvailableSpells.Count; i++)
            values.spellsCooldown.Add(_launcher.AvailableSpells[i].Cooldown);
        PhotonNetwork.RaiseEvent(EventCode.STATS_STRAT, (object)values, true, null);
        ManageSynchronize.Instance.addStrategistValues(values);
    }
}
