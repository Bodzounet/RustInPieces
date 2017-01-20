using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(HeroController))]
public class PhotonNetHero : MonoBehaviour {



	//HeroController hero;
	PhotonView m_PhotonView;
	PhotonTransformView m_TransformView;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
    private Animator _animator;
	private Transform childModel;
	private Entity _entity;
	private BoxCollider _collider;
	private HeroSpellCasterManager _spellLauncher;
	private Vector3 repopPos;
    private Vector3 startScale;
	private ScoreBoard scoreboard = null;
	private bool dead = false;

	RectTransform UIHealthBarIG;
	RectTransform UIManaBarIG;
	Text UIPlayerName;
	// GameObject canvas;

	void Start()
	{
        PlayersInfos.Instance.heroList.Add(this.gameObject);
		m_PhotonView = GetComponent<PhotonView>();
		//hero = GetComponent<HeroController>();
		childModel = GetComponentInChildren<Animator>().transform;
        _animator = GetComponentInChildren<Animator>();

        correctPlayerPos = transform.position;
		correctPlayerRot = childModel.rotation;
		_collider = GetComponentInChildren<BoxCollider>();
		_spellLauncher = GetComponent<HeroSpellCasterManager>();
		_entity = GetComponent<Entity>();

		UIHealthBarIG = transform.Find("Canvas").Find("HealthBarContainer").Find("HealthBarIG").GetComponent<RectTransform>();
		UIManaBarIG = transform.Find("Canvas").Find("ManaBarContainer").Find("ManaBarIG").GetComponent<RectTransform>();
		UIPlayerName = transform.Find("Canvas").Find("PlayerName").GetComponent<Text>();
		UIPlayerName.text = m_PhotonView.owner.name;
		repopPos = transform.position;
        startScale = transform.localScale;
		if (GameObject.Find("HeroUI") != null)
			scoreboard = GameObject.Find("HeroUI").GetComponent<ScoreBoard>();
		if (m_PhotonView.isMine == true)
		{
			this.GetComponentInChildren<Camera>().enabled = true;
			this.GetComponentInChildren<AudioListener>().enabled = true;
			
            transform.Find("Canvas").gameObject.SetActive(false);
			m_PhotonView.RPC("Init", PhotonTargets.All, PhotonNetwork.player.customProperties["team"]);
		}
		else
		{
			this.GetComponentInChildren<Camera>().enabled = false;
			this.GetComponentInChildren<AudioListener>().enabled = false;
		}
		//if ((e_Team)PhotonNetwork.player.customProperties["team"] == _entity.Team)
		//	childModel.GetComponent<MeshRenderer>().material.color = Color.green;
		//else
		//	childModel.GetComponent<MeshRenderer>().material.color = Color.red;

	}


	[PunRPC]
	void Init(e_Team team)
	{
		GetComponent<Entity>().Team = team;
		_entity = GetComponent<Entity>();
		if (GameObject.Find("HeroUI") != null)
			scoreboard = GameObject.Find("HeroUI").GetComponent<ScoreBoard>();
		this.gameObject.name = "[" + team + "]" + _entity.EntityName;
		m_PhotonView = GetComponent<PhotonView>();
		if (scoreboard != null)
		{
			scoreboard.InitializeScoreBoard((int)team, m_PhotonView.viewID, _entity.EntityName + "Icon", m_PhotonView.owner.name);
			_entity.OnDeath += UpdateScoreOnDeath;
        }
	}

	void UpdateScoreOnDeath(GameObject thisentity)
	{
		if (scoreboard)
		{
			scoreboard.UpdateScoreBoard((int)_entity.Team, m_PhotonView.viewID, 0, 1);
			Entity killer = _entity.Hitter;
			if (killer is HeroEntity)
			{
				scoreboard.UpdateScoreBoard((int)killer.Team, killer.GetComponent<PhotonView>().viewID, 1);
            }
			foreach (Entity hitter in _entity.LastHitters.Keys)
			{
				if (hitter is HeroEntity && killer != hitter)
					scoreboard.UpdateScoreBoard((int)hitter.Team, hitter.GetComponent<PhotonView>().viewID, 0, 0, 1);
			}
		}
	}

	[PunRPC]
	void ShowModel(bool show)
	{
        if (_collider != null)
		_collider.enabled = show;
		if (show)
			_entity.setStat(Entity.e_StatType.HP_CURRENT, _entity.getStat(Entity.e_StatType.HP_MAX));
		childModel.gameObject.SetActive(show);
	}

	// Update is called once per frame
	void Update()
	{
		if (!m_PhotonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, 0.2f);
			childModel.rotation = Quaternion.Lerp(childModel.rotation, this.correctPlayerRot, 0.2f);

			float currentHealth, currentMana, maxHealth, maxMana;
			currentHealth = _entity.getStat(Entity.e_StatType.HP_CURRENT);
			currentMana = _entity.getStat(Entity.e_StatType.MANA_CURRENT);
			maxMana = _entity.getStat(Entity.e_StatType.MANA_MAX);
			maxHealth = _entity.getStat(Entity.e_StatType.HP_MAX);
			UIHealthBarIG.sizeDelta = new Vector2(200 * (currentHealth / maxHealth), UIHealthBarIG.sizeDelta.y);
			UIManaBarIG.sizeDelta = new Vector2(200 * (currentMana / maxMana), UIManaBarIG.sizeDelta.y);
			
		}
		else
		{
			if (_entity.getStat(Entity.e_StatType.HP_CURRENT) <= 0)
			{
				_entity.setStat(Entity.e_StatType.HP_CURRENT, -_entity.getStat(Entity.e_StatType.REGEN_HP) - 1);				
				_spellLauncher.enabled = false;				
				if (!dead)
				{
                    if (PhotonNetwork.connectionState == ConnectionState.Connected)
                    {
                        int id = this.GetComponent<PhotonView>().GetInstanceID();
                        PhotonNetwork.RaiseEvent(EventCode.HERO_DEATH, (object)id, true, RaiseEventOptions.Default);
                    }
                    m_PhotonView.RPC("ShowModel", PhotonTargets.All, false);
					Invoke("Repop", 8.0f);
					dead = true;
				}
            }
		}
	}

	void Repop()
	{
		transform.position = repopPos;		
		_spellLauncher.enabled = true;
		m_PhotonView.RPC("ShowModel", PhotonTargets.All, true);
		dead = false;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(childModel.rotation);
            stream.SendNext(_animator.GetBool("walk"));
			stream.SendNext(_entity.getStat(Entity.e_StatType.HP_CURRENT));
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            _animator.SetBool("walk", (bool)stream.ReceiveNext());
			_entity.setStat(Entity.e_StatType.HP_CURRENT, (float)stream.ReceiveNext());
			//Debug.Log("Recoit " + gameObject.name);
		}
	}


}
