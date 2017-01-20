using UnityEngine;
using System.Collections;

namespace Spells
{
    public abstract class LaunchingConditions : MonoBehaviour
    {
        public abstract bool CheckConditions(GameObject caster, e_Team casterTeam, GameObject target);
    }
}