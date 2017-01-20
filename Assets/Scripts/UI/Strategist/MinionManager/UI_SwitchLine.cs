using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI_MinionManager
{
    public class UI_SwitchLine : MonoBehaviour
    {
        public CanvasGroup line1;
        public CanvasGroup line2;

        public void SwitchLine()
        {
            if (line1.interactable)
                switchLineAux(line1, line2);
            else
                switchLineAux(line2, line1);
        }

        void switchLineAux(CanvasGroup lineOn, CanvasGroup lineOff)
        {
            lineOn.interactable = false;
            lineOn.blocksRaycasts = false;
            lineOn.alpha = 0;
            lineOff.interactable = true;
            lineOff.blocksRaycasts = true;
            lineOff.alpha = 1;
        }
    }
}