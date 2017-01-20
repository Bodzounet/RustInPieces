using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Hero
{
    public class Shop : MonoBehaviour
    {
        GameObject _shop;
        GameObject _content;
        Transform _selectedItem;
        Transform _buyButton;
        Transform _sellButton;
        Inventory _inventory;

        HeroController _heroController;

        List<IItem> _items;
        int _selectedItemIndex;

        void Start()
        {
            _shop = transform.Find("Shop").gameObject;
            _content = GameObject.Find("ShopContent");
            _selectedItem = _shop.transform.Find("SelectedItem");
            _buyButton = _selectedItem.Find("Buy");
            _sellButton = _selectedItem.Find("Sell");
            _selectedItem.gameObject.SetActive(false);

            _items = new List<IItem>();
            _items.Add(new Sword());
            _items.Add(new Bow());
            _items.Add(new Staff());
            _items.Add(new Shield());
            _items.Add(new Cleaver());
            _items.Add(new Dagger());
            _items.Add(new VampiricJewel());
            _items.Add(new LifeCrystal());
            _items.Add(new ManaCrystal());
            //AddItemsToShop();
            _shop.SetActive(false);

            _selectedItemIndex = -1;
        }

        void AddItemsToShop()
        {
            int i = 0;

            foreach (IItem item in _items)
            {
                int itemIndex = i;
                item.Id = i;

                GameObject UIItem = Instantiate(Resources.Load<GameObject>("ShopUI/ItemInShop"));
                RectTransform itemTransform = UIItem.GetComponent<RectTransform>();
				if (itemTransform != null)
				{
					itemTransform.SetParent(_content.transform);
					itemTransform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemsIcons/" + item.Name);
					itemTransform.Find("Description").GetComponent<Text>().text = item.Description;
					itemTransform.Find("Price").GetComponent<Text>().text = item.Cost + " Gold.";
					itemTransform.GetComponent<Button>().onClick.AddListener(() => { ClickedItem(itemIndex); } );
					itemTransform.localPosition = new Vector3(10, -10 + i * -40, 0);
				}
                i++;
            }
        }

        void Update()
        {
            if (Input.GetButtonUp("Shop"))
            {
                _shop.SetActive(!_shop.GetActive());
                _heroController.IsFrozen = _shop.GetActive();
            }
        }

        public void ClickedItem(int itemIndex)
        {
            _selectedItem.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(true);
            _sellButton.gameObject.SetActive(false);
            _selectedItemIndex = itemIndex;
            DisplayItem(_items[itemIndex], true);
        }

        public void ClickedInventoryItem(int itemIndex)
        {
            if (_inventory.Items[itemIndex] != null)
            {
                _buyButton.gameObject.SetActive(false);
                _sellButton.gameObject.SetActive(true);
                _selectedItemIndex = itemIndex;
                DisplayItem(_inventory.Items[itemIndex], false);
            }
        }

        public void DisplayItem(IItem item, bool buy)
        {
            _selectedItem.transform.Find("Name").GetComponent<Text>().text = item.Name;
            _selectedItem.transform.Find("Description").GetComponent<Text>().text = item.Description;
            _selectedItem.transform.Find("Price").GetComponent<Text>().text = (buy ? item.Cost : item.ResellCost) + " Gold.";
            _selectedItem.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemsIcons/" + item.Name);
        }

        public void BuyItem()
        {
            _inventory.AddItem(_items[_selectedItemIndex]);
        }

        public void SellItem()
        {
            _inventory.RemoveItem(_selectedItemIndex);
        }

        public Inventory Inventory
        {
            get { return (_inventory); }
            set { _inventory = value; }
        }

        public HeroController HeroController
        {
            get { return (_heroController); }
            set { _heroController = value; }
        }

        public List<IItem> Items
        {
            get { return (_items); }
        }
    }
}
