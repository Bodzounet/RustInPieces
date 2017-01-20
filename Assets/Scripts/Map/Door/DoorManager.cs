using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

    // Use this for initialization
    public GameObject[] doors;

    void Start()
    {
        EventManager.Instance.addEventCallback(EventCode.Door, PhotonDoorEvent);
    }

    public void PhotonDoorEvent(object doorInfo, int senderId)
    {
        DoorInfo info = (DoorInfo)doorInfo;
        doors[info.doorId].SetActive(false);
    }
}
