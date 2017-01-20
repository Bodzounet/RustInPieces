using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Void_D_Event(object content, int senderid);

public class EventManager : Singleton<EventManager>
{
    private Dictionary<byte, Void_D_Event> _eventCallbacks = new Dictionary<byte,Void_D_Event>();

	void Awake()
    {
        PhotonNetwork.OnEventCall += OnEvent;
	}
	
	void Update()
    {	
	}

    public void addEventCallback(byte code, Void_D_Event eventcall)
    {
        _eventCallbacks.Add(code, eventcall);
    }

    public void removeEventCallback(byte code)
    {
        _eventCallbacks.Remove(code);
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        if (eventcode >= 0 && eventcode <= 200)
        {
            //Debug.Log("eventcode : " + eventcode);
            _eventCallbacks[eventcode](content, senderid);
        }
    }
}