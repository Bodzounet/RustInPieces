using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEndGame : MonoBehaviour {

	private bool end;
	private Image Panel;
	// Use this for initialization
	void Start ()
	{
		Panel = this.GetComponentInChildren<Image>();

	}

	public void EndTrigger()
	{
		end = true;
		Invoke("EndGame", 4.0f);
	}

	public void EndGame()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}

	// Update is called once per frame
	void Update ()
	{
		if (end)
		{
			Panel.color = new Color(1, 1, 1, Panel.color.a + Time.deltaTime / 4);
		}
	}
}
