using UnityEngine;
using System.Collections;

public class CaptureEvents : MonoBehaviour {

    SecondaryObjective so;
	void Start () {
        so  = GameObject.Find("SecondaryObjective").gameObject.GetComponent<SecondaryObjective>();
    }
	

    [PunRPC]
    public void heroExitPoint(int team)
    {
        if (team == 1)
        {
            so.Speed1--;
        }
        else if (team == 2)
        {
            so.Speed2--;
        }
    }


    [PunRPC]
    public void heroEnterPoint(int team)
    {
        if (team == 1)
        {
            so.Speed1++;
        }
        else if (team == 2)
        {
            so.Speed2++;
        }
    }

}
