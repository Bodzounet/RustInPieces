using UnityEngine;
using System.Collections;

namespace StrategistUI
{
    public class SpellUI : MonoBehaviour
    {
        private LaunchSpellUI _launchSpellUI;
        public string spellId;

        void Start()
        {
            _launchSpellUI = this.GetComponentInParent<LaunchSpellUI>();
        }

        public void OnLeftClicked()
        {
            _launchSpellUI.Launch(spellId);
        }
    }
}