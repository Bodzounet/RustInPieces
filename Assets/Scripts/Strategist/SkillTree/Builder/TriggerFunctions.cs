using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

namespace SkillTreeBuilder
{
    public class TriggerFunctions : MonoBehaviour
    {
        public RectTransform poolParent;
        public RectTransform treeParent;

        private BuildingRules _br;

        private PopUp _popUp;

        private int _indexInHierarchy;

        private bool _isEliteSkill;

        void Start()
        {
            _br = GetComponentInParent<BuildingRules>();
            _indexInHierarchy = transform.GetSiblingIndex();

             _isEliteSkill = GetComponentInParent<PoolSkillHolder>().rank == 3;

            _popUp = GetComponentInParent<SkillTreeBuilderManager>()._popup;
        }

        void _OnDoubleClick()
        {
            if (transform.parent == poolParent)
            {
                if (_br.canPickSkill(_isEliteSkill))
                {
                    transform.SetParent(treeParent);
                    _br.PickSill(_isEliteSkill);
                }
                else
                {
                    _popUp.SpawnPopUp("Your tree can't have more than 1 Elite skill and 10 Regular skills");
                }
            }
        }

        void _OnRightClick()
        {
            if (transform.parent == treeParent)
            {
                transform.SetParent(poolParent);
                List<Transform> children = new List<Transform>();
                for (int i = 0; i < poolParent.childCount; i++)
                {
                    children.Add(poolParent.GetChild(i));
                }   

                foreach (var v in children.OrderBy(x => x.GetComponent<TriggerFunctions>()._indexInHierarchy))
                {
                    v.SetAsLastSibling();
                }

                _br.DropSkill(_isEliteSkill);
            }
        }

        public void OnPointerClick(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData)data;
            if (ped.button == PointerEventData.InputButton.Right)
            {
                _OnRightClick();
            }
            else if (ped.clickCount == 2)
            {
                _OnDoubleClick();
            }
        }
    }
}