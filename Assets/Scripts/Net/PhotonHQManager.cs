using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonHQManager : MonoBehaviour
{


	public RectTransform UIHealth;
	public GameObject EndPanel;

	private Entity _entity;
	private PhotonView _pView;
	bool end = false;
	// Use this for initialization
	void Start()
	{
		_entity = GetComponent<Entity>();
		
		_pView = GetComponent<PhotonView>();
	}

	// Update is called once per frame
	void Update()
	{
		float currentHealth, maxHealth;
		currentHealth = _entity.getStat(Entity.e_StatType.HP_CURRENT);
		maxHealth = _entity.getStat(Entity.e_StatType.HP_MAX);
		UIHealth.sizeDelta = new Vector2(200 * (currentHealth / maxHealth), UIHealth.sizeDelta.y);
		if (currentHealth <= 0)
		{
			if (!end)
			{
				
				_pView.RPC("Result", PhotonTargets.All, _entity.Team == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1);
			}
		}
	}



	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(_entity.getStat(Entity.e_StatType.HP_CURRENT));
		}
		else
		{
			_entity.setStat(Entity.e_StatType.HP_CURRENT, (float)stream.ReceiveNext());
		}
	}

	[PunRPC]
	void Result(e_Team winTeam)
	{
		EndPanel.SetActive(true);
		EndPanel.GetComponent<UIEndGame>().EndTrigger();
		end = true;
		if (winTeam != (e_Team)PhotonNetwork.player.customProperties["team"])
		{
			EndPanel.GetComponentInChildren<Text>().color = Color.red;
			EndPanel.GetComponentInChildren<Text>().text = "DEFEAT";
        }
		else
		{
			EndPanel.GetComponentInChildren<Text>().color = Color.blue;
			EndPanel.GetComponentInChildren<Text>().text = "VICTORY";
		}
		
	}

	
}
