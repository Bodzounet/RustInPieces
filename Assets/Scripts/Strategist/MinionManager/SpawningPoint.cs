using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core_MinionManager
{
    public class SpawningPoint : MonoBehaviour
    {
        private Dictionary<UnitsInfo.e_UnitType, int> _squad = new Dictionary<UnitsInfo.e_UnitType, int>();
        public Dictionary<UnitsInfo.e_UnitType, int> Squad
        {
            get { return _squad; }
        }

        private int _spnumber;
        public int SPnumber
        {
            get { return _spnumber; }
        }

        //private e_Team team;
		public int lane;

        void Awake()
        {
            _spnumber = this.transform.GetSiblingIndex();

			//team = this.GetComponentInParent<HeadQuarter>().team;
            foreach (var v in ((UnitsInfo.e_UnitType[])System.Enum.GetValues(typeof(UnitsInfo.e_UnitType))).Where(x => x != UnitsInfo.e_UnitType.NONE))
            {
                Squad[v] = 0;
            }
        }

  //      public void Spawn()
  //      {
		//	try
		//	{
		//		StrategistManager strategist = GameObject.FindObjectsOfType<StrategistManager>().Where(x => x.Team == team).Single();
	 //           foreach (var v in Squad)
		//		{
		//			for (int i = 0; i < v.Value; i++)
		//			{
	 //                   GameObject mob = strategist.minionManager.CreateMinion(v.Key, transform.position, Quaternion.identity, lane);
		//			}
	 //           }
		//	}
		//	catch
		//	{
		//		Debug.LogWarning("Aucun Stratege " + team + " assigné pas cool ");
		//	}
		//}

        public void AddUnitToThisSpawnPoint(UnitsInfo.e_UnitType unitType)
        {
            Squad[unitType] += 1;
        }

        public void RemoveUnitToThisSpawnPoint(UnitsInfo.e_UnitType unitType)
        {
            Squad[unitType] -= 1;
#if UNITY_EDITOR
            if (Squad[unitType] < 0)
                Debug.Log("Cannot be a negative value here, error made elsewhere...");
#endif
        }

        public void ClearSpawnPoint()
        {
            _squad = Squad.ToDictionary(x => x.Key, x => 0);
        }
    }
}