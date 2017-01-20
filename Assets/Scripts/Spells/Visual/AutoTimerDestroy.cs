using UnityEngine;
using System.Collections;

public class AutoTimerDestroy : MonoBehaviour {

    [SerializeField]
    float _time;

	// Use this for initialization
	void Start () {
        Object.Destroy(gameObject, _time);
	}
	

    void OnDestroy()
    {
        //transform.parent = null;
    }
}
