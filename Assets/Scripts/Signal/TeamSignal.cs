using UnityEngine;
using System.Collections;

public class TeamSignal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StrategistManager _strat;
        if ((_strat = gameObject.GetComponent<StrategistManager>()) != null)
            _eventCode = (_strat.Team == e_Team.TEAM1 ? EventCode.SIGNAL_PING_TEAM_1 : EventCode.SIGNAL_PING_TEAM_2);
        _pView = gameObject.GetComponent<PhotonView>();
    }

    private byte _eventCode;
    private PhotonView _pView;

    [SerializeField]
    private GameObject _signal;

    [SerializeField]
    private Camera _camera;

    void PhotonGetTeamSignal(object point, int senderId)
    {

        Instantiate(_signal, (Vector3)point, transform.rotation);
    }

    // Update is called once per frame
    void Update () {
	if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            int mask = 1 << LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, mask))
            {
                Instantiate(_signal, hitinfo.point, Quaternion.Euler(new Vector3(25f, 0f, 0f)));
                if (_pView != null)
                    PhotonNetwork.RaiseEvent(_eventCode, hitinfo.point, true, RaiseEventOptions.Default);
            }
        }
	}
}
