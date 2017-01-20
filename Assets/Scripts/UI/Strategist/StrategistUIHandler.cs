using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrategistUIHandler : MonoBehaviour
{
    public CanvasGroup SkilltreeMenu;
    public CanvasGroup MinionMenu;
    public Text textGold;
    public Text textGear;

    private UIQuitGame _uiQuitGame;

    void Awake()
    {
        this.transform.root.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gold].OnCurrencyAmountModification += updateUiGold;
        this.transform.root.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gears].OnCurrencyAmountModification += updateUiGear;

        _uiQuitGame = GameObject.FindObjectOfType<UIQuitGame>();
    }

    void Update()
    {
        if (Input.GetButton("Menu"))
        {
            if (SkilltreeMenu.interactable)
                SetInactive(SkilltreeMenu);
            else if (MinionMenu.interactable)
            {
                MinionMenu.SendMessage("OnSetInactive");
                SetInactive(MinionMenu);
            }
            _uiQuitGame.CanUseInput = true;
        }
    }

    public void OpenSkilltree()
    {
        if (SkilltreeMenu.interactable)
            SetInactive(SkilltreeMenu);
        else
            SetActive(SkilltreeMenu);
        SetInactive(MinionMenu);
        MinionMenu.SendMessage("OnSetInactive");
    }

    public void OpenMinion()
    {
        if (MinionMenu.interactable)
        {
            MinionMenu.SendMessage("OnSetInactive");
            SetInactive(MinionMenu);
        }
        else
            SetActive(MinionMenu);
        SetInactive(SkilltreeMenu);
    }

    private void SetInactive(CanvasGroup cg)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0;
    }

    private void SetActive(CanvasGroup cg)
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.alpha = 1;

        _uiQuitGame.CanUseInput = false;
    }

    void updateUiGold(int before, int after)
    {
        textGold.text = after.ToString();
    }

    void updateUiGear(int before, int after)
    {
        textGear.text = after.ToString();
    }
}
