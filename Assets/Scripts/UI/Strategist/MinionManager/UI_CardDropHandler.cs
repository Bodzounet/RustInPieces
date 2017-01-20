using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;

using Core_MinionManager;

namespace UI_MinionManager
{
    public class UI_CardDropHandler : MonoBehaviour, IDropHandler
    {
        private UnitSlot _unitSlot;
        private CardManager _cm;

        void Awake()
        {
            _unitSlot = this.GetComponent<UnitSlot>();
            _cm = GetComponentInParent<StrategistManager>().minionPanelManager.cardManager;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_unitSlot.SlotState == UnitSlot.e_SlotState.LOCKED || !eventData.pointerDrag.GetComponent<UI_CardDragHandler>().IsDragging)
                return;

            UI_Card ui_card = eventData.pointerDrag.GetComponent<UI_Card>();

            UnitsInfo.e_UnitType overridenUnitType = _unitSlot.Unit;
            _unitSlot.Unit = ui_card.managedUnit;



            _cm.UseCard(ui_card.managedUnit);

            var overridenAssociatedCard = ui_card.CardManager.cards.SingleOrDefault(x => x.managedUnit == overridenUnitType);
            if (overridenAssociatedCard != null)
                _cm.RefundCard(overridenAssociatedCard.managedUnit);
        }
    }
}