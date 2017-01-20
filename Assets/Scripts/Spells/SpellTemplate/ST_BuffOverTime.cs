using UnityEngine;
using System.Collections;

namespace Spells
{
    public abstract class ST_BuffOverTime : Spells.ST_Buff
    {
        [SerializeField]
        protected float _ticInterval;
        public float TicInterval
        {
            get { return _ticInterval; }
            set { _ticInterval = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            _baseSpell.OnTic.AddListener(ContinuousEffect);
        }

        protected virtual IEnumerator Co_ContinuousEffect()
        {
            while (true)
            {
                _baseSpell.OnTic.Invoke();
                yield return new WaitForSeconds(TicInterval);
            }
        }

        /// <summary>
        /// override and call DoEffect.base() prior to the code you'll add;
        /// </summary>
        protected override void DoEffect()
        {
            StartCoroutine("Co_ContinuousEffect");
        }

        /// <summary>
        /// Same here.
        /// </summary>
        protected override void CancelEffect()
        {
            StopCoroutine("Co_ContinuousEffect");
        }

        /// <summary>
        /// override this one for your effect.
        /// </summary>
        protected abstract void ContinuousEffect();
    }
}