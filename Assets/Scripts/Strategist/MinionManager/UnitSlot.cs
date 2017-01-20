using UnityEngine;
using System.Collections;

namespace Core_MinionManager
{
    public class UnitSlot : MonoBehaviour
    {
        public delegate void Void_D_UnitSlot(UnitSlot slot);
        public delegate void Void_D_UnitType(UnitsInfo.e_UnitType ut);

        public event Void_D_UnitSlot OnSlotStateChanged;
        public event Void_D_UnitType OnUnitTypeChanged;

        public enum e_SlotState
        {
            LOCKED,     // unit can be affected to this slot
            UNLOCKED,   // this slot must be unlocked throught the skill tree before becoming usable
            SEALED      // slot that cannot be modified (units that must spawn each wave). for thoses slot, set the name of the unit in the inspector, or throught a script if it is on runtime.
        }

        [SerializeField]
        private UnitsInfo.e_UnitType _unit = UnitsInfo.e_UnitType.NONE;

        //private UnitsInfo.e_UnitType _unitPreview = UnitsInfo.e_UnitType.NONE;
        //public UnitsInfo.e_UnitType UnitPreview
        //{
        //    get { return _unitPreview; }
        //    set
        //    {
        //        _unitPreview = value;

        //    }
        //}

        public UnitsInfo.e_UnitType Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                if (OnUnitTypeChanged != null)
                    OnUnitTypeChanged(value);
            }
        }

        [SerializeField]
        private e_SlotState _slotState;
        public e_SlotState SlotState
        {
            get { return _slotState; }
            set
            {
                _slotState = value;
                if (OnSlotStateChanged != null)
                    OnSlotStateChanged(this);
            }
        }
    }
}