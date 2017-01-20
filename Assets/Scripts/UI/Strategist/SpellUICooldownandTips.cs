using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class SpellUICooldownandTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public GameObject graphicCD;
    private bool _showGraphic = true;

    public GameObject toolTip;

    private Spells.SpellLauncher _sl;
    private string _spellId;

    private Button btn;

    void Awake()
    {
        graphicCD = transform.FindChild("CD").gameObject;
        toolTip = transform.FindChild("Tips").gameObject;
        text = graphicCD.transform.GetChild(0).GetComponent<Text>();
        text.resizeTextForBestFit = true;
        text.color = Color.white;

        btn = GetComponent<Button>();
    }

    void Start()
    {
        _sl = GetComponentInParent<Spells.SpellLauncher>();
        _spellId = GetComponent<StrategistUI.SpellUI>().spellId;
    }

    void Update()
    {
        if (!_sl.IsSpellInCooldown(_spellId))
        {
            btn.interactable = false;
            if (_showGraphic)
            {
                graphicCD.SetActive(true);
                text.text = Mathf.CeilToInt(_sl.GetSpecificCooldown(_spellId).RemainingTime).ToString();
            }
            else
            {
                graphicCD.SetActive(false);
            }
        }
        else
        {
            graphicCD.SetActive(false);
            btn.interactable = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _showGraphic = false;
        toolTip.SetActive(true);
        var txt = toolTip.GetComponentInChildren<Text>();
        txt.resizeTextForBestFit = true;
        txt.color = Color.white;
        txt.text = _spellId + "\n CD :" + _sl.getSpellInfoFromId(_spellId).BaseCooldown.ToString() + "s";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _showGraphic = true;
        toolTip.SetActive(false);
    }
}
