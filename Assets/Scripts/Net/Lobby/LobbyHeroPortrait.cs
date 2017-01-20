using UnityEngine;
using System.Collections;

public class LobbyHeroPortrait : MonoBehaviour {


	public Sprite[] portraits;
	private PhotonView pView;
	public int current = 0;
	// Use this for initialization
	void Start ()
	{
		pView = GetComponent<PhotonView>();
		if ((e_Team)pView.owner.customProperties["team"] != (e_Team)PhotonNetwork.player.customProperties["team"])
			transform.SetParent(GameObject.Find("Opp List").transform);
		else
			transform.SetParent(GameObject.Find("Allies List").transform);
		LobbyRole role = (LobbyRole)PhotonNetwork.player.customProperties["role"];
		if (role == LobbyRole.STRATEGIST)
			current = 5;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.GetComponentInChildren<UnityEngine.UI.Image>().sprite = portraits[current];
		this.GetComponentInChildren<UnityEngine.UI.Text>().text = pView.owner.name + "[" + (e_Team)pView.owner.customProperties["team"] + "]";
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(current);

		}
		else
		{
			this.current = (int)stream.ReceiveNext();

		}
	}

	}
