using UnityEngine;
using System.Collections;

public class Saver : MonoBehaviour {

    [SerializeField]
    Transform[] tosave;

    void OnDestroy()
    {
        for (int i = 0; i < tosave.Length; ++i)
        {
            if (tosave[i] != null)
            {
                tosave[i].parent = null;
            }
        }
    }
}
