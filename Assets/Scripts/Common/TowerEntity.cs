using UnityEngine;
using System.Collections;

public class TowerEntity : Entity
{

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
				_manaBar = rekt;
			if (rekt.name.Equals("CoolDownMinion"))
				rekt.parent.gameObject.SetActive(false);
		}

		MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in mr)
		{
			foreach (Material mat in m.materials)
			{
				if (mat.name == "turret (Instance)")
				{
					mat.color = (e_Team)PhotonNetwork.player.customProperties["team"] == this.Team ? new Color(0.678f, 0.847f, 0.902f) : new Color(0.545f, 0, 0);
				}
				if ((e_Team)PhotonNetwork.player.customProperties["team"] == this.Team)
				{
					if (mat.name == "turret1 (Instance)")
						mat.color = new Color(0.447f, 0.737f, 0.831f);
					if (mat.name == "blah (Instance)")
						mat.color = new Color(0.847f, 0.902f, 0.678f);
                }
				else
				{
					if (mat.name == "turret1 (Instance)")
						mat.color = new Color(0.345f, 0, 0);
					if (mat.name == "blah (Instance)")
						mat.color = new Color(0.545f, 0, 0.502f);
				}
            }
		}
		_damageText = Resources.Load("UI/DamageDoneText") as GameObject;
		_canvas = GetComponentInChildren<Canvas>();
		addCallbackStat(Entity.e_StatType.HP_CURRENT, currentHealthUp);
		addCallbackStat(Entity.e_StatType.HP_MAX, maxHealthUp);
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
		if (_healthBar != null)
		{
			_healthBar.sizeDelta = new Vector2(200 * (aft / getStat(Entity.e_StatType.HP_MAX)), _healthBar.sizeDelta.y);
			_healthBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.HP_CURRENT) < getStat(Entity.e_StatType.HP_MAX));
		}
	}

	void maxHealthUp(float bef, float aft)
	{
		_healthBar.sizeDelta = new Vector2(200 * (getStat(Entity.e_StatType.HP_CURRENT) / aft), _healthBar.sizeDelta.y);
		_healthBar.parent.gameObject.SetActive(getStat(Entity.e_StatType.HP_CURRENT) < getStat(Entity.e_StatType.HP_MAX));
	}
}
