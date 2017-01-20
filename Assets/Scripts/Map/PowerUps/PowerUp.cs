using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{


    public e_Team team;
    public int pwuID;

    public void StartPowerUpType(int type)
    {
        PowerUpInfo pwuInfos = new PowerUpInfo();
        pwuInfos.pwuId = pwuID;
        pwuInfos.pwuType = type;
        var opts = RaiseEventOptions.Default;
        //opts.CachingOption = ExitGames.Client.Photon.EventCaching.AddToRoomCache;
        opts.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(EventCode.POWER_UP, (object)pwuInfos, true, opts);
        this.transform.GetChild(type).gameObject.SetActive(true);
    }

    public void setPwuActive(int i)
    {
        this.transform.GetChild(i).gameObject.SetActive(true);
    }

    public void setPwuInactive()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Debug.Log((this.transform.GetChild(i).gameObject.tag));
            if (this.transform.GetChild(i).gameObject.tag != "Socle")
                this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
