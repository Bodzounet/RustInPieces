using UnityEngine;
using System.Collections.Generic;

public class CurrenciesManager : MonoBehaviour
{
    public enum e_Currencies
    {
        Gold,
        Gears
    }

    [HideInInspector]
    public Dictionary<e_Currencies, CurrencyHelper> currencies;

    public int initialGold;

    void Awake()
    {
        currencies = new Dictionary<e_Currencies, CurrencyHelper>();
        currencies[e_Currencies.Gold] = new CurrencyHelper();
        currencies[e_Currencies.Gears] = new CurrencyHelper();
    }

    void Start()
    {
        currencies[e_Currencies.Gold].AddCurrency(initialGold);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            currencies[e_Currencies.Gold].AddCurrency(1000);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            currencies[e_Currencies.Gears].AddCurrency(1);
    }

    public class CurrencyHelper
    {
        public delegate void CurrencyAmountModification(int before, int after);
        public event CurrencyAmountModification OnCurrencyAmountModification;

        private int _amount;
        public int Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                int before = _amount;
                _amount = value;
                if (OnCurrencyAmountModification != null)
                    OnCurrencyAmountModification(before, value);
            }
        }

        public bool HasEnoughCurrency(int amount)
        {
            return Amount >= amount;
        }

        public bool UseCurrency(int amount)
        {
            if (!HasEnoughCurrency(amount))
                return false;

            Amount -= amount;
            return true;
        }

        public void AddCurrency(int amount)
        {
            Amount += amount;
        }


    }
}
