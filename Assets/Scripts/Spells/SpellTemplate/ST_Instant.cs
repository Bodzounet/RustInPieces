using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// very easy here
    /// inherit from this class and code whatever your spell has to do in DoAction.
    /// it will mainly be one single and instant effect.
    /// for more complicated behaviour, check the other templates or create yours :p
    /// 
    /// use -1 as lifetime instead of 0 for this kind of spell.
    /// </summary>
    /// 
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_Instant : MonoBehaviour
    {
        protected Spells.BaseSpell _baseSpell;

        /// <summary>
        /// virtual protected so that if you need to get some other stuff, override it
        /// </summary>
        virtual protected void Awake()
        {
            _baseSpell = this.GetComponent<Spells.BaseSpell>();
            _baseSpell.OnLaunchCallback.AddListener(DoAction);
        }

        virtual protected void Start()
        {
            _baseSpell.End();
            //Destroy(this.gameObject);
        }

        protected abstract void DoAction();
    }
}