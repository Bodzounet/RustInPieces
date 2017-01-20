using UnityEngine;
using System.Collections;

namespace SkillTreeBuilder
{
    public class BuildingRules : MonoBehaviour
    {
        public delegate void ValueChanged(int newValue);
        public event ValueChanged OnElitePickedChanged;
        public event ValueChanged OnRegularPickedChanged;

        private bool _elitePicked;
        public bool ElitePicked
        {
            get { return _elitePicked; }
            set
            {
                _elitePicked = value;
                OnElitePickedChanged(value ? 1 : 0);
            }
        }

        public int maxSkillPickable = 10;

        private int _skillPicked;
        public int SkillPicked
        {
            get { return _skillPicked; }
            set
            {
                _skillPicked = value;
                OnRegularPickedChanged(value);
            }
        }

        public bool canPickSkill(bool isEliteSkill)
        {
            if (isEliteSkill)
            {
                if (_elitePicked)
                    return false;
                return true;
            }
            else
            {
                if (_skillPicked >= maxSkillPickable)
                    return false;
                return true;
            }
        }

        public void PickSill(bool isEliteSkill)
        {
            if (!canPickSkill(isEliteSkill))
                return;

            if (isEliteSkill)
                ElitePicked = true;
            else
                SkillPicked++;
        }

        public void DropSkill(bool isEliteSkill)
        {
            if (isEliteSkill)
                ElitePicked = false;
            else if (SkillPicked > 0)
                SkillPicked--;
        }

        public void ResetSkills()
        {
            ElitePicked = false;
            SkillPicked = 0;
        }
    }
}