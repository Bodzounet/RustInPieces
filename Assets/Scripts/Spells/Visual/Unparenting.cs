using UnityEngine;
using System.Collections;

public class Unparenting : MonoBehaviour {

    [SerializeField]
    Transform[] toUnparent;


    // Use this for initialization
    void Start () {
        for (int i = 0; i < toUnparent.Length; ++i)
        {
            if (toUnparent[i] != null)
            {
                toUnparent[i].parent = null;
            }
        }
    }
	
}
