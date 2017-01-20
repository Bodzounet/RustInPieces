using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeroStats : MonoBehaviour
{
    Entity _entity;

    List<Text> _UIStatsText;
    List<Entity.e_StatType> _statTypes;

    Text _UIHealth;
    Text _UIMana;
    RectTransform _UIHealthBar;
    RectTransform _UIManaBar;
    RectTransform _UIHealthBarIG;
    RectTransform _UIManaBarIG;
	Image _Hitmarker;
	Image _UIDieMessage;
	GameObject _ComboText;
	HeroFeedbackOnScreen _feedback;
    PhotonView _pView;
	int comboNumber = 0;
	int totalDamage = 0;
	float _comboTimer = 0;
	private GameObject _damageText;
	private Canvas _canvas;
	bool _isMine;

	public float deadTimer = 0;

    void Start()
    {
        _entity = GetComponent<HeroEntity>();
        _pView = GetComponent<PhotonView>();
		_damageText = Resources.Load("UI/DamageDoneText") as GameObject;
		_canvas = GetComponentInChildren<Canvas>();
		_entity.OnHit += DamageShowOnHit;

		_isMine = false; 
        if (_pView != null)
        {
            _isMine = _pView.isMine;
        }
        if (_isMine)
        {
			_feedback = GetComponentInChildren<Camera>().gameObject.AddComponent<HeroFeedbackOnScreen>();
			/*GameObject.Find("HeroUI").GetComponent<Canvas>().worldCamera = GetComponentInChildren<Camera>();
			GameObject.Find("HeroUI").GetComponent<Canvas>().planeDistance = 1;
			GameObject.Find("HeroUI").GetComponent<Canvas>().sortingLayerName = "UI";*/
			_entity.OnHit += FeedbackOnHit;
			_entity.OnDoDamage += FeedbackMLG;
			//Get the UI text for the different stats
			InitializeStats();

            _UIHealth = GameObject.Find("HealthValue").GetComponent<Text>();
            _UIMana = GameObject.Find("ManaValue").GetComponent<Text>();
            _UIHealthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
            _UIManaBar = GameObject.Find("ManaBar").GetComponent<RectTransform>();
            _UIDieMessage = GameObject.Find("DieMessage").GetComponent<Image>();
			_Hitmarker = GameObject.Find("Hitmarker").GetComponent<Image>();
			_ComboText = GameObject.Find("ComboText");
			//UIHealthBarIG = transform.Find("Canvas").Find("HealthBarContainer").Find("HealthBarIG").GetComponent<RectTransform>();
			//UIManaBarIG = transform.Find("Canvas").Find("ManaBarContainer").Find("ManaBarIG").GetComponent<RectTransform>();
		}
    }

	Color changeColor(Color color, Color newColor)
	{
		color.r = newColor.r == 0 ? color.r : newColor.r;
		color.g = newColor.g == 0 ? color.g : newColor.g;
		color.b = newColor.b == 0 ? color.b : newColor.b;
		color.a = newColor.a == 0 ? color.a : newColor.a;
		return color;
	}

	void FeedbackMLG(Entity other, float damagedone)
	{
		_Hitmarker.color = new Color(1, 1, 1, 1);
		if (!_Hitmarker.GetComponent<AudioSource>().isPlaying)
		_Hitmarker.GetComponent<AudioSource>().Play();
		comboNumber++;
		totalDamage += (int)damagedone;
		_comboTimer = 1.2f;
        if (comboNumber > 1)
		{
			/*_ComboText.GetComponentInChildren<ParticleSystem>().loop = true;
			if (!_ComboText.GetComponentInChildren<ParticleSystem>().isPlaying)
				_ComboText.GetComponentInChildren<ParticleSystem>().Play();*/
			_ComboText.GetComponent<Text>().color = changeColor(_ComboText.GetComponent<Text>().color, new Color(0, 0, 0, 1));
			_ComboText.transform.GetChild(0).GetComponent<Text>().color = changeColor(_ComboText.transform.GetChild(0).GetComponent<Text>().color, new Color(0, 0, 0, 1));
			_ComboText.transform.GetChild(1).GetComponent<Text>().color = changeColor(_ComboText.transform.GetChild(1).GetComponent<Text>().color, new Color(0, 0, 0, 1));
			_ComboText.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = changeColor(_ComboText.transform.GetChild(1).GetChild(0).GetComponent<Text>().color, new Color(0, 0, 0, 1)); //MLG Code
			_ComboText.transform.GetChild(0).GetComponent<Text>().text = ""+comboNumber + " !";
			for (int i = 0; i < comboNumber / 2; i++)
				_ComboText.transform.GetChild(0).GetComponent<Text>().text += "!";
			_ComboText.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + totalDamage ;
		}
    }

    void FeedbackOnHit(float before, float after, Entity.e_StatType stat)
	{
		if (stat != Entity.e_StatType.HP_CURRENT || before < after)
			return;
		float critDamage = this._entity.getStat(Entity.e_StatType.HP_MAX) / 4;

		_feedback.intensity += (before - after) / critDamage;
	}

	void DamageShowOnHit(float before, float after, Entity.e_StatType stat)
	{
		if (stat != Entity.e_StatType.HP_CURRENT || (before - after < -_entity.getStat(Entity.e_StatType.REGEN_HP) - 1 || before - after <= 1))
			return;

		GameObject newText = Instantiate(_damageText, _canvas.transform) as GameObject;
		newText.GetComponent<DamageDone>().Damage((int)(before - after), (int)_entity.getStat(Entity.e_StatType.HP_MAX));
		newText.transform.localScale = Vector3.one;

	}

	void InitializeStats()
    {
        Transform UIStatsParent = GameObject.Find("HeroStats").transform;
        _statTypes = new List<Entity.e_StatType>();
        _statTypes.Add(Entity.e_StatType.RANGE_ATT);
        _statTypes.Add(Entity.e_StatType.MELEE_ATT);
        _statTypes.Add(Entity.e_StatType.MAGIC_ATT);
        _statTypes.Add(Entity.e_StatType.DEFENSE);
        _statTypes.Add(Entity.e_StatType.ATTACK_SPEED);
        _statTypes.Add(Entity.e_StatType.FOCUS);
        _statTypes.Add(Entity.e_StatType.LIFE_DRAIN);

        _UIStatsText = new List<Text>();
        for (int i = 0; i < UIStatsParent.childCount; i++)
        {
            _UIStatsText.Add(UIStatsParent.GetChild(i).GetChild(0).GetComponent<Text>());
        }
    }

    void Update()
    {
        if (_isMine)
        {
            float currentHealth, currentMana, maxHealth, maxMana;

            for (int i = 0; i < _UIStatsText.Count; i++)
            {
                _UIStatsText[i].text = _entity.getStat(_statTypes[i]).ToString();
            }
            currentHealth = _entity.getStat(Entity.e_StatType.HP_CURRENT);
            currentMana = _entity.getStat(Entity.e_StatType.MANA_CURRENT);
            maxMana = _entity.getStat(Entity.e_StatType.MANA_MAX);
            maxHealth = _entity.getStat(Entity.e_StatType.HP_MAX);

            if (currentHealth <= 0)
            {
                if (deadTimer < 4.0f)
                {
                    _UIDieMessage.color = new Color(1, 1, 1, deadTimer / 4.0f);
                }
                else
                {
                    _UIDieMessage.color = new Color(1, 1, 1, 1 - ((deadTimer - 4.0f) / 4.0f));
                }
                deadTimer += Time.deltaTime;
            }
            else
            {
                _UIDieMessage.color = new Color(1, 1, 1, 0);
                deadTimer = 0;
            }
            _UIHealth.text = (int)currentHealth + " / " + (int)maxHealth;
            _UIHealthBar.sizeDelta = new Vector2(200 * (currentHealth / maxHealth), _UIHealthBar.sizeDelta.y);
            _UIMana.text = (int)currentMana + " / " + (int)maxMana;
            _UIManaBar.sizeDelta = new Vector2(200 * (currentMana / maxMana), _UIManaBar.sizeDelta.y);
			if (_feedback.intensity > 0)
			{
				_feedback.intensity -= Time.deltaTime / 2;
				if (_feedback.intensity < 0 || currentHealth <= 0)
					_feedback.intensity = 0;
			}
			if (_Hitmarker.color.a > 0)
			{
				_Hitmarker.color -= new Color(0, 0, 0, Time.deltaTime);
			}

			if (_comboTimer > 0)
				_comboTimer -= Time.deltaTime;

			if (_ComboText.transform.GetChild(1).GetChild(0).GetComponent<Text>().color.a > 0 && _comboTimer <= 0)
			{
				comboNumber = 0;
				totalDamage = 0;
				/*_ComboText.GetComponentInChildren<ParticleSystem>().loop = false;
				_ComboText.GetComponentInChildren<ParticleSystem>().Stop();*/
				_comboTimer = 0;
				_ComboText.GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
				_ComboText.transform.GetChild(0).GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
				if (_ComboText.GetComponent<Text>().color.a < 0.2f)
				{
					_ComboText.transform.GetChild(1).GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
					_ComboText.transform.GetChild(1).GetChild(0).GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
				}
			}

			//UIHealthBarIG.sizeDelta = new Vector2(200 * (currentHealth / maxHealth), UIHealthBarIG.sizeDelta.y);
			//UIManaBarIG.sizeDelta = new Vector2(200 * (currentMana / maxMana), UIManaBarIG.sizeDelta.y);
		}
	}

    public void AddItemStats(IItem item)
    {
        foreach (KeyValuePair<Entity.e_StatType, float> stat in item.StatBoosts)
        {
            _entity.modifyStat(stat.Key, Entity.e_StatOperator.ADD, stat.Value, _entity);
        }
    }

    public void RemoveItemStats(IItem item)
    {
        foreach (KeyValuePair<Entity.e_StatType, float> stat in item.StatBoosts)
        {
            _entity.modifyStat(stat.Key, Entity.e_StatOperator.SUBTRACT, stat.Value, _entity);
        }
    }
}