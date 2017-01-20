using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageDone : MonoBehaviour {


	private int _damage;
	private Color _color;
	private int _fontSize;
	private Text _text;
	private bool _changed = false;
	private float speed = 1;

	public void Text(string Text, Color color)
	{
		_text = GetComponentInChildren<Text>();
		_text.text = (string)Text.Clone();
		_text.color = color;
		_text.fontSize = 30;
		Invoke("Die", 1.0f);
		speed = 0.5f;
	}

	public void Damage (int value, int maxHp)
	{
		_text = GetComponentInChildren<Text>();
		_damage = value;
		_color = value >= 0 ? Color.red : Color.green;
		_fontSize = 80 + (200 * Mathf.Abs(value) / maxHp + 1);
		_changed = true;
		Invoke("Die", 1.0f);
		this.transform.localPosition = new Vector2(200, -300);
	}


	// Use this for initialization
	void Start ()
	{
		_text = GetComponentInChildren<Text>();
	}
	
	void Die()
	{
		Destroy(this.gameObject);
	}
	// Update is called once per frame
	void Update ()
	{
		this.transform.position += new Vector3(Time.deltaTime * 2, Time.deltaTime * 5) * speed;
		speed += Time.deltaTime;
		if (!_changed)
			return;
		_text.text = _damage > 0 ? "-" : "+";
		_text.text += _damage;
		_text.color = _color;
		_text.fontSize = _fontSize;
		_changed = false;
	}
}
