using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SkillTreeBuilder;
using UnityEngine.EventSystems;

public class PanelLobbyUi : MonoBehaviour {

	public PhotonLobbyManager manager;
	public GameObject barPanel;
	public GameObject privateRoomPanel;
	public GameObject roomCreationPanel;
	public GameObject rolePanel;
	public GameObject buttonConnect;
	public GameObject gameSelectPanel;
	public GameObject streePanel;
	public GameObject newsPanel;
	public GameObject matchPanel;
	public GameObject heroSelectPanel;
	public GameObject treeSelectPanel;
	public GameObject InputName;
	public GameObject buttonPlay;
	public GameObject timerTxt;
	public GameObject cancelButton;
	public GameObject RoomList;
	public GameObject optionPanel;
	public Button JoinButton;
	public Text RoomInfo;

	public GameObject roomSelectablePrefab;
	public PopUp popUp;
	public Text playerListTeam1;
	public Text playerListTeam2;


	bool inMatchmaking = false;
	bool inPreparation = false;
	//bool inPrivateRoom = false;
	bool isUnshowing = false;
	float secondsWaiting = 0;
	// Use this for initialization
	void Start ()
	{
		manager.myShowBarPanel += ShowBarPanel;
		manager.myShowConnect += ShowConnectButton;
		manager.myShowRole += ShowRole;
		manager.myShowPrivateRoom += ShowPrivateRoom;
		manager.myShowMatch += ShowMatchPanel;
		manager.myShowGameSelect += ShowGameSelect;
		manager.myShowRoomCreation += ShowRoomCreation;
		manager.myShowStree += ShowStree;
		manager.myShowNewsPanel += ShowNewsPanel;
		manager.myShowMatchmaking += ShowMatchmaking;
		manager.myShowPrivateWait += ShowPrivateWait;
		manager.myShowHeroSelect += ShowHeroSelect;
		manager.myShowTreeSelect += ShowTreeSelect;
		manager.myClearRoomList += ClearRoomList;
		manager.myAddRoomToRoomList += AddRoomToRoomList;
		manager.myShowOptionPanel += ShowOptionPanel;

		manager.myShowPopUp += popUp.SpawnPopUp;

		manager.myUpdateInfo += ListPlayer;
		InputName.GetComponent<InputField>().text = manager.PlayerName;
		UnshowAll();
		ShowBarPanel(false);
		ShowConnectButton(true, "Welcome to Rust In Pieces !");
    }

	void UnshowAll()
	{
		if (isUnshowing)
			return;
		isUnshowing = true;
		ShowConnectButton(false);
		ShowRole(false);
		ShowPrivateRoom(false);
		ShowGameSelect(false);
		ShowRoomCreation(false);
		ShowStree(false);
		ShowNewsPanel(false);
		ShowHeroSelect(false);
		ShowTreeSelect(false);
		ShowOptionPanel(false);
		isUnshowing = false;
    }

	void ClearRoomList()
	{

		for (int i = RoomList.transform.childCount; i > 0; i--)
		{
			Destroy(RoomList.transform.GetChild(i - 1).gameObject);
		}
		JoinButton.interactable = false;
		RoomInfo.text = "No Room Selected";
    }

	void AddRoomToRoomList(string room, string description, bool isOpen = true)
	{
		GameObject nRoom = Instantiate(roomSelectablePrefab);
		nRoom.transform.SetParent(RoomList.transform);

		nRoom.GetComponent<Text>().text = room + " - " + description;
		Navigation nav = nRoom.GetComponent<Selectable>().navigation;
		nav.mode = Navigation.Mode.None;
		nRoom.GetComponent<Selectable>().navigation = nav;

		EventTrigger trigger = nRoom.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((data) => { RoomClicked(data, room, description, isOpen);  });
		trigger.triggers.Add(entry);
	}

	void RoomClicked(BaseEventData basedata, string room, string description, bool isOpen)
	{
		var data = basedata as PointerEventData;
		SelectRoom(room, description, isOpen);
		if (data.clickCount == 2 && JoinButton.IsInteractable())
			manager.ButtonJoinRoom();
	}

	public void SelectRoom(string room, string description, bool interactable)
	{
		manager.RoomName = room;
		RoomInfo.text = room + " - " + description;
		JoinButton.interactable = interactable;
    }

	void ListPlayer(string playerteam1, string playerteam2)
	{
		playerListTeam1.text = playerteam1;
		playerListTeam2.text = playerteam2;
	}

	void ShowHeroSelect(bool active)
	{
		UnshowAll();
        heroSelectPanel.SetActive(active);
	}

	void ShowTreeSelect(bool active)
	{
		UnshowAll();
		treeSelectPanel.SetActive(active);
	}

	void ShowOptionPanel(bool active)
	{
		UnshowAll();
		optionPanel.SetActive(active);
	}

    void ShowMatchmaking(bool active)
	{
		inMatchmaking = active;
		//inPrivateRoom = false;

        if (active)
		{
			cancelButton.SetActive(true);
			buttonPlay.GetComponent<Button>().interactable = false;
			secondsWaiting = 0;
			buttonPlay.GetComponentInChildren<Text>().text = "Searching...\n" + (int)(secondsWaiting) + "s";
			buttonPlay.GetComponentInChildren<Text>().fontSize = 20;
		}
		else
		{
			cancelButton.SetActive(false);
			buttonPlay.GetComponent<Button>().interactable = true;
			buttonPlay.GetComponentInChildren<Text>().text = "Play";
			buttonPlay.GetComponentInChildren<Text>().fontSize = 33;
		}

	}

	void ShowPrivateWait(bool active, string roomname)
	{
		inMatchmaking = false;
		//inPrivateRoom = active;
		if (active)
		{
			cancelButton.SetActive(true);
			buttonPlay.GetComponent<Button>().onClick.RemoveAllListeners();
			buttonPlay.GetComponent<Button>().onClick.AddListener(delegate { ShowPrivateRoom(true); });
			secondsWaiting = 0;
			buttonPlay.GetComponentInChildren<Text>().text = "Waiting in " + roomname;
			buttonPlay.GetComponentInChildren<Text>().fontSize = 20;
		}
		else
		{
			cancelButton.SetActive(false);
			buttonPlay.GetComponent<Button>().onClick.RemoveAllListeners();
			buttonPlay.GetComponent<Button>().onClick.AddListener(delegate { manager.ButtonPlay(); });
			buttonPlay.GetComponentInChildren<Text>().text = "Play";
			buttonPlay.GetComponentInChildren<Text>().fontSize = 33;
		}

	}

	void ShowNewsPanel(bool active)
	{
		UnshowAll();
		newsPanel.SetActive(active);
	}

	void ShowGameSelect(bool active)
	{
		UnshowAll();
		gameSelectPanel.SetActive(active);
	}

	void ShowStree(bool active)
	{
		UnshowAll();
		streePanel.SetActive(active);
	}

	void ShowRoomCreation(bool active)
	{
		UnshowAll();
		roomCreationPanel.SetActive(active);
	}

	void ShowPrivateRoom(bool active)
	{
		UnshowAll();
		privateRoomPanel.SetActive(active);
	}

	void ShowMatchPanel(bool active)
	{
		UnshowAll();
		matchPanel.SetActive(active);
		if (active)
		{
			inMatchmaking = false;
			inPreparation = true;
			secondsWaiting = manager.waitingTime;
		}
		else
		{
			inMatchmaking = true;
			inPreparation = false;
			secondsWaiting = 0;
		}
	}
	void ShowBarPanel(bool active)
	{
		barPanel.SetActive(active);
    }

	void ShowConnectButton(bool active, string text = "Connecting...")
	{
		UnshowAll();
		buttonConnect.SetActive(active);
	}


	void ShowRole(bool active)
	{
		UnshowAll();
		rolePanel.SetActive(active);
    }

	public void ExitGame()
	{
		Debug.Log("je quit");
		Application.Quit();
	}


	
	// Update is called once per frame
	void Update () {
		if (inMatchmaking)
		{
			buttonPlay.GetComponentInChildren<Text>().fontSize = 20;
			buttonPlay.GetComponentInChildren<Text>().text = "Searching";
			for (int i = 0; i <= (int)(secondsWaiting * 2) % 3; i++)
				buttonPlay.GetComponentInChildren<Text>().text += ".";
			buttonPlay.GetComponentInChildren<Text>().text += "\n" + (int)(secondsWaiting) + "s";

			secondsWaiting += Time.deltaTime;
		}

		if(inPreparation)
		{
			if (secondsWaiting < 0)
				timerTxt.GetComponent<Text>().text = "Get Ready";
			else
				timerTxt.GetComponent<Text>().text = (int)(secondsWaiting) + "s";
			secondsWaiting -= Time.deltaTime;
		}
	}
}
