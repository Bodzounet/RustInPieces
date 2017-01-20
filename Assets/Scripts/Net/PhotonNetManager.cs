using UnityEngine;
using System.Collections;

public class PhotonNetManager : MonoBehaviour {

	int number = 0;
    public GameObject HeroUi;
	void Awake()
	{
		CreatePlayerObject();
    }


	void CreatePlayerObject()
	{
		e_Team team = (e_Team)PhotonNetwork.player.customProperties["team"];
        #region Stratege
        if ((LobbyRole)PhotonNetwork.player.customProperties["role"] == LobbyRole.STRATEGIST)
		{
			Vector3 position = GameObject.Find("Spawn" + ((int)team + 1)).transform.position;

			GameObject newPlayerObject = PhotonNetwork.Instantiate("Strategist/Strategist", Vector3.zero, Quaternion.identity, 0);
            number++;
			if (newPlayerObject.GetComponent<PhotonView>().isMine)
			{
				if (GameObject.Find("HeroUI") != null)
					GameObject.Find("HeroUI").SetActive(false);
                if (newPlayerObject.GetComponentInChildren<Camera>() == null)
					Debug.LogError("Camera non trouvé, il faut desactiver le component et pas le gameobject !!!");
				newPlayerObject.GetComponentInChildren<Camera>().transform.position = position + Vector3.up * 35;
				newPlayerObject.GetComponentInChildren<Camera>().enabled = true;
				newPlayerObject.GetComponentInChildren<Canvas>().enabled = true;
				//newPlayerObject.GetComponentInChildren<MouseMovement>().enabled = true;
				newPlayerObject.GetComponentInChildren<AudioListener>().enabled = true;
				if (newPlayerObject.GetComponentInChildren<PhotonChatInMatch>() != null)
					newPlayerObject.GetComponentInChildren<PhotonChatInMatch>().Connect(PhotonNetwork.room.name + PhotonNetwork.player.customProperties["team"]);
			}
		}
        #endregion
        #region mechas
        else 
		{
			Vector3 position = GameObject.Find("Spawn" + ((int)team + 1)).transform.position;
			GameObject newPlayerObject = PhotonNetwork.Instantiate("Heroes/" + PhotonNetwork.player.customProperties["hero"], position, Quaternion.identity, 0);
			if (newPlayerObject.GetComponent<PhotonView>().isMine)
			{
                HeroUi.SetActive(true);
				if (HeroUi.GetComponentInChildren<PhotonChatInMatch>() != null)
					HeroUi.GetComponentInChildren<PhotonChatInMatch>().Connect(PhotonNetwork.room.name + PhotonNetwork.player.customProperties["team"]);
				if (newPlayerObject.GetComponentInChildren<Camera>() == null)
					Debug.LogError("Camera non trouvé, il faut desactiver le component et pas le gameobject !!!");
				newPlayerObject.GetComponentInChildren<Camera>().enabled = true;
				newPlayerObject.GetComponentInChildren<HeroCameraController>().enabled = true;
				newPlayerObject.GetComponent<HeroController>().enabled = true;
				newPlayerObject.GetComponentInChildren<AudioListener>().enabled = true;
				newPlayerObject.GetComponentInChildren<HeroSpellCasterManager>().enabled = true;
				newPlayerObject.GetComponentInChildren<HeroStats>().enabled = true;
				if (newPlayerObject.GetComponentInChildren<PhotonChatInMatch>() != null)
					newPlayerObject.GetComponentInChildren<PhotonChatInMatch>().Connect(PhotonNetwork.room.name + PhotonNetwork.player.customProperties["team"]);
			}
        }
        #endregion
    }
}
