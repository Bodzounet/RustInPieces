using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SkillTreeBuilder
{
    public class SkillProgression : MonoBehaviour
    {
        BuildingRules _br;
        Text _text;

        void Awake()
        {
            _br = GetComponentInParent<BuildingRules>();
            _text = GetComponentInChildren<Text>();

            _br.OnElitePickedChanged += (int i) => { DisplayProgression(); };
            _br.OnRegularPickedChanged += (int i) => { DisplayProgression(); };
        }

        void Start()
        {
            DisplayProgression();
        }

        public void DisplayProgression()
        {
            _text.text = " Tree : \n\t Elite : " + (_br.ElitePicked ? "1" : "0") + "/1\n\t Regular : " + _br.SkillPicked.ToString() + "/" + _br.maxSkillPickable.ToString();
        }
    }
}