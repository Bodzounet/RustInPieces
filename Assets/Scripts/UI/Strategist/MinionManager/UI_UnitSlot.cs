using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UI_MinionManager
{
    [RequireComponent(typeof(Core_MinionManager.UnitSlot))]
    [RequireComponent(typeof(UnitSlotMouse))]
    public class UI_UnitSlot : MonoBehaviour
    {
        UI_SlotHelper _ui_slotHelper;
        Core_MinionManager.UnitSlot _unitSlot; // the managed slot

        Image _image;

        public Core_MinionManager.CardManager cardManager;

        void Awake()
        {
            _ui_slotHelper = GameObject.FindObjectOfType<UI_SlotHelper>();

            _unitSlot = GetComponent<Core_MinionManager.UnitSlot>();
            _image = GetComponent<Image>();

            _unitSlot.OnUnitTypeChanged += ChangeUnitType;
            _unitSlot.OnSlotStateChanged += _unitSlot_OnSlotStateChanged;
        }

        private void _unitSlot_OnSlotStateChanged(Core_MinionManager.UnitSlot slot)
        {
            if (slot.SlotState == Core_MinionManager.UnitSlot.e_SlotState.UNLOCKED)
            {
                _image.sprite = _ui_slotHelper.EmptySlot;
            }
            else if (slot.SlotState == Core_MinionManager.UnitSlot.e_SlotState.LOCKED)
            {
                _image.sprite = _ui_slotHelper.LockedSlot;
            }
        }

        public void ClearSlot()
        {
            if (_unitSlot.SlotState == Core_MinionManager.UnitSlot.e_SlotState.LOCKED)
                return;

            UI_Card card = cardManager.cards.SingleOrDefault(x => x.managedUnit == _unitSlot.Unit);
            if (card != null)
                card.Stock++;

            _unitSlot.Unit = UnitsInfo.e_UnitType.NONE;
            _image.sprite = _ui_slotHelper.EmptySlot;
        }

        public void ChangeUnitType(UnitsInfo.e_UnitType ut)
        {
            if (_unitSlot.SlotState == Core_MinionManager.UnitSlot.e_SlotState.LOCKED)
            {
                _image.sprite = _ui_slotHelper.LockedSlot;
            }
            else if (ut == UnitsInfo.e_UnitType.NONE)
            {
                _image.sprite = _ui_slotHelper.EmptySlot;
            }
            else
            {
                _image.sprite = _ui_slotHelper.unitToSprite[ut];
            }
        }

        void OnEnable()
        {
            ChangeUnitType(_unitSlot.Unit);
        }
    }
}