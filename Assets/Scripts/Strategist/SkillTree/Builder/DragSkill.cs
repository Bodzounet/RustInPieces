using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;

namespace SkillTreeBuilder
{
    public class DragSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform dragParent;
        public RectTransform treeParent;

        public RectTransform placeHolder;

        private CanvasGroup _canvasGroup;

        private float delta;

        void Start()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();

            try
            {
                delta = this.GetComponentInParent<HorizontalLayoutGroup>().spacing / 2 + this.GetComponent<LayoutElement>().minWidth / 2;
            }
            catch
            {
                //osef
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (transform.parent != treeParent)
            {
                eventData.pointerDrag = null;
                return;
            }

            placeHolder.gameObject.SetActive(true);
            placeHolder.SetSiblingIndex(this.transform.GetSiblingIndex());
            this.transform.SetParent(dragParent);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            this.transform.position = eventData.position;

            for (int i = 0; i < treeParent.childCount; i++)
            {
                if (this.transform.position.x < treeParent.GetChild(i).transform.position.x + delta)
                {
                    placeHolder.SetSiblingIndex(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Unity says it is always called after OnDrop, if the drop is done on a GO having a OnDrop Function
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            this.transform.SetParent(treeParent);
            this.transform.SetSiblingIndex(placeHolder.GetSiblingIndex());

            placeHolder.gameObject.SetActive(false);

            _canvasGroup.blocksRaycasts = true;
        }
    }
}