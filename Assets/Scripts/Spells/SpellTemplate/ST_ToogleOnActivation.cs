using UnityEngine;
using System.Collections;


namespace Spells
{
    public abstract class ST_ToogleOnActivation : Spells.ST_EffectOnActivation
    {
        [SerializeField]
        protected float _ticInterval;
        public float TicInterval
        {
            get { return _ticInterval; }
            set { _ticInterval = value; }
        }

        /// <summary>
        /// resources consumed each tic 
        /// you just don't care about it, set this info in the SpellInfo
        /// </summary>
        private SpellResources _spellResources;
        public SpellResources SpellResources
        {
            get { return _spellResources; }
            set { _spellResources = value; }
        }

        private SpellResourcesManager _spellResourcesManager;
        public SpellResourcesManager SpellResourcesManager
        {
            get { return _spellResourcesManager; }
            set { _spellResourcesManager = value; }
        }

        protected virtual IEnumerator Co_ContinuousEffect()
        {
            while (true)
            {
                foreach (var v in SpellResources.CurrentSpellCost)
                {
                    if (!SpellResourcesManager.HasResources(v.Key, v.Value))
                    {
                        this.CurrentState = e_State.DISABLED;
                        yield break;
                    }
                }
                foreach (var v in SpellResources.CurrentSpellCost)
                {
                    SpellResourcesManager.UseResources(v.Key, v.Value);
                }
                yield return new WaitForSeconds(TicInterval);
            }
        }

        protected override void DoEffect()
        {
            StartCoroutine("Co_ContinuousEffect");
        }

        protected override void CancelEffect()
        {
            StopCoroutine("Co_ContinuousEffect");
        }
    }
}