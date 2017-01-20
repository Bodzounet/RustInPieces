using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillTree
{
    public class UI_TreeNode : MonoBehaviour
    {
        private TreeNode _node;
        private TreeManager _manager;

        public Color LockedNode;
        public Color AvailableNode;
        public Color PartiallyUnlockedNode;
        public Color FullyUnlockedNode;
        private Image _img;

        private Text _text;

        void Awake()
        {
            _node = this.GetComponent<TreeNode>();
            _img = this.GetComponent<Image>();

            _node.OnLevelChanged += CB_OnManagedNodeLevelHasChanged;
            _node.OnStateChanged += CB_OnManagedNodeStateHasChanged;

            _text = GetComponentInChildren<Text>();
        }

        public void Init()
        {
            _manager = this.GetComponentInParent<TreeManager>();
            _manager.OnChangeBranchLevel += CB_OnTreeManagerUnlocksNextLevel;
        }

        void Start()
        {
            CB_OnManagedNodeLevelHasChanged(_node.Level, TreeNode.e_LevelOperation.ADD);
        }

        public void CB_OnManagedNodeLevelHasChanged(int newLevel, TreeNode.e_LevelOperation op)
        {
            UpdateNodeVisual();
            _text.text = newLevel.ToString() + "/" + _node.MaxLevel.ToString();
        }

        public void CB_OnManagedNodeStateHasChanged(TreeNode.e_NodeState state)
        {
            UpdateNodeVisual();
        }

        public void CB_OnTreeManagerUnlocksNextLevel(int newLevel)
        {
            UpdateNodeVisual();
        }

        private void UpdateNodeVisual()
        {
            if (_node.nodeBranch > _manager.Level)
            {
                _img.color = LockedNode;
            }
            else if (_node.Level == 0)
            {
                _img.color = AvailableNode;
            }
            else if (_node.Level == _node.MaxLevel)
            {
                _img.color = FullyUnlockedNode;
            }
            else
            {
                _img.color = PartiallyUnlockedNode;
                if (_node.Level < _node.MaxLevel)
                {
                    _img.color = Color.Lerp(_img.color, LockedNode, 0.5f);
                }
            }
        }

        public void LeftClickHandler()
        {
            //Debug.Log(_manager.canAddPointToNode(_node));

            if (_manager.canAddPointToNode(_node) == TreeManager.e_CanAddPointToNodeReturn.OK)
            {
                _node.Level++;
            }
            // else display error
        }

        public void RightClickHandler()
        {
            if (_manager.canRemovePointToNode(_node))
            {
                _node.Level--;
            }
        }

        public void OnPointerClick(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData)data;
            if (ped.button == PointerEventData.InputButton.Left)
            {
                LeftClickHandler();
            }
            else
            {
                RightClickHandler();
            }
        }
    }
}