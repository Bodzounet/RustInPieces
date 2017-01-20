using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LobbyOptions : MonoBehaviour {

	Resolution[] resolutionsList;
	public Dropdown resolutionDropDown;	 

	public bool FullscreenTougueule
	{
		get
		{
			if (PlayerPrefs.HasKey("fullscreen"))
				return PlayerPrefs.GetInt("fullscreen") == 1;
			return Screen.fullScreen;
		}

		set
		{
			Screen.fullScreen = value;
			PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
		}
	}

	public int ResolutionChange
	{
		set
		{
			Screen.SetResolution(resolutionsList[value].width, resolutionsList[value].height, Screen.fullScreen);
			PlayerPrefs.SetInt("width", resolutionsList[value].width);
			PlayerPrefs.SetInt("height", resolutionsList[value].height);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.HasKey("width"))
			Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), FullscreenTougueule);
		resolutionsList = Screen.resolutions;
		List <string>  resolutionsOptions = new List<string>();
		foreach(var reso in resolutionsList)
		{
			resolutionsOptions.Add(reso.width + "x" + reso.height);
		}
		resolutionDropDown.AddOptions(resolutionsOptions);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
