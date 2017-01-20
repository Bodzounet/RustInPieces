using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// this script is placed on the GameObject which is the parent of the Script holding the BaseSpell 
    /// 
    /// You Should (must) created your script that inherits from this one in order to do an appropriated animation behaviour
    /// 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SpellAnimation : MonoBehaviour
    {
        void Awake()
        {
            var _baseSpell = this.GetComponentInChildren<BaseSpell>();
            _baseSpell.OnLaunchCallback.AddListener(onLaunch);
            _baseSpell.OnTic.AddListener(onTic);
            _baseSpell.OnEndCallback.AddListener(onEnd);
        }

        /// <summary>
        /// When the spells spawn.
        /// For instant, projectile, AOEs, Traps, etc...
        /// </summary>
        virtual protected void onLaunch()
        {
        }

        /// <summary>
        /// for AOEs and Traps, and maybe for DoT and HoT
        /// </summary>
        virtual protected void onTic()
        {
        }

        /// <summary>
        /// When the spell is over.
        /// Be careful of parenting. If the animation is a child of a gameobject such as the caster, the spell or the target, and this one is destroyed, the animation is also destroyed too.
        /// life time should be managed by the animator, but it is a mess, so we have to do with a dirty solution
        /// </summary>
        virtual protected void onEnd()
        {
        }

        public void DestroySpell()
        {
            Destroy(this.gameObject);
        }
    }
}