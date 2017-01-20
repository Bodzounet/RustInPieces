using UnityEngine;
using System.Collections;

public class DetectCollisionsAndPrint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " entered in collision with " + other.name);
    }
}
