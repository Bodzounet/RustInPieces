using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UI_MinionManager
{
    public class UnitSlotMouse : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnLeftClick;
        public UnityEvent OnRightClick;

        public UnityEvent OnMouseEnter;
        public UnityEvent OnMouseExit;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit.Invoke();
        }
    }
}