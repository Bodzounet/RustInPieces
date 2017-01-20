using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

[Serializable]
public struct LobbyNewsCollection
{
	public LobbyNews[] news;
}

[Serializable]
public struct LobbyNews
{
	public string title;
	public string text;
	public string img;
	public string url;
}

public class LobbyNewsFeed : MonoBehaviour {

	public GameObject welcomeText;
	public GameObject newsPrefab;
	// Use this for initialization
	IEnumerator Start()
	{
		welcomeText.GetComponent<Text>().text = "Connecting to news";
		WWW www = new WWW("http://eip.epitech.eu/2017/rustinpieces/news.php");
		yield return www;

		
		try
		{
			LobbyNewsCollection news = JsonUtility.FromJson<LobbyNewsCollection>(www.text);
			foreach (LobbyNews n in news.news)
			{
				GameObject nContainer = Instantiate(newsPrefab);
				nContainer.transform.SetParent(transform);

				nContainer.transform.FindChild("Title").GetComponent<Text>().text = n.title;
				nContainer.transform.FindChild("Text").GetComponent<Text>().text = n.text;
				Navigation nav = nContainer.GetComponent<Button>().navigation;
				nav.mode = Navigation.Mode.None;
				nContainer.GetComponent<Button>().navigation = nav;
				string u = n.url.ToString();
				AddListener(nContainer.GetComponent<Button>(), u);
				if (n.img.StartsWith("http"))
					StartCoroutine("NewsImg", new KeyValuePair<string, GameObject>(n.img, nContainer));
			}
			welcomeText.SetActive(false);
		}
		catch 
		{
			welcomeText.GetComponent<Text>().text = "Hello darkness my old friend...";
        }

		

    }

	void AddListener(Button b, string value)
	{
		b.onClick.AddListener(() => GoToUrl(value));
	}

	IEnumerator NewsImg(KeyValuePair<string, GameObject> img)
	{
		WWW www = new WWW(img.Key);
		yield return www;
		img.Value.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToUrl(string url)
	{
		Debug.Log(url);
		Application.OpenURL(url);
    }
}
