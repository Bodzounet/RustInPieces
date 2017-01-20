using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core_MinionManager
{
    public class CardManager : MonoBehaviour
    {
        public UI_MinionManager.UI_Card[] cards;
        public UnitSlot[] slots;

        private Dictionary<UnitsInfo.e_UnitType, int> _unitsCards = new Dictionary<UnitsInfo.e_UnitType, int>();

        void Awake()
        {
            foreach (var v in slots)
            {
                v.OnSlotStateChanged += SlotStateHasChanged;
            }

            foreach (var v in ((UnitsInfo.e_UnitType[])System.Enum.GetValues(typeof(UnitsInfo.e_UnitType))).Where(x => x != UnitsInfo.e_UnitType.NONE))
            {
                _unitsCards[v] = 0;
            }
        }

        void Start()
        {
            for (int i = 0; i < 6; i++)
                AddCard(UnitsInfo.e_UnitType.MELEE);
            for (int i = 0; i < 4; i++)
                AddCard(UnitsInfo.e_UnitType.RANGE);
            GetComponentInParent<StrategistManager>().minionPanelManager.ResetLanes();
        }

        /// <summary>
        /// When UNLOCKING a card
        /// </summary>
        /// <param name="unitType"></param>
        public void AddCard(UnitsInfo.e_UnitType unitType)
        {
            _unitsCards[unitType] += 1;
            var card = cards.Single(x => x.managedUnit == unitType);
            card.Stock++;
        }

        /// <summary>
        /// When LOCKING again a card
        /// </summary>
        /// <param name="unitType"></param>
        public void RemoveCard(UnitsInfo.e_UnitType unitType)
        {
            _unitsCards[unitType] -= 1;
            var card = cards.Single(x => x.managedUnit == unitType);
            card.Stock--;
#if UNITY_EDITOR
            if (_unitsCards[unitType] < 0)
                Debug.Log("There cannot be a negative number here.");
#endif
        }

        /// <summary>
        /// return the card both available and unavailable quantity of of the type "unitType"
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public int GetCardQuantity(UnitsInfo.e_UnitType unitType)
        {
            if (_unitsCards.ContainsKey(unitType))
            {
                return _unitsCards[unitType];
            }
            return 0;
        }

        /// <summary>
        /// return the card available quantity of type "unitType"
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public int GetCardAvailableQuantity(UnitsInfo.e_UnitType unitType)
        {
            return cards.Single(x => x.managedUnit == unitType).Stock;
        }

        /// <summary>
        /// When using a card
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public bool UseCard(UnitsInfo.e_UnitType unitType)
        {
            if (GetCardAvailableQuantity(unitType) == 0)
                return false;
            var card = cards.Single(x => x.managedUnit == unitType);
            card.Stock--;
            return true;
        }

        /// <summary>
        /// When unusing a card
        /// </summary>
        /// <param name="unitType"></param>
        public void RefundCard(UnitsInfo.e_UnitType unitType)
        {
            var card = cards.Single(x => x.managedUnit == unitType);
            card.Stock++;
        }

        public void ResetCards()
        {
            _unitsCards = _unitsCards.ToDictionary(x => x.Key, x => 0);
        }

        void SlotStateHasChanged(UnitSlot slot)
        {
            if (slot.SlotState == UnitSlot.e_SlotState.LOCKED)
            {
                if (slot.Unit != UnitsInfo.e_UnitType.NONE)
                {
                    AddCard(slot.Unit);
                    slot.Unit = UnitsInfo.e_UnitType.NONE;
                }
            }
        }
    }
}