using UnityEngine;
using System.Collections;

public class AutoDestroyDivineShield : MonoBehaviour {

	void Start ()
    {
        Destroy(this.gameObject, 5);
	}
}
