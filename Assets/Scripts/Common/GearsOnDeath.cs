using UnityEngine;
using System.Collections;

public class GearsOnDeath : MonoBehaviour {

    [SerializeField]
    private int gearsOnDeath = 1;

    private Entity _ent;
    void Start ()
    {
        _ent = GetComponent<Entity>();
        _ent.OnDeath += OnDeath;
	}

	void OnDeath(GameObject dead)
    {
        if (dead.GetComponent<Entity>().Hitter != null)
        {
            GameObject strat = PlayersInfos.Instance.GetStrategistOfTeam(dead.GetComponent<Entity>().Hitter.Team);
            strat.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gears].AddCurrency(5);
        }
    }
}
