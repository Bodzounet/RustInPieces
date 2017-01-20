using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SkillTreeBuilder
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField]
        CanvasGroup _mainUI;
        CanvasGroup _popUpUI;

        Text _msg;

        void Awake()
        {
            _popUpUI = this.GetComponent<CanvasGroup>();
            _msg = transform.FindChild("Msg").GetComponent<Text>();
        }

        public void SpawnPopUp(string msg)
        {
			if (_mainUI != null)
	            switchUiState(_mainUI, false);
            switchUiState(_popUpUI, true);
            _msg.text = msg;
        }

        public void ClosePopUp()
        {
			if (_mainUI != null)
				switchUiState(_mainUI, true);
            switchUiState(_popUpUI, false);
        }

        private void switchUiState(CanvasGroup cg, bool active)
        {
            cg.interactable = active ? true : false;
            cg.blocksRaycasts = active ? true : false;
            cg.alpha = active ? 1 : 0;
        }
    }
}