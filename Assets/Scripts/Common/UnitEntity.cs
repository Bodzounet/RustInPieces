using UnityEngine;
using System.Collections;

public enum e_UnitType
{
    NOT = 1,
    MELEE = 100,
    RANGE = 250,
    MAGIC = 300,
    MEDIC = 200,
    TANK = 1000,
    HERAULT = 450
}

public class UnitEntity : Entity 
{
    public UnitsInfo.e_UnitType type; // Raph, tu peux utiliser ça à la place du tiens s'teupl ?

    //public e_UnitType unitType;

    private RectTransform _healthBar;
    private RectTransform _manaBar;
	private GameObject _damageText;
	private Canvas _canvas;

    protected override void Start()
    {
        base.Start();
        RectTransform[] tr = GetComponentsInChildren<RectTransform>();
		
        foreach (RectTransform rekt in tr)
        {
            if (rekt.name.Equals("HealthBarMinion"))
                _healthBar = rekt;
			if (rekt.name.Equals("ManaBarMinion"))
			{
				_manaBar = rekt;
				rekt.parent.gameObject.SetActive(false);
			}
			if (rekt.name.Equals("CoolDownMinion"))
				rekt.parent.gameObject.SetActive(false);
        }
		MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in mr)
		{
			foreach (Material mat in m.materials)
			{
		
				if (mat.name == "Default-Material (Instance)")
				{
					mat.color = (e_Team)PhotonNetwork.player.customProperties["team"] == this.Team ? new Color(0.678f, 0.847f, 0.902f) : new Color(0.545f, 0, 0);
				}
				
			}
		}
		SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer m in smr)
		{
			foreach (Material mat in m.materials)
			{
				if ((e_Team)PhotonNetwork.player.customProperties["team"] == this.Team)
				{
					if (mat.name == "Metal_Plaques_minion (Instance)")
						mat.color = new Color(0.447f, 0.737f, 0.831f);
					if (mat.name == "Metal_Rouge (Instance)")
						mat.color = new Color(0.847f, 0.902f, 0.678f);
				}
				else
				{
					if (mat.name == "Metal_Plaques_minion (Instance)")
						mat.color = new Color(0.345f, 0, 0);
					if (mat.name == "Metal_Rouge (Instance)")
						mat.color = new Color(0.545f, 0, 0.502f);
				}
			}
		}
		_damageText = Resources.Load("UI/DamageDoneText") as GameObject;
		_canvas = GetComponentInChildren<Canvas>();
        addCallbackStat(Entity.e_StatType.HP_CURRENT, currentHealthUp);
        addCallbackStat(Entity.e_StatType.HP_MAX, maxHealthUp);
        addCallbackStat(Entity.e_StatType.MANA_CURRENT, currentManaUp);
        addCallbackStat(Entity.e_StatType.MANA_MAX, maxManaUp);
		OnHit += DamageShowOnHit;
    }

	void DamageShowOnHit(float before, float after, Entity.e_StatType stat)
	{
		if (stat != Entity.e_StatType.HP_CURRENT || (before - after < -getStat(Entity.e_StatType.REGEN_HP) - 1 || before - after <= 1))
			return;

		GameObject newText = Instantiate(_damageText, _canvas.transform) as GameObject;
		newText.GetComponent<DamageDone>().Damage((int)(before - after), (int)getStat(e_StatType.HP_MAX));
		newText.transform.localScale = Vector3.one;

	}

	void currentHealthUp(float bef, float aft)
    {
        _healthBar.sizeDelta = new Vector2(200 * (aft / getStat(Entity.e_StatType.HP_MAX)), _healthBar.sizeDelta.y);
        _healthBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.HP_CURRENT) < getStat(Entity.e_StatType.HP_MAX));
    }

    void maxHealthUp(float bef, float aft)
    {
        _healthBar.sizeDelta = new Vector2(200 * (getStat(Entity.e_StatType.HP_CURRENT) / aft), _healthBar.sizeDelta.y);
        _healthBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.HP_CURRENT) < getStat(Entity.e_StatType.HP_MAX));
    }

    void currentManaUp(float bef, float aft)
    {
        _manaBar.sizeDelta = new Vector2(200 * (aft / getStat(Entity.e_StatType.MANA_MAX)), _manaBar.sizeDelta.y);
        _manaBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.MANA_CURRENT) < getStat(Entity.e_StatType.MANA_MAX) && getStat(Entity.e_StatType.REGEN_MANA) > 0);
    }
    void maxManaUp(float bef, float aft)
    {
        _manaBar.sizeDelta = new Vector2(200 * (getStat(Entity.e_StatType.MANA_CURRENT) / aft), _manaBar.sizeDelta.y);
        _manaBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.MANA_CURRENT) < getStat(Entity.e_StatType.MANA_MAX) && getStat(Entity.e_StatType.REGEN_MANA) > 0);
    }
}
