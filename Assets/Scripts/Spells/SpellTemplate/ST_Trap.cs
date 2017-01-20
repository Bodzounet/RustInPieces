using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells
{
    /// <summary>
    /// template for a trap.
    /// inherit for this class and override the "DoAction" Method.
    /// code the trap "tic" effect in this method, and enjoy.
    /// </summary>
    /// 
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_Trap : MonoBehaviour
    {
        [SerializeField]
        protected int _ticCount;
        public int TicCount
        {
            get { return _ticCount; }
            set { _ticCount = value; }
        }

        [SerializeField]
        protected float _ticInterval;
        public float TicInterval
        {
            get { return _ticInterval; }
            set { _ticInterval = value; }
        }
        
        protected Spells.BaseSpell _baseSpell;

        /// <summary>
        /// virtual protected so that if you need to get some other stuff, override it
        /// </summary>
        virtual protected void Awake()
        {
            _baseSpell = this.GetComponent<Spells.BaseSpell>();
            _baseSpell.OnTriggerEnterCallback += TriggerTrap;
            _baseSpell.OnLifeTimeIsOver.AddListener(TriggerTrapAux);
            _baseSpell.OnTic.AddListener(DoAction);
        }

        protected void TriggerTrap(GameObject TriggeringGameObject)
        {
            if (!mustTrigger(TriggeringGameObject))
                return;

            TriggerTrapAux();
        }

        private void TriggerTrapAux()
        {
            _baseSpell.OnTriggerEnterCallback += TriggerTrap;
            StartCoroutine("Co_TrapAction");
        }

        protected IEnumerator Co_TrapAction()
        {
            for (int i = 0; i < TicCount; i++)
            {
                _baseSpell.OnTic.Invoke();
                yield return new WaitForSeconds(TicInterval);
            }
            _baseSpell.End();
            Destroy(this.gameObject);
        }

        /// <summary>
        /// the effect of the trap when it "tics"
        /// GOs which are currently affected by the trap are listed in TriggerTargets, and are updated while the trap is active
        /// </summary>
        protected abstract void DoAction();

        protected abstract bool mustTrigger(GameObject GoInTrigger);
    }
}