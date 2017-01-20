using UnityEngine;
using System.Collections;

public class MinionManager_CleanMinion : MonoBehaviour 
{
	public MinionManager manager;

    void Start()
    {
        GetComponent<UnitEntity>().OnDeath += OnDeath;
    }

    void OnDeath(GameObject self)
    {
        manager.RemoveMinion(_idMinion);
    }

    private bool _quit;
	private int _idMinion;
	public int IdMinion
	{
		get
		{
			return _idMinion;
		}
		set
		{
			_idMinion = value;
		}
	}


    void OnApplicationQuit()
    {
        _quit = true;
    }
}
