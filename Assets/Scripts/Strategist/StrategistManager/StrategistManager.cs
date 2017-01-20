using UnityEngine;
using System.Collections;
using System.Linq;

// exactly 2 instances of this script will be needed, so even if it would made a perfect singleton, it's no more possible
// place each script on a DIFFERENT gameobject.
[RequireComponent(typeof(MinionManager))]
[RequireComponent(typeof(Spells.SpellLauncher))]
[RequireComponent(typeof(CurrenciesManager))]
public class StrategistManager : MonoBehaviour
{
	public Void_D_IntInt myBoosterClick;
	[SerializeField]
    private e_Team team;
    public e_Team Team
    {
        get { return team; }
    }

    [HideInInspector] public CurrenciesManager currenciesManager;
    [HideInInspector] public MinionManager minionManager;
    [HideInInspector] public Spells.SpellLauncher spellLauncher;
    [HideInInspector] public Core_MinionManager.MinionPanelManager minionPanelManager;
    [HideInInspector] public Core_MinionManager.HeadQuarter hq;

    private PhotonView _photonView;
    public PhotonView GetStrategistPhotonView
    {
        get { return _photonView; }
    }

    void Awake()
    {
        PlayersInfos.Instance.strategistList.Add(this.gameObject);
        _photonView = this.GetComponent<PhotonView>();
        if (_photonView != null)
        {
            team = (e_Team)GetComponent<PhotonView>().owner.customProperties["team"];
        }

        currenciesManager = GetComponent<CurrenciesManager>();
        minionManager = GetComponent<MinionManager>();
        spellLauncher = GetComponent<Spells.SpellLauncher>();
        minionPanelManager = this.GetComponentInChildren<Core_MinionManager.MinionPanelManager>(true);
        hq = GameObject.FindObjectsOfType<Core_MinionManager.HeadQuarter>().Single(x => x.team == team);
    }

	void Start()
	{
        if (_photonView.isMine)
        {
            _photonView.RPC("Init", PhotonTargets.All, PhotonNetwork.player.customProperties["team"]);
			GetComponentInChildren<AudioListener>().enabled = true;
		}
		else
		{
			GetComponentInChildren<Canvas>().enabled = false;
			GetComponentInChildren<Camera>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;

		}
	}

	[PunRPC]
	void Init(e_Team tem)
	{
		this.team = tem;
		this.gameObject.name = "Strategist" + team;
	}
}
