using UnityEngine;
using System.Collections;

namespace Spells
{
    public abstract class ST_ToogleOverTime : Spells.ST_ToogleOnActivation
    {
        protected override void Awake()
        {
            base.Awake();
            _baseSpell.OnTic.AddListener(ContinuousEffect);
        }

        protected override IEnumerator Co_ContinuousEffect()
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
                _baseSpell.OnTic.Invoke();
                yield return new WaitForSeconds(TicInterval);
            }
        }

        protected abstract void ContinuousEffect();
    }
}