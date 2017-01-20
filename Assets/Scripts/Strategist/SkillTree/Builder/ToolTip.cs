using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace SkillTreeBuilder
{
    public class ToolTip : MonoBehaviour
    {
        private Text _msg;
        private CanvasGroup _cg;

        void Awake()
        {
            _msg = this.GetComponentInChildren<Text>();
            _cg = this.GetComponent<CanvasGroup>();
        }

        public void ShowTip(BaseEventData bed)
        {
            PointerEventData ped = (PointerEventData)bed;

            (transform as RectTransform).position = (ped.pointerEnter.transform as RectTransform).position;

            _cg.alpha = 1;
            _msg.text = OpCode_SkillNodes.opCodeToNode[ped.pointerEnter.GetComponent<NodeData>().nodeId].prefabName.ToString() + "\n\n" +
                OpCode_SkillNodes.opCodeToNode[ped.pointerEnter.GetComponent<NodeData>().nodeId].description;
        }

        public void Hide()
        {
            _cg.alpha = 0;
        }
    }
}