using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int _numberOfSlots;
    List<Image> _itemImages;
    List<Image> _shopItemImages;
    List<IItem> _items;
    HeroEntity _heroEntity;
    HeroStats _heroStats;
    Hero.Shop _shop;
    PhotonView _pView;
    Text _gold;
    
    void Start()
    {
        _numberOfSlots = 7;
        _heroStats = this.GetComponent<HeroStats>();

        _heroEntity = GetComponent<HeroEntity>();

        _pView = GetComponentInParent<PhotonView>();
        if (_pView != null)
        {
            if (_pView.isMine)
            {
                _heroEntity.OnGoldChanged += UpdateGoldUI;
                _heroEntity.UnlockingLastSlot += UnlockLastSlot;

                _itemImages = new List<Image>();
                _items = new List<IItem>();
                GameObject inventory = GameObject.Find("Inventory");
                for (int i = 0; i < inventory.transform.childCount; i++)
                {
                    _items.Add(null);
                    _itemImages.Add(inventory.transform.GetChild(i).GetComponent<Image>());
                }
                _gold = inventory.GetComponentInChildren<Text>();
                StartCoroutine(PassiveGold());

                _shop = GameObject.Find("HeroUI").GetComponent<Hero.Shop>();

                _shop.Inventory = this;
                _shop.HeroController = this.GetComponent<HeroController>();
            }
        }
    }
    
    IEnumerator PassiveGold()
    {
        while (true)
        {
            _heroEntity.Gold += 1;
            yield return new WaitForSeconds(2);
        }
    }

    void UnlockLastSlot()
    {
        _itemImages[_itemImages.Count - 1].color = new Color(1, 1, 1, 0);
        _shopItemImages[_shopItemImages.Count - 1].color = new Color(1, 1, 1, 0);
    }

    void UpdateGoldUI(int value)
    {
        if (_gold != null)
        {
            _gold.text = value.ToString();
        }
    }

    public void AddItem(IItem item)
    {
        int firstSlotAvailable;

        if (_shopItemImages == null)
        {
            _shopItemImages = new List<Image>();
            GameObject shopInventory = GameObject.Find("PlayerItems");
            for (int i = 0; i < shopInventory.transform.childCount; i++)
            {
                _shopItemImages.Add(shopInventory.transform.GetChild(i).GetComponent<Image>());
            }
        }
        if (item.Cost < _heroEntity.Gold)
        {
            for (firstSlotAvailable = 0; firstSlotAvailable < _numberOfSlots && _items[firstSlotAvailable] != null; firstSlotAvailable++) ;
            if ((firstSlotAvailable < _numberOfSlots && _heroEntity.LastSlotUnlocked) || firstSlotAvailable < _numberOfSlots - 1)
            {
                _heroEntity.Gold -= item.Cost;
                _items[firstSlotAvailable] = item;
                _heroStats.AddItemStats(item);
                _itemImages[firstSlotAvailable].sprite = Resources.Load<Sprite>("ItemsIcons/" + item.Name);
                _itemImages[firstSlotAvailable].color = new Color(255, 255, 255, 255);
                _shopItemImages[firstSlotAvailable].sprite = Resources.Load<Sprite>("ItemsIcons/" + item.Name);
                _shopItemImages[firstSlotAvailable].color = new Color(255, 255, 255, 255);


                if (PhotonNetwork.connectionState == ConnectionState.Connected)
                {
                    _pView.RPC("givePhotonIItem", PhotonTargets.All, item.Id, _pView.viewID);
                }
            }
        }
    }

    public void RemoveItem(int itemIndex)
    {
        _heroEntity.Gold += _items[itemIndex].ResellCost;
        _heroStats.RemoveItemStats(_items[itemIndex]);
        _items[itemIndex] = null;
        _itemImages[itemIndex].sprite = null;
        _itemImages[itemIndex].color = new Color(255, 255, 255, 0);
        _shopItemImages[itemIndex].sprite = null;
        _shopItemImages[itemIndex].color = new Color(255, 255, 255, 0);

        if (PhotonNetwork.connectionState == ConnectionState.Connected)
        {
            _pView.RPC("removePhotonIItem", PhotonTargets.All, itemIndex, _pView.viewID);
        }
    }

    public  List<IItem> Items
    {
        get { return (_items); }
        set { _items = value; }
    }

    [PunRPC]
    public void givePhotonIItem(int info, int senderId)
    {
        if (_pView.viewID == senderId)
        {
            if (_pView.isMine == false)
            {
                int firstSlotAvailable;
                for (firstSlotAvailable = 0; firstSlotAvailable < _numberOfSlots && _items[firstSlotAvailable] != null; firstSlotAvailable++) ;
                if ((firstSlotAvailable < _numberOfSlots && _heroEntity.LastSlotUnlocked) || firstSlotAvailable < _numberOfSlots - 1)
                {
                    _items[firstSlotAvailable] = _shop.Items[info];
                    _heroStats.AddItemStats(_shop.Items[info]);
                    if (_shop.Items[info].Cost < _heroEntity.Gold)
                    {
                        _heroEntity.Gold -= _shop.Items[info].Cost;
                    }
                    else
                    {
                        print("Cheat item too expensive, vote ban");
                    }
                }
                else
                {
                    print("Cheat too many items, vote ban");
                }
            }
        }
    }


    [PunRPC]
    public void removePhotonIItem(int info, int senderId)
    {
        if (_pView.viewID == senderId)
        {
            if (_pView.isMine == false)
            {
                _heroStats.RemoveItemStats(_items[info]);
                _heroEntity.Gold += _items[info].ResellCost;
                _items[info] = null;
            }
        }
    }
}
