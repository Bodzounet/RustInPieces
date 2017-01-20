using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeroPortraitManager : MonoBehaviour
{
    public RectTransform portraitsParent;
    public GameObject portraitPrefab;

    private List<GameObject> _portraitList = new List<GameObject>();
    private List<GameObject> _heroList;

    void Start()
    {
        Invoke("updateHeroList", 0.8f);
    }

    void updateHeroList()
    {
        _heroList = PlayersInfos.Instance.GetHeroesOfTeam(transform.root.GetComponent<StrategistManager>().Team);
        
        foreach (GameObject hero in _heroList)
        {
            GameObject portrait = GameObject.Instantiate(portraitPrefab);
            portrait.transform.SetParent(portraitsParent.transform);
            portrait.GetComponent<HeroPortraitHolder>().SetHeroEntity(hero.GetComponent<HeroEntity>());
            _portraitList.Add(portraitPrefab);
        }
    }
}
