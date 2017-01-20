using UnityEngine;
using System.Collections;

public class TeamSignalReciver : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Entity _hero;
        if ((_hero = gameObject.GetComponent<Entity>()) != null)
            _eventCode = (_hero.Team == e_Team.TEAM1 ? EventCode.SIGNAL_PING_TEAM_1 : EventCode.SIGNAL_PING_TEAM_2);
        _pView = gameObject.GetComponent<PhotonView>();
        if (_pView != null)
            if (_pView.isMine == true)
                EventManager.Instance.addEventCallback(_eventCode, PhotonGetTeamSignal);
    }

    private byte _eventCode;
    private PhotonView _pView;

    [SerializeField]
    private GameObject _signal;

    [SerializeField]
    private Camera _camera;

    void PhotonGetTeamSignal(object point, int senderId)
    {

        Instantiate(_signal, (Vector3)point, _camera.gameObject.transform.rotation);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
