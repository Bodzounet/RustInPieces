using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// The base for buff, debuff, aura, toogle, etc...
    /// Don't use it directly, use one of its children
    /// </summary>
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_EffectOnActivation : MonoBehaviour
    {
        public bool autoLaunch = true; // does this buff start as soon as it is created ?

        public enum e_State
        {
            ENABLED,
            DISABLED
        }

        [SerializeField]
        protected e_State _state = e_State.ENABLED;
        public e_State CurrentState
        {
            get { return _state; }
            set 
            {
                _state = value; 
                switch (value)
                {
                    case e_State.DISABLED:
                        CancelEffect();
                        break;
                    case e_State.ENABLED:
                        DoEffect();
                        break;
                }
            }
        }

        protected Spells.BaseSpell _baseSpell;

        /// <summary>
        /// virtual protected so that if you need to get some other stuff, override it
        /// </summary>
        virtual protected void Awake()
        {
            //Debug.Log("Hey, je suis un Awake, de " + this.GetType().ToString());

            _baseSpell = this.GetComponent<Spells.BaseSpell>();
            _baseSpell.OnLifeTimeIsOver.AddListener(CancelEffect);
            _baseSpell.OnLifeTimeIsOver.AddListener(_baseSpell.End);

            if (autoLaunch)
                _baseSpell.OnLaunchCallback.AddListener(() => { /*Debug.Log("Moi je suis un message dans le callback qui va appeler le setter"); */CurrentState = _state; });
        }

        virtual protected void Start()
        {
            //// this two lines place the effect has a child of the caster, and center the spell on him.
            //// feel free to override them if your effect targets someone else
            //if (spawnOnCaster)
            //{
            //    transform.parent.parent = _baseSpell.Caster.transform;
            //    transform.parent.localPosition = Vector3.zero;
            //}
        }

        /// <summary>
        /// when this effect is actually enabled. if it is an effect over time, use ST_EffectOverTime instead of this class
        /// </summary>
        protected abstract void DoEffect();

        /// <summary>
        /// when it becomes disabled (passive only when active reloading, for example)
        /// </summary>
        protected abstract void CancelEffect();
    }
}