using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// While active, calls GoIsInInfluenceRadius is a go enter the trigger
    /// calls GOIsNoMoreInInfluenceRadius if it leaves.
    /// </summary>
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_Aura : MonoBehaviour
    {
        protected Spells.BaseSpell _baseSpell;

        protected virtual void Awake()
        {
            _baseSpell = this.GetComponent<BaseSpell>();
            _baseSpell.OnTriggerEnterCallback += GOIsInInfluenceRadius;
            _baseSpell.OnTriggerExitCallback.AddListener(GOIsNoMoreInInfluenceRadius);
            _baseSpell.OnEndCallback.AddListener(OnEnd);
            _baseSpell.OnLifeTimeIsOver.AddListener(_baseSpell.End);
        }

        protected virtual void Start()
        {
        }

        protected abstract void GOIsInInfluenceRadius(GameObject go);

        protected abstract void GOIsNoMoreInInfluenceRadius(GameObject go);

        protected void OnEnd()
        {
            foreach (GameObject go in _baseSpell.TriggerTargets)
            {
                GOIsNoMoreInInfluenceRadius(go);
            }
        }
    }
}