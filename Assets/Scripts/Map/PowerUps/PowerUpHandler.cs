using UnityEngine;
using System.Collections;

public class PowerUpHandler : MonoBehaviour
{
    public Camera myCam;
    public GameObject[] buttonPowerUps;
    public GameObject[] doorTab;
    public RectTransform[] rectransformPowerUps;
    public int levelBox;
    public int levelFake;

    private GameObject _target;
    private RectTransform[] _rectransformDoors;

    // Use this for initialization
    void Start()
    {

        //Debug.Log("length = " + buttonPowerUps.Length);
        rectransformPowerUps = new RectTransform[buttonPowerUps.Length];
        for (var i = 0; i < buttonPowerUps.Length; i++)
        {
            rectransformPowerUps[i] = buttonPowerUps[i].GetComponent<RectTransform>();
        }

        _rectransformDoors = new RectTransform[doorTab.Length];
        for (var i = 0; i < doorTab.Length; i++)
        {
            _rectransformDoors[i] = doorTab[i].GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit vHit = new RaycastHit();
            Ray vRay = myCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(vRay, out vHit, 1000, (1 << LayerMask.NameToLayer("GameElement"))))
            {
                switch (vHit.transform.gameObject.tag)
                {

                    case "PowerUp":
                        _target = vHit.transform.gameObject;
                        if (_target.GetComponent<PowerUp>().team == this.transform.root.GetComponent<StrategistManager>().Team)
                        {
                            if (levelBox >= 1)
                            {
                                buttonPowerUps[0].SetActive(true);
                                buttonPowerUps[1].SetActive(true);

                            }
                            if (levelBox >= 2)
                            {
                                buttonPowerUps[2].SetActive(true);
                            }
                            if (levelBox >= 3)
                            {
                                buttonPowerUps[3].SetActive(true);
                            }
                        }
                        else
                        {
                            if (levelFake == 1)
                            {
                                buttonPowerUps[4].SetActive(true);
                            }
                        }
                        StartCoroutine(PrintScreenBonusBoxes(vHit.transform.gameObject));
                        break;
                    case "Door":
                        _target = vHit.transform.gameObject;
                        doorTab[0].SetActive(true);
                        StartCoroutine(PrintDoor(_target));
                        break;

                }
            }
        }
    }

    IEnumerator PrintDoor(GameObject target)
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 wantedpos = myCam.WorldToScreenPoint(target.transform.position);
                Vector3 tPos = new Vector3(wantedpos.x /*- Screen.height / width */, wantedpos.y + 100/* + Screen.height / height */, wantedpos.z);
                _rectransformDoors[0].position = tPos;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PrintScreenBonusBoxes(GameObject target)
    {
        while (true)
        {
            Vector3 wantedpos = myCam.WorldToScreenPoint(target.transform.position);

            if (_target.GetComponent<PowerUp>().team == this.transform.root.GetComponent<StrategistManager>().Team)
            {
                if (levelBox == 0)
                {
                    Vector3 tPos = new Vector3(wantedpos.x /*- Screen.height / width */, wantedpos.y + 100/* + Screen.height / height */, wantedpos.z);
                    rectransformPowerUps[0].position = tPos;
                    tPos = new Vector3(wantedpos.x - 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[1].position = tPos;
                }
                else if (levelBox == 1)
                {
                    Vector3 tPos = new Vector3(wantedpos.x, wantedpos.y + 100, wantedpos.z);
                    rectransformPowerUps[0].position = tPos;
                    tPos = new Vector3(wantedpos.x - 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[1].position = tPos;
                    tPos = new Vector3(wantedpos.x + 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[2].position = tPos;
                }
                else if (levelBox == 2)
                {
                    Vector3 tPos = new Vector3(wantedpos.x, wantedpos.y + 100, wantedpos.z);
                    rectransformPowerUps[0].position = tPos;
                    tPos = new Vector3(wantedpos.x - 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[1].position = tPos;
                    tPos = new Vector3(wantedpos.x + 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[2].position = tPos;
                    tPos = new Vector3(wantedpos.x + 150, wantedpos.y - 100, wantedpos.z);
                    rectransformPowerUps[3].position = tPos;
                }
                else if (levelBox == 3)
                {
                    Vector3 tPos = new Vector3(wantedpos.x, wantedpos.y + 100, wantedpos.z);
                    rectransformPowerUps[0].position = tPos;
                    tPos = new Vector3(wantedpos.x - 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[1].position = tPos;
                    tPos = new Vector3(wantedpos.x + 150, wantedpos.y, wantedpos.z);
                    rectransformPowerUps[2].position = tPos;
                    tPos = new Vector3(wantedpos.x + 150, wantedpos.y - 100, wantedpos.z);
                    rectransformPowerUps[3].position = tPos;
                    tPos = new Vector3(wantedpos.x - 150, wantedpos.y - 100, wantedpos.z);
                    rectransformPowerUps[5].position = tPos;
                }
            }
            else
            {
                if (levelFake == 1)
                {
                    Vector3 tPos = new Vector3(wantedpos.x, wantedpos.y + 100, wantedpos.z);
                    rectransformPowerUps[4].position = tPos;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void Desactivebuttons()
    {
        foreach (GameObject obj in buttonPowerUps)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in doorTab)
        {
            obj.SetActive(false);
        }
    }

    public void clickButtonStartPowerUpType(int type)
    {
        _target.GetComponent<PowerUp>().StartPowerUpType(type);
        Desactivebuttons();
        StopCoroutine("PrintScreenBonusBoxes");
    }

    public void clickOpenButtonDoor()
    {
        StopCoroutine("PrintDoor");
        DoorInfo doorInfos = new DoorInfo();
        doorInfos.doorId = _target.GetComponent<DoorHandler>().GetId();
        var opts = RaiseEventOptions.Default;
        //opts.CachingOption = ExitGames.Client.Photon.EventCaching.AddToRoomCache;
        opts.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(EventCode.Door, (object)doorInfos, true, opts);
        Desactivebuttons();
    }
}
