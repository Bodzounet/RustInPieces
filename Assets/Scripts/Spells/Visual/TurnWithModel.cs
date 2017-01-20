using UnityEngine;
using System.Collections;

public class TurnWithModel : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private string _name;

    void Start()
    {
        Transform tr = null;
        {
            foreach (Transform child in transform.parent.parent)
            {
                if (child.name == _name)
                {
                    tr = child.transform;
                }
            }
        }
        if (tr != null)
        {
            float angle;
            Vector3 axis;
            tr.rotation.ToAngleAxis(out angle, out axis);
            this.gameObject.transform.Rotate(axis * angle );
            print(axis * angle);
            transform.parent = null;
        }
    }
}
