using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Hold the list of all existing minion for one team.
/// Stop working corretly if you create a minion with another function that the one in this class.
/// </summary>
public class MinionManager : MonoBehaviour 
{
    private TeamFactory _myTeamFactory;
	private PhotonView _pView;

    private Dictionary<int, GameObject> _minions = new Dictionary<int, GameObject>();
	private Dictionary<int, Vector3> _minionsLerpPos = new Dictionary<int, Vector3>();
	private Dictionary<int, Quaternion> _minionsLerpRot = new Dictionary<int, Quaternion>();
	public List<GameObject> Minions
    {
        get { return _minions.Values.ToList(); }
    }

    private LaneId[] _lanes;

    private StrategistManager _sm;
	private int _nbMinion = 0;

    void Start()
    {
        _myTeamFactory = GameObject.FindObjectsOfType<TeamFactory>().Single(x => x.MyTeam == GetComponent<StrategistManager>().Team);
        _lanes = FindObjectsOfType<LaneId>().OrderBy(x => x.id).ToArray();
        _sm = GetComponent<StrategistManager>();

		_pView = this.transform.GetComponent<PhotonView>();
    }

	void Update()
	{
		foreach (KeyValuePair<int, GameObject> pair in _minions)
		{
			if (_minions.ContainsKey(pair.Key) && _minions[pair.Key] != null && _minionsLerpPos.ContainsKey(pair.Key) && _minionsLerpPos[pair.Key] != null && _minionsLerpRot.ContainsKey(pair.Key) && _minionsLerpRot[pair.Key] != null)
			{
				_minions[pair.Key].transform.position = Vector3.Lerp(_minions[pair.Key].transform.position, _minionsLerpPos[pair.Key], 0.2f);
				_minions[pair.Key].transform.rotation = Quaternion.Lerp(_minions[pair.Key].transform.rotation, _minionsLerpRot[pair.Key], 0.2f);
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			foreach (KeyValuePair<int, GameObject> minion in _minions)
			{
				stream.SendNext(minion.Key);
				stream.SendNext(minion.Value.transform.position);
				stream.SendNext(minion.Value.transform.rotation);
				//stream.SendNext(minion.Value.GetComponent<Entity>().getStat(Entity.e_StatType.HP_CURRENT));
			}
		}
		else
		{
			//foreach (KeyValuePair<int, GameObject> minion in Minions)
			//{
			while (stream.PeekNext() != null)
			{
				int id = (int)stream.ReceiveNext();
				if (_minions.ContainsKey(id) && _minions[id] != null)
				{
					_minionsLerpPos[id] = (Vector3)stream.ReceiveNext();
					_minionsLerpRot[id] = (Quaternion)stream.ReceiveNext();
					//_minions[id].GetComponent<Entity>().setStat(Entity.e_StatType.HP_CURRENT, (float)stream.ReceiveNext());
				}
				else
				{
					for (int i = 0; i < 2; i++)
						stream.ReceiveNext();
				}
			}
			//}
		}
	}

	public GameObject CreateMinion(UnitsInfo.e_UnitType type, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), int lane = 1)
	{
		GameObject newMinion = _myTeamFactory.CreateMinion(type, position, rotation);

        Transform nearestWaypoint;
        if (_sm.Team == e_Team.TEAM1)
            nearestWaypoint = _lanes[lane - 1].childrens.Where(x => x.transform.position.x > position.x).OrderBy(y => Vector3.Distance(y.position, position)).First();
        else
            nearestWaypoint = _lanes[lane - 1].childrens.Where(x => x.transform.position.x < position.x).OrderBy(y => Vector3.Distance(y.position, position)).First();

        int wpIndexInHierarchy = _lanes[lane - 1].childrens.IndexOf(nearestWaypoint);

        PatrolAI ai = newMinion.GetComponentInChildren<PatrolAI>();
        ai.Lane = lane;

        if (_sm.Team == e_Team.TEAM1)
            ai.WayPoint = new Transform[_lanes[lane - 1].childrens.Count - wpIndexInHierarchy];
        else
            ai.WayPoint = new Transform[wpIndexInHierarchy + 1];

        for (int i = 0; i < ai.WayPoint.Length; i++)
		{
            if (_sm.Team == e_Team.TEAM1)
                ai.WayPoint[i] = _lanes[lane - 1].childrens[wpIndexInHierarchy + i];
            else
                ai.WayPoint[i] = _lanes[lane - 1].childrens[wpIndexInHierarchy - i];
		}
		newMinion.AddComponent<MinionManager_CleanMinion>().manager = this;
		newMinion.GetComponent<MinionManager_CleanMinion>().IdMinion = _nbMinion;
		_minions[_nbMinion] = newMinion;
		_nbMinion++;
        return newMinion;
	}

	[PunRPC]
	private void RPCRemoveMinion(int index)
	{
		if (_minions.ContainsKey(index))
			_minions.Remove(index);
		if (_minionsLerpPos.ContainsKey(index))
			_minionsLerpPos.Remove(index);
		if (_minionsLerpRot.ContainsKey(index))
			_minionsLerpRot.Remove(index);

	}

	public void RemoveMinion(int idMinion)
	{
        if (_pView.isMine)
		    _pView.RPC("RPCRemoveMinion", PhotonTargets.All, idMinion);
    }

	//public GameObject CreateMinion(UnitsInfo.e_UnitType type, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), int lane = 1)
 //   {
 // 		_pView.RPC("RPCCreateMinion", PhotonTargets.AllBuffered, type, position, rotation, lane);
	//	return _minions[Minions.Count - 1];
 //   }

    public List<GameObject> GetAllMinionOfType(UnitsInfo.e_UnitType type)
    {
        return _minions.Values.Where(x => x.GetComponent<UnitEntity>().type == type).ToList();
    }
}
