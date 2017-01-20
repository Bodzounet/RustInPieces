using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI_MinionManager
{
    public class UI_Card : MonoBehaviour
    {
        public UnitsInfo.e_UnitType managedUnit;

        private Text text;
        private int _stock = 0;
        public int Stock
        {
            get
            {
                return _stock;
            }

            set
            {
                _stock = value;
                if (text == null)
                    text = GetComponentInChildren<Text>();
                text.text = _stock.ToString();
            }
        }

        public Core_MinionManager.CardManager CardManager
        {
            get
            {
                return _cardManager;
            }
        }

        Core_MinionManager.CardManager _cardManager;

        void Awake()
        {
            text = GetComponentInChildren<Text>();
            _cardManager = this.GetComponentInParent<Core_MinionManager.CardManager>();
        }

        void OnEnable()
        {
            text.text = _stock.ToString();
        }
    }
}