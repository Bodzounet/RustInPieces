using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpellsUI : MonoBehaviour
{
    Spells.SpellLauncher _spellLauncher;
    List<GameObject> _cooldowns;

    void Start()
    {
        InitSpellUI();
    }

    void InitSpellUI()
    {
        Transform spellUI;
        _spellLauncher = Camera.main.GetComponentInParent<Spells.SpellLauncher>();
        _cooldowns = new List<GameObject>();
        _spellLauncher.LaunchCallback += StartCooldownUI;

        for (int i = 1; i < 6; i++)
        {
            spellUI = transform.GetChild(i - 1);
            SetTooltip(spellUI.Find("Tooltip"), _spellLauncher.AvailableSpells[i]);
            spellUI.Find("Icon").GetComponent<Image>().sprite = _spellLauncher.AvailableSpells[i].Icon;
            _cooldowns.Add(spellUI.FindChild("Cooldown").gameObject);
            spellUI.FindChild("Cooldown").gameObject.SetActive(false);
        }
    }

    void SetTooltip(Transform toolTip, Spells.SpellInfo spellInfo)
    {
        SetChildText(toolTip, "Name", spellInfo.SpellName);
        SetChildText(toolTip, "Desc", spellInfo.SpellDesc);
        SetChildText(toolTip, "ManaCost", spellInfo.ManaCost);
        SetChildText(toolTip, "Cooldown", "Cooldown : " + spellInfo.Cooldown + "s");
        toolTip.gameObject.SetActive(false);

    }

    void SetChildText(Transform parent, string childName, string text)
    {
        parent.Find(childName).GetComponent<Text>().text = text;
    }

    void StartCooldownUI(int spellIndex)
    {
        if (spellIndex > 0)
        {
            _cooldowns[spellIndex - 1].SetActive(true);
            _cooldowns[spellIndex - 1].transform.GetChild(0).GetComponent<Text>().text = _spellLauncher.AvailableSpells[spellIndex].Cooldown + "";
            StartCoroutine(HandleCooldownUI(spellIndex));
        }
    }

    IEnumerator HandleCooldownUI(int spellIndex)
    {
        Text text = _cooldowns[spellIndex - 1].transform.GetChild(0).GetComponent<Text>();
        Spells.CooldownManager cooldownManager = _spellLauncher.GetSpecificCooldown(_spellLauncher.GetSpellIDByIndex(spellIndex));
        float secondsLeft = cooldownManager.RemainingTime;

        while (secondsLeft > 0.5)
        {
            yield return new WaitForEndOfFrame();
            secondsLeft = cooldownManager.RemainingTime;
            text.text = Mathf.Ceil(secondsLeft) + "";
        }
        _cooldowns[spellIndex - 1].SetActive(false);
    }
}
