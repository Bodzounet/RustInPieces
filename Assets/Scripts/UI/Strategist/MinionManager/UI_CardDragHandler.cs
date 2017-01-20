using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace UI_MinionManager
{
    public class UI_CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Transform DraggedImageParent;
        public Camera cam;

        public GameObject _visualImg;

        public UI_Card associatedCard;

        private bool _isDragging = false;
        public bool IsDragging
        {
            get
            {
                return _isDragging;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (associatedCard.Stock <= 0)
                return;

            _visualImg.SetActive(true);
            _visualImg.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;

            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_visualImg.activeSelf)
            {
                Vector3 pos = eventData.position;
                //RectTransformUtility.ScreenPointToWorldPointInRectangle(DraggedImageParent as RectTransform, eventData.position, cam, out pos);

                _visualImg.transform.position = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _visualImg.SetActive(false);
            _isDragging = false;
        }
    }
}