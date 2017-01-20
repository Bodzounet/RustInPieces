using UnityEngine;
using System.Collections;

public class UIQuitGame : MonoBehaviour {

	public SkillTreeBuilder.PopUp popUp;

    private bool _canUseInput = true;

    public bool CanUseInput
    {
        get
        {
            return _canUseInput;
        }

        set
        {
            _canUseInput = value;
        }
    }

    void Update ()
	{
		if (CanUseInput && Input.GetButton("Menu"))
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			popUp.SpawnPopUp("Do you want to RAGE QUIT ?");
		}
	}

	public void QuitToLobby()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}
}
