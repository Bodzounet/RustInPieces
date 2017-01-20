using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersInfos: Singleton<PlayersInfos> {
    public List<GameObject> heroList;
    public List<GameObject> strategistList;

    void Awake()
    {
        heroList = new List<GameObject>();
        strategistList = new List<GameObject>();
    }

    public GameObject getGameObjectWithPhotonId(int photonId)
    {
        foreach (GameObject obj in heroList)
        {
            if (obj == null)
                continue;
            if (obj.GetComponent<PhotonView>().viewID == photonId)
                return obj;
        }

       // Debug.Log("Items count : " + strategistList.Count);


        foreach (GameObject obj in strategistList)
        {
         //   Debug.Log("Item : " + obj.name);
          //  Debug.Log("viewID : " + obj.GetComponent<PhotonView>().viewID + ", photonID : " + photonId);
            if (obj == null)
                continue;
            if (obj.GetComponent<PhotonView>().viewID == photonId)
                return obj;
        }
        return null;
    }

    public List<GameObject> GetHeroesOfTeam(e_Team team)
    {
        List<GameObject> ret = new List<GameObject>();
        foreach (GameObject obj in heroList)
        {
            if (obj == null)
                continue;
            if (obj.GetComponent<HeroEntity>().Team == team)
                ret.Add(obj);
        }
        return ret;
    }

    public GameObject GetStrategistOfTeam(e_Team team)
    {
        List<GameObject> ret = new List<GameObject>();
        foreach (GameObject obj in strategistList)
        {
            if (obj == null)
                continue;
            if (obj.GetComponent<StrategistManager>().Team == team)
                return obj;
        }
        return null;
    }
}
