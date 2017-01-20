using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ToolTipStrat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text textBoxTips;
    private GameObject panelTip;
    private Animator anim;

    private int id;

    void Start()
    {
        id = GetComponent<SkillTree.TreeNode>().UniqueID;

        panelTip = this.transform.root.Find("UI").Find("SkillTree").Find("ToolTipPanel").gameObject;
        textBoxTips = panelTip.transform.GetChild(0).GetComponent<Text>();
        anim = panelTip.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textBoxTips.text = OpCode_SkillNodes.opCodeToNode[id].prefabName + "\n\n" + OpCode_SkillNodes.opCodeToNode[id].description;
        anim.Play("FadeIn");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //textBoxTips.text = "";
        anim.Play("FadeOut");
    }

}
