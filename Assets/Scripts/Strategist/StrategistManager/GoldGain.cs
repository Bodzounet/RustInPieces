using UnityEngine;
using System.Collections;

public class GoldGain : MonoBehaviour {

    // Use this for initialization
    public int amount = 1;
    public bool stopPassiveGold = false;

	void Start () {
        StartCoroutine(GainGoldPassive());
    }
	
    IEnumerator GainGoldPassive ()
    {
        while (!stopPassiveGold)
        {
            this.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gold].AddCurrency(amount);
            yield return new WaitForSeconds(1f);
        }
    }
}
