using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoldData : MonoBehaviour
{
    CurrenciesManager cm;

    public CurrenciesManager.e_Currencies currency;

    Text txt;

	void Start ()
    {
        cm = GetComponentInParent<StrategistManager>().currenciesManager;
        txt = this.GetComponent<Text>();
	}
	
	void Update ()
    {
        txt.text = cm.currencies[currency].Amount.ToString();
	}
}
