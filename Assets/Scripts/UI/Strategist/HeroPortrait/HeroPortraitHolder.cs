using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class HeroPortraitHolder : MonoBehaviour, IPointerClickHandler
{
    public GameObject healthBar;

    private Image _heroImage;
    private HeroEntity _linkedHeroEntity = null;
    private RectTransform _healthRect;

    void Awake()
    {
        _heroImage = GetComponentInChildren<Image>();
        _healthRect = healthBar.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_linkedHeroEntity != null)
            _healthRect.sizeDelta = new Vector2(120 * (_linkedHeroEntity.getStat(Entity.e_StatType.HP_CURRENT) / _linkedHeroEntity.getStat(Entity.e_StatType.HP_MAX)), _healthRect.sizeDelta.y);
    }

    public HeroEntity GetHeroEntity()
    {
        return _linkedHeroEntity;
    }

    public void SetHeroEntity(HeroEntity ent)
    {
        _linkedHeroEntity = ent;
        //_heroImage = Factory.CreateResource<Image>(_linkedHeroEntity.EntityName + "Icon");
        _heroImage.sprite = Resources.Load("HeroIcons/" + _linkedHeroEntity.EntityName + "Icon", typeof(Sprite)) as Sprite;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.clickCount == 2)
            transform.root.GetComponentInChildren<StrategistUI.StrategistControls>().Focus(_linkedHeroEntity);
    }
}