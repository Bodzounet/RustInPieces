using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;
using UnityEngine.EventSystems;

public class PhotonChatInMatch : MonoBehaviour, IChatClientListener
{


	public Text messages;
	public InputField field;
	ChatClient chatClient = null;
	private ScrollRect scrollRect;
    string currentRoom;
	bool focus;
	// Use this for initialization
	void Start()
	{
		this.scrollRect = GetComponent<ScrollRect>();

		//foreach (var g in this.GetComponentsInChildren<Image>())
		//{
		//	Color col = g.color;
		//	col.a = 0.2f;
		//	g.color = col;
		//}
		//focus = field.isFocused;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (chatClient != null && chatClient.State != ChatState.Disconnected && chatClient.State != ChatState.Disconnecting)
			chatClient.Service();
	}


	void Update()
	{
		if (Input.GetButton("Chat") && EventSystem.current.currentSelectedGameObject != field.gameObject)
		{
			EventSystem.current.SetSelectedGameObject(field.gameObject, null);
			field.OnPointerClick(null);
		}
		if (Input.GetButton("Submit"))
		{
			EventSystem.current.SetSelectedGameObject(null, null);
		}

		//if (focus != field.isFocused)
		//{
		//	focus = field.isFocused;
		//	foreach (var g in this.GetComponentsInChildren<Image>())
		//	{
		//		Color col = g.color;
		//		col.a = focus ? 0.6f : 0.2f;
		//		g.color = col;
		//	}
			
		//}
	}

	public void Connect(string room)
	{
		if (chatClient == null)
			chatClient = new ChatClient(this);
		// Set your favourite region. "EU", "US", and "ASIA" are currently supported.
		chatClient.ChatRegion = "EU";
		chatClient.Connect("e1417631-6602-410b-8dee-ad7c7ebf92cc", "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(PhotonNetwork.player.name));

		this.currentRoom = room;
    }

	public void Send(string message)
	{
		if (message.Length > 0)
			chatClient.PublishMessage(currentRoom, message);
		field.text = "";
    }

	public void DebugReturn(DebugLevel level, string message)
	{

	}

	public void OnChatStateChange(ChatState state)
	{
		
	}

	public void OnConnected()
	{
		messages.text += "Connecté au chat\n";
		chatClient.Subscribe(new string[] {this.currentRoom});
	}

	private void Reconnect()
	{
		Connect(this.currentRoom);
	}

	public void Disconnect()
	{
		if (chatClient != null)
		chatClient.Disconnect();
		
	}

	
	void OnApplicationQuit()
	{
		this.Disconnect();
	}

	void OnDestroy()
	{
		this.Disconnect();
	}

	public void OnDisconnected()
	{
		if (chatClient.DisconnectedCause != ChatDisconnectCause.None)
			messages.text += "Deconnecté du chat car " + chatClient.DisconnectedCause.ToString() + "\nReconnexion dans 2 secondes...";
	}

	public void OnGetMessages(string channelName, string[] senders, object[] msg)
	{
		for (int i = 0; i < senders.Length; i++)
		{
			messages.text += "<" + senders[i] + ">:" + msg[i].ToString() + "\n";
		}
		scrollRect.normalizedPosition = new Vector2(0, 0);
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		messages.text += "Rejoint salle " + channels[0] + "\n";
	}

	public void OnUnsubscribed(string[] channels)
	{
		messages.text += "Quitte la salle " + channels[0] + "\n";
	}


}
