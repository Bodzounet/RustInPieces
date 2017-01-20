using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


public class SpellBarUIManager : MonoBehaviour
{
    Dictionary<string, GameObject> _dicPrefabs;
    Dictionary<KeyCode, int> _dicKeysSpellFirst;
    Dictionary<KeyCode, int> _dicKeysSpellSecond;
    void Start()
    {
        _dicPrefabs = new Dictionary<string, GameObject>();
        _dicKeysSpellFirst = new Dictionary<KeyCode, int>();
        _dicKeysSpellFirst.Add(KeyCode.Alpha1, 0);
        _dicKeysSpellFirst.Add(KeyCode.Alpha2, 1);
        _dicKeysSpellFirst.Add(KeyCode.Alpha3, 2);
        _dicKeysSpellFirst.Add(KeyCode.Alpha4, 3);
        _dicKeysSpellFirst.Add(KeyCode.Alpha5, 4);
        _dicKeysSpellFirst.Add(KeyCode.Alpha6, 5);
        _dicKeysSpellFirst.Add(KeyCode.Alpha7, 6);
        _dicKeysSpellSecond = new Dictionary<KeyCode, int>();
        _dicKeysSpellSecond.Add(KeyCode.Alpha1, 7);
        _dicKeysSpellSecond.Add(KeyCode.Alpha2, 8);
        _dicKeysSpellSecond.Add(KeyCode.Alpha3, 9);
        _dicKeysSpellSecond.Add(KeyCode.Alpha4, 10);
        _dicKeysSpellSecond.Add(KeyCode.Alpha5, 11);
        _dicKeysSpellSecond.Add(KeyCode.Alpha6, 12);
        _dicKeysSpellSecond.Add(KeyCode.Alpha7, 13);
    }

    public void AddSpell(string name)
    {
        GameObject itemUI = Factory.CreateInstanceOf(name);
        itemUI.transform.SetParent(this.transform);
        _dicPrefabs.Add(name, itemUI);
    }

    public void RemoveSpell(string name)
    {
        Destroy(_dicPrefabs[name]);
        _dicPrefabs.Remove(name);
    }

    KeyCode key;
    Dictionary<KeyCode, int> _dictmp;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            _dictmp = _dicKeysSpellSecond;
        else
            _dictmp = _dicKeysSpellFirst;
        foreach (var item in _dicKeysSpellFirst)
        {
            int cpt = 0;
            if (Input.GetKeyDown(item.Key))
            {
                    for (int i = 0; i < this.transform.childCount; i++)
                    {
                        if (this.transform.GetChild(i).gameObject.activeSelf)
                        {
                            if (cpt == item.Value)
                            {
                                this.transform.GetChild(i).gameObject.GetComponent<StrategistUI.SpellUI>().OnLeftClicked();
                                break;
                            }
                            else
                                cpt++;
                        }
                    }
            }
        }
    }

}
