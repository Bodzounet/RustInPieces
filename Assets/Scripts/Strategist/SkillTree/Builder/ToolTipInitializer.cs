using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace SkillTreeBuilder
{
    public class ToolTipInitializer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ToolTip _toolTip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _toolTip.ShowTip(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _toolTip.Hide();
        }

        void Start()
        {
            _toolTip = transform.root.GetComponentInChildren<ToolTip>();
        }
    }
}