using UnityEngine;
using System.Collections;

public class PowerUpsManager : MonoBehaviour {

    // Use this for initialization
    public GameObject[] powerups;

    void Start()
    {
        EventManager.Instance.addEventCallback(EventCode.POWER_UP, PhotonPowerUpEvent);
    }

    public void PhotonPowerUpEvent(object powerUpInfo, int senderId)
    {
        PowerUpInfo info = (PowerUpInfo)powerUpInfo;
        powerups[info.pwuId].GetComponent<PowerUp>().setPwuInactive();
        powerups[info.pwuId].GetComponent<PowerUp>().setPwuActive(info.pwuType);
    }
}
