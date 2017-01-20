using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Linq;
using UnityEngine.SceneManagement;

public enum LobbyRole
{
	STRATEGIST,
	HEROS
}

public enum RoomAction
{
	CREATE,
	JOIN,
	MATCHMAKING
}

public delegate void Void_D_Bool(bool b);
public delegate void Void_D_Bool_String(bool b, string s);
public delegate void Void_D_String_String(string b, string s);
public delegate void Void_D_String_String_Bool(string b, string s, bool b2);
public delegate int Int_D_Void();


public class PhotonLobbyManager : Photon.PunBehaviour
{

	public event Void_D_Bool myShowBarPanel;
	public event Void_D_Bool_String myShowConnect;
	public event Void_D_Bool myShowRole;
	public event Void_D_Bool myShowPrivateRoom;
	public event Void_D_Bool myShowReady;
	public event Void_D_Bool myShowMatch;
	public event Void_D_Bool myShowStree;
	public event Void_D_Bool myShowGameSelect;
	public event Void_D_Bool myShowRoomCreation;
	public event Void_D_Bool myShowNewsPanel;
	public event Void_D_Bool myShowOptionPanel;
	public event Void_D_Bool myShowMatchmaking;
	public event Void_D_Bool_String myShowPrivateWait;
	public event Void_D_Bool myShowHeroSelect;
	public event Void_D_Bool myShowTreeSelect;
	public event Void_D_String_String myUpdateInfo;
	public event Void_D_String myShowPopUp;
	public event Void_D_Void myClearRoomList;
	public event Void_D_String_String_Bool myAddRoomToRoomList;


	RoomInfo[] rooms = null;

	LobbyHeroPortrait heroIcon = null;

	public int waitingTime = 10;
	public PhotonChatInMatch chat;
	#region DynamicUiVar
	int nbStrat = 0;
	int nbHeros = 0;
	bool matchmaking = false;
	string playerName;
	string roomName;
	RoomAction action;

	
	public string[] herosPrefabs;

	public string PlayerName
	{
		get
		{
			return playerName;
		}

		set
		{
			playerName = value;
			PhotonNetwork.player.name = value;
			PlayerPrefs.SetString("playerName", value);
        }
	}

	public string RoomName
	{
		get
		{
			return roomName;
		}

		set
		{
			roomName = value;
		}
	}

	public void NbHerosAssign(string parameter)
	{
		nbHeros = int.Parse(parameter);
	}

	public void NbStratAssign(string parameter)
	{
		nbStrat = int.Parse(parameter);
	}

	public void HeroSelect(int choice)
	{
		heroIcon.current = choice;
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		playerProperties["hero"] = herosPrefabs[choice];
		PhotonNetwork.player.SetCustomProperties(playerProperties);
	}
	#endregion


	void Start()
	{
		if (PlayerPrefs.GetString("playerName") == null)
			PlayerPrefs.SetString("playerName", System.Environment.UserName);
		playerName = PlayerPrefs.GetString("playerName");
        Invoke("ButtonConnect", 3);
	}

	void Update()
	{

	}
	public void ButtonStree()
	{
		myShowStree(true);
	}

	public void ButtonCreateRoom()
	{
		action = RoomAction.CREATE;
		myShowRoomCreation(false);
		myShowRole(true);
	}

	public void ButtonJoinRoom()
	{
		action = RoomAction.JOIN;
		myShowRoomCreation(false);
		myShowRole(true);
	}

	public void ButtonPlay()
	{
		myShowStree(false);
		myShowRole(false);
		myShowRoomCreation(false);
		myShowNewsPanel(true);
		myShowGameSelect(true);
	}

	public void ButtonNews()
	{
		myShowRole(false);
		myShowGameSelect(false);
		myShowRoomCreation(false);
		myShowStree(false);
		myShowNewsPanel(true);
	}

	public void ButtonOptions()
	{
		myShowOptionPanel(true);
	}

	public void ButtonMatchmaking()
	{
		action = RoomAction.MATCHMAKING;
		myShowGameSelect(false);
		myShowNewsPanel(false);
		myShowRole(true);
	}

	public void ButtonRefresh()
	{
		myClearRoomList();

		var roomList = PhotonNetwork.GetRoomList();
		if (roomList.Length == 0)
		{
			myAddRoomToRoomList("No Room Found", "", false);
			return;
		}
		for (int i = 0; i < roomList.Length; i++)
		{
			var room = roomList[i];
			var desc = room.open && room.visible && room.playerCount < room.maxPlayers ? "" : "<color=red>";
            desc += room.playerCount + "/" + room.maxPlayers + (room.open ? "" : " In Game" )+ (room.visible ? "" : " Invisible");
			desc += room.open && room.visible && room.playerCount < room.maxPlayers ? "" : "</color>";
			myAddRoomToRoomList(room.name,  desc, room.open && room.visible && room.playerCount < room.maxPlayers);
		}
	}

	public void ButtonPrivatematch()
	{
		myShowGameSelect(false);
		myShowNewsPanel(false);
		myShowRoomCreation(true);

		ButtonRefresh();
	}
	public void ChooseRole(int role)
	{
		if (action == RoomAction.CREATE)
		{
			CreateRoom(playerName + "RoomP", 4, 2, (LobbyRole)role);
			myShowRole(false);
			myShowPrivateWait(true, playerName + " room");
		}
		else if (action == RoomAction.JOIN)
		{
			myShowRole(false);
			myShowPrivateWait(true, roomName + " room");
			JoinRoom(roomName, (LobbyRole)role);
		}
		else if (action == RoomAction.MATCHMAKING)
		{
			myShowRole(false);
			myShowMatchmaking(true);
			myShowNewsPanel(true);
			SearchRoom((LobbyRole)role);
		}
	}

	void SearchRoom(LobbyRole role)
	{
		matchmaking = true;
		if (rooms == null)
			rooms = PhotonNetwork.GetRoomList();
		for  (int i = 0; i < rooms.Length; i++)
		{
			var room = rooms[i];			
			if (room != null && room.open && room.playerCount+1 < room.maxPlayers && room.name[room.name.Length - 1] != 'P')
			{
				if (JoinRoom(room.name, role))
					return;
				rooms[i] = null;
			}
		}
		matchmaking = false;
		rooms = null;
		CreateRoom(playerName + "Room", 2, 6, (LobbyRole)role);
	}

	public void ButtonQuitRoom()
	{
		this.myShowMatchmaking(false);
		this.myShowPrivateWait(false, "");
		PhotonNetwork.LeaveRoom();
	}

	public void ButtonConnect()
	{
		if (!PhotonNetwork.connected)
		{
			PhotonNetwork.autoJoinLobby = true;
			PhotonNetwork.ConnectUsingSettings("1.0");
		}
	}


	public bool JoinRoom(string roomName, LobbyRole role)
	{
		var room = PhotonNetwork.GetRoomList().SingleOrDefault(x => x.name == roomName);
		if (room != null)
		{
			if (room.playerCount == room.maxPlayers)
			{
				if (!matchmaking)
				{
					myShowPopUp("Too many player");
					ButtonQuitRoom();
					myShowNewsPanel(true);
				}
				return false;
			}			
		}
		else
		{
			if (!matchmaking)
			{
				myShowPopUp("La room n'existe pas");
				ButtonQuitRoom();
				myShowNewsPanel(true);
			}
			myShowBarPanel(true);
			return false;
		}
		Debug.Log("Join room " + roomName);
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		playerProperties["hero"] = "Archer";
		playerProperties["role"] = role;
		PhotonNetwork.player.SetCustomProperties(playerProperties);
		return PhotonNetwork.JoinRoom(roomName);
	}

	public void CreateRoom(string roomName, int stratCount, int heroCount, LobbyRole role)
	{
		RoomOptions options = new RoomOptions();
		options.maxPlayers = (byte)(stratCount + heroCount);
		options.isOpen = true;
		options.isVisible = true;
		options.cleanupCacheOnLeave = true;
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		playerProperties["role"] = role;
		playerProperties["hero"] = "Archer";
		PhotonNetwork.player.SetCustomProperties(playerProperties);

		if (!PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default))
			Debug.LogError("Failed to create room");
	}

	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
		customRoomProperties["strat"] = 0;
		customRoomProperties["heros"] = 0;
		if (nbHeros == 0)
			nbHeros = 6;
		if (nbStrat == 0)
			nbStrat = 2;
		customRoomProperties["maxStrat"] = nbStrat;
		customRoomProperties["maxHeros"] = nbHeros;
		PhotonNetwork.room.SetCustomProperties(customRoomProperties);
	}

	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		myShowBarPanel(true);
		myShowConnect(false, "");
		myShowNewsPanel(true);
	}

	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		base.OnPhotonJoinRoomFailed(codeAndMsg);
		myShowPopUp("La room est pas accessible" + codeAndMsg[1]);
		Debug.Log("La vrai raison : " + codeAndMsg);
		myShowBarPanel(true);
		ButtonQuitRoom();
		myShowNewsPanel(true);
	}

	public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		base.OnPhotonCreateRoomFailed(codeAndMsg);
		myShowPopUp("La room ne peut pas être créee : " + codeAndMsg[1]);
		Debug.Log("La vrai raison : " + codeAndMsg);
		ButtonQuitRoom();
		myShowNewsPanel(true);
		myShowBarPanel(true);
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		
		LobbyRole role = (LobbyRole)PhotonNetwork.player.customProperties["role"];
		if ((int)PhotonNetwork.room.customProperties[role == LobbyRole.STRATEGIST ? "strat" : "heros"] == (int)PhotonNetwork.room.customProperties[role == LobbyRole.STRATEGIST ? "maxStrat" : "maxHeros"])
		{
			if (!matchmaking)
			{
				myShowPopUp("Mauvais role dommage");
				ButtonQuitRoom();
				myShowNewsPanel(true);
			}
			PhotonNetwork.LeaveRoom();			
			if (matchmaking)
			{
				rooms.SingleOrDefault(x => x.name == roomName);
				for (int i = 0; i < rooms.Length; i++)
				{
					if (rooms[i] != null && rooms[i].name == PhotonNetwork.room.name)
					{
						rooms[i] = null;
					}
                }
				SearchRoom(role);
				return;
			}
			myShowBarPanel(true);
			return;
		}
		if (action != RoomAction.MATCHMAKING)
			myShowPrivateRoom(true);
			
		Room currentRoom = PhotonNetwork.room;

		ExitGames.Client.Photon.Hashtable customProperties = currentRoom.customProperties;
		customProperties[role == LobbyRole.STRATEGIST ? "strat" : "heros"] = (int)currentRoom.customProperties[role == LobbyRole.STRATEGIST ? "strat" : "heros"] + 1;
		currentRoom.SetCustomProperties(customProperties);
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		playerProperties["ready"] = false;
		if (role == LobbyRole.STRATEGIST)
		{
			foreach (var p in PhotonNetwork.otherPlayers)
			{
				if ((LobbyRole)p.customProperties["role"] == LobbyRole.STRATEGIST && p.customProperties.ContainsKey("team"))
				{
					playerProperties["team"] = (e_Team)p.customProperties["team"] == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1;
				}
			}
			if (!playerProperties.ContainsKey("team"))
				playerProperties["team"] = e_Team.TEAM1;
		}
		else
		{
			int t1p = 0;
			int t2p = 0;
			foreach (var p in PhotonNetwork.otherPlayers)
			{
				if ((LobbyRole)p.customProperties["role"] == LobbyRole.HEROS && p.customProperties.ContainsKey("team"))
					{
						if ((e_Team)p.customProperties["team"] == e_Team.TEAM1)
							t1p++;
						else
							t2p++;
					}
			}
				playerProperties["team"] = t1p <= t2p ? e_Team.TEAM1 : e_Team.TEAM2;
		}
		PhotonNetwork.player.SetCustomProperties(playerProperties);
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		base.OnPhotonPlayerDisconnected(otherPlayer);
		if (heroIcon != null)
			return;
		LobbyRole role = (LobbyRole)otherPlayer.customProperties["role"];
		Room currentRoom = PhotonNetwork.room;
		ExitGames.Client.Photon.Hashtable customProperties = currentRoom.customProperties;
		customProperties[role == LobbyRole.STRATEGIST ? "strat" : "heros"] = (int)currentRoom.customProperties[role == LobbyRole.STRATEGIST ? "strat" : "heros"] - 1;
		currentRoom.SetCustomProperties(customProperties);

		string playerStates1 = "";
		string playerStates2 = "";
		foreach (PhotonPlayer player in PhotonNetwork.playerList)
		{
			if (((int)player.customProperties["team"] + 1) == 1)
				playerStates1 += player.name + " - " + ((LobbyRole)player.customProperties["role"] == LobbyRole.STRATEGIST ? "Strategist" : "Mecha") + "\n";
			else
				playerStates2 += player.name + " - " + ((LobbyRole)player.customProperties["role"] == LobbyRole.STRATEGIST ? "Strategist" : "Mecha") + "\n";
        }
		myUpdateInfo(playerStates1, playerStates2);
	}

	public void SetReady()
	{
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		if (PhotonNetwork.player.customProperties["ready"] != null)
			playerProperties["ready"] = !(bool)(PhotonNetwork.player.customProperties["ready"]);
		else
			playerProperties["ready"] = true;
		myShowReady((bool)playerProperties["ready"]);
        PhotonNetwork.player.SetCustomProperties(playerProperties);
	}

	public void ChangeTeam()
	{
		LobbyRole role = (LobbyRole)PhotonNetwork.player.customProperties["role"];
		ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
		playerProperties["team"] = (e_Team)PhotonNetwork.player.customProperties["team"];
        if (role == LobbyRole.STRATEGIST)
		{
			foreach (var p in PhotonNetwork.otherPlayers)
			{
				if ((LobbyRole)p.customProperties["role"] == LobbyRole.STRATEGIST && p.customProperties.ContainsKey("team"))
				{
					playerProperties["team"] = (e_Team)p.customProperties["team"];
					ExitGames.Client.Photon.Hashtable otherProperties = new ExitGames.Client.Photon.Hashtable();
					otherProperties["team"] = (e_Team)p.customProperties["team"] == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1;
					PhotonNetwork.player.SetCustomProperties(playerProperties);
					PhotonNetwork.player.SetCustomProperties(otherProperties);
					return;
				}
			}
			playerProperties["team"] = (e_Team)PhotonNetwork.player.customProperties["team"] == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1;
		}
		else
		{
			int tp = 0;
			foreach (var p in PhotonNetwork.otherPlayers)
			{
				if ((LobbyRole)p.customProperties["role"] == LobbyRole.HEROS && p.customProperties.ContainsKey("team") && (e_Team)p.customProperties["team"] != (e_Team)PhotonNetwork.player.customProperties["team"])
				{
					tp++;
				}
			}
			if (tp < 3)
				playerProperties["team"] = (e_Team)PhotonNetwork.player.customProperties["team"] == e_Team.TEAM1 ? e_Team.TEAM2 : e_Team.TEAM1;
			else
				myShowPopUp("Too many mecha in other team (" + tp + " max");
		}
		PhotonNetwork.player.SetCustomProperties(playerProperties);
	}

	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
		if (heroIcon != null)
			return;
		if (PhotonNetwork.room == null)
			return;
		string playerStates1 = "";
		string playerStates2 = "";
		foreach (PhotonPlayer player in PhotonNetwork.playerList)
		{
			if (player.customProperties.ContainsKey("team") && ((e_Team)player.customProperties["team"]) == e_Team.TEAM1)
				playerStates1 += player.name + " - " + ((LobbyRole)player.customProperties["role"] == LobbyRole.STRATEGIST ? "Strategist" : "Mecha") + "\n";
			else
				playerStates2 += player.name + " - " + ((LobbyRole)player.customProperties["role"] == LobbyRole.STRATEGIST ? "Strategist" : "Mecha") + "\n";
		}
		myUpdateInfo(playerStates1, playerStates2);
		if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers)
		{
			GetIntoTheMatch();
        }
	}

	public void GetIntoTheMatch()
	{
		PhotonNetwork.room.open = false;
        this.photonView.RPC("ShowMatch", PhotonTargets.All);
	} 

	public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		base.OnPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
		string customInfo = "";
		foreach (var debug in PhotonNetwork.player.customProperties)
		{
			customInfo += debug.Key + " : " + debug.Value + "\n";
		}
	}

	 [PunRPC]
	 public void ShowMatch()
	{
		if (this.heroIcon == null)
		{
			this.heroIcon = PhotonNetwork.Instantiate("Lobby/LobbyPlayerIcon", Vector3.zero, Quaternion.identity, 0).GetComponent<LobbyHeroPortrait>();
			chat.Connect(PhotonNetwork.room.name + PhotonNetwork.player.customProperties["team"]);
			myShowMatch(true);
			myShowNewsPanel(false);
			myShowRole(false);
			myShowGameSelect(false);
			myShowRoomCreation(false);
			myShowPrivateRoom(false);
			myShowBarPanel(false);

			LobbyRole role = (LobbyRole)PhotonNetwork.player.customProperties["role"];

			if (role == LobbyRole.HEROS)
				myShowHeroSelect(true);
			else
				myShowTreeSelect(true);
			
			Invoke("LoadLevel", waitingTime);
		}
	}

	void LoadLevel()
	{
		chat.Disconnect();
		PhotonNetwork.LoadLevel(1);
    }
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		myShowBarPanel(false);
		myShowConnect(true, "Connexion rekt: " + cause.ToString() + "\n Reconnect in 5 seconds...");
		ButtonQuitRoom();
		ButtonConnect();
		//Invoke("ButtonConnect", 5);

	}

	public override void OnDisconnectedFromPhoton()
	{
		myShowBarPanel(false);
		myShowConnect(true, "Disconnected\n Reconnect in 5 seconds...");
		ButtonQuitRoom();
		ButtonConnect();
		//Invoke("ButtonConnect", 5);
	}
}
