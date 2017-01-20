using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core_MinionManager
{
    public class HeadQuarter : MonoBehaviour
    {
        public delegate void Void_D_Void();

        public event Void_D_Void OnSpawningTimeChanged;

        public SpawningPoint[] spawningPoints; // set them through the inspector

        public e_Team team;

        [SerializeField]
        private float _firstSpawnDelay;

        [SerializeField]
        private float _baseSpawningTime;
        public float BaseSpawningTime
        {
            get { return _baseSpawningTime; }
        }

        private float _spawningTime; // time between waves.
        public float SpawningTime
        {
            get { return _spawningTime; }
            set
            {
                _spawningTime = value;

                if (OnSpawningTimeChanged != null)
                    OnSpawningTimeChanged();
            }
        }

        void Start()
        {
            SpawningTime = BaseSpawningTime;
            Invoke("SpawnUnits", _firstSpawnDelay);
        }

        public void SpawnUnits()
        {
            if (FindObjectsOfType<StrategistManager>().Single(x => x.Team == team) == null || !FindObjectsOfType<StrategistManager>().Single(x => x.Team == team).GetStrategistPhotonView.isMine)
                return;

            RaiseEventOptions reo = new RaiseEventOptions();
            reo.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

            RaiseEventStrategist.SpawnMinionData smd = new RaiseEventStrategist.SpawnMinionData();
            smd.pid = FindObjectsOfType<StrategistManager>().Single(x => x.Team == team).GetStrategistPhotonView.viewID;

            PhotonNetwork.RaiseEvent(EventCode.STRAT_SPAWN_MINION, smd, true, reo);

		    Invoke("SpawnUnits", _spawningTime);
		}
    }
}