using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core_MinionManager
{
    /// <summary>
    /// Has a ref on each slot, and on the CardManager;
    /// Can reset the MinionManager to its default configuration : 3 melee and 2 range minions on the sealed slots.
    /// Allow to sava the modification done to the minionManager.
    /// Auto save when closing the panel.
    /// </summary>
    public class MinionPanelManager : MonoBehaviour
    {
        public CardManager cardManager;
        public UI_MinionManager.UI_SlotReferences[] allSlots;

        // /!\ here, you HAVE TO fill the data correctly, because they do not belong to the prefab.
        public SaveLineData[] saveLineData;

        void Awake()
        {
            HeadQuarter hq = GameObject.FindObjectsOfType<HeadQuarter>().Single(x => x.team == GetComponentInParent<StrategistManager>().Team);
            saveLineData[0].associatedSpawnPoint = hq.transform.GetChild(0).GetComponent<SpawningPoint>();
            saveLineData[1].associatedSpawnPoint = hq.transform.GetChild(1).GetComponent<SpawningPoint>();
        }

        public void ResetLanes()
        {
            foreach (UnitSlot[] slots in allSlots.Select(x => x.allLaneSlots))
            {
                foreach (UnitSlot slot in slots)
                {
                    if (slot.SlotState == UnitSlot.e_SlotState.LOCKED)
                        continue;

                    if (slot.Unit != UnitsInfo.e_UnitType.NONE)
                    {
                        cardManager.AddCard(slot.Unit);
                        slot.Unit = UnitsInfo.e_UnitType.NONE;
                    }
                }

                int i = 0;
                foreach (UnitSlot sealedSlot in slots.Where(x => x.SlotState == UnitSlot.e_SlotState.SEALED))
                {
                    if (i < 3)
                    {
                        cardManager.UseCard(UnitsInfo.e_UnitType.MELEE);
                        sealedSlot.Unit = UnitsInfo.e_UnitType.MELEE;
                    }
                    else
                    {
                        cardManager.UseCard(UnitsInfo.e_UnitType.RANGE);
                        sealedSlot.Unit = UnitsInfo.e_UnitType.RANGE;
                    }
                    i++;
                }
            }

            foreach (var v in saveLineData)
            {
                SaveLaneConfiguration(v.managedLane, v.associatedSpawnPoint);
            }
        }

        public void SaveLaneConfiguration(UI_MinionManager.UI_SlotReferences allLaneSlots, SpawningPoint spawnPoint)
        {
            List<UnitsInfo.e_UnitType> units = new List<UnitsInfo.e_UnitType>();

            foreach (UnitSlot slot in allLaneSlots.allLaneSlots)
            {
                if (slot.Unit == UnitsInfo.e_UnitType.NONE || slot.SlotState == UnitSlot.e_SlotState.LOCKED)
                    continue;

                units.Add(slot.Unit);
            }

            RaiseEventStrategist.ModifyLaneCompositionData mlcd = new RaiseEventStrategist.ModifyLaneCompositionData();
            mlcd.newCompo = units.ToArray();
            mlcd.lineID = spawnPoint.SPnumber;
            mlcd.pid = GetComponentInParent<StrategistManager>().GetStrategistPhotonView.viewID;

            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;
            PhotonNetwork.RaiseEvent(EventCode.STRAT_MODIFY_LANE_COMPOSITION, mlcd, true, reo);
        }

        public bool CanSaveLane(UI_MinionManager.UI_SlotReferences lane)
        {
            return !lane.allLaneSlots.Where(x => x.SlotState == UnitSlot.e_SlotState.SEALED).Any(y => y.Unit == UnitsInfo.e_UnitType.NONE);
        }

        public void OnSetInactive()
        {
            foreach (var v in saveLineData)
            {
                if (!CanSaveLane(v.managedLane))
                {
                    foreach (var x in v.managedLane.allLaneSlots)
                    {
                        Debug.Log("je suis : " + x.gameObject.name + ", not preview : " + x.Unit);
                    }

                    Debug.Log("Cannot Save a conf where the (Sealed)Mandatory Slot are not filled. Slots are reset to their default configuration");
                    ResetLanes();
                    break;
                }
                SaveLaneConfiguration(v.managedLane, v.associatedSpawnPoint);
            }
        }

        public void AddSlot()
        {
            foreach (var lane in allSlots)
            {
                var slotToModify = lane.allLaneSlots.FirstOrDefault(x => x.SlotState == UnitSlot.e_SlotState.LOCKED);
                if (slotToModify != null)
                {
                    slotToModify.SlotState = UnitSlot.e_SlotState.UNLOCKED;
                }
            }
        }

        public void RemoveSlot()
        {
            foreach (var lane in allSlots)
            {
                var slotToModify = lane.allLaneSlots.LastOrDefault(x => x.SlotState == UnitSlot.e_SlotState.UNLOCKED);
                if (slotToModify != null)
                {
                    slotToModify.SlotState = UnitSlot.e_SlotState.LOCKED;
                }
            }
        }

        [System.Serializable]
        public struct SaveLineData
        {
            public UI_MinionManager.UI_SlotReferences managedLane;
            public SpawningPoint associatedSpawnPoint;
        }
    }
}