using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UI_MinionManager
{
    public class UI_SlotHelper : MonoBehaviour
    {
        [SerializeField]
        private KV_UnitSprite[] _dicoUnitSprite; // since unity doesn't allow to fill a dico from the editor, we use our "false" dico.

        /// <summary>
        /// a map to display the correct image in the slot based on the id of the current slot.
        /// in the future, the value may be more complex, and turn into a structure with more data.
        /// </summary>
        public Dictionary<UnitsInfo.e_UnitType, Sprite> unitToSprite = new Dictionary<UnitsInfo.e_UnitType, Sprite>();

        /// <summary>
        /// this will represent the rotation order of unit when right clicking on the slot.
        /// it is filled base on the order of _dicoStringSprite.
        /// ex : _unitRotation[0] -> _unitRotation[1] -> ... _unitROtation[n] -> unitRotation[0] -> ...
        /// </summary>
        [HideInInspector]
        public List<UnitsInfo.e_UnitType> _unitRotation = new List<UnitsInfo.e_UnitType>();

        public Sprite EmptySlot;
        public Sprite LockedSlot;

        void Awake()
        {
            foreach (var kv in _dicoUnitSprite)
            {
                // some checks to avoid thoughtlessness...
#if UNITY_EDITOR
                if (unitToSprite.ContainsKey(kv.key))
                {
                    Debug.LogError("The dictionary has not been filled correctly. : the key already exists");
                }
                if (unitToSprite.ContainsValue(kv.Value))
                {
                    Debug.LogError("The dictionary has not been filled correctly. : the value already exists");
                }
#endif
                unitToSprite[kv.key] = kv.Value;
                _unitRotation.Add(kv.key);
            }
        }
    }

    [System.Serializable]
    public class KV_UnitSprite
    {
        public UnitsInfo.e_UnitType key;
        public Sprite Value;
    }
}