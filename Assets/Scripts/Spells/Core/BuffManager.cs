using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spells
{
    /// <summary>
    /// Each entity MUST HAVE this script attached to
    /// </summary>
    public class BuffManager : MonoBehaviour
    {
        List<Spells.ST_Buff> _buffs = new List<ST_Buff>();

        /// <summary>
        /// Give the spell prefab to this function, it will instanciate it, make it a child of the target and start it.
        /// note that the caster may be the target
        /// also, if the buff already exist, the previous one is removed, and replaced with the new one
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        public Spells.ST_Buff AddBuff(GameObject buff, GameObject caster, GameObject target)
        {
            GameObject b = GameObject.Instantiate(buff);

            b.transform.parent = target.transform;
            b.transform.localPosition = Vector3.zero;

            Spells.BaseSpell baseSpell = b.GetComponentInChildren<Spells.BaseSpell>();

            baseSpell.Caster = caster;
            baseSpell.SpellTargets.Add(target);

            Spells.ST_Buff b2 = b.GetComponentInChildren<Spells.ST_Buff>();
            
            if (HasBuff(b2.BuffId)) // NO MULTIPLE INSTANCE OF THE SAME BUFF
            {
                DeleteBuff(b2.BuffId);
            }

            //b2.CurrentState = ST_EffectOnActivation.e_State.ENABLED;

            _buffs.Add(b2);

            return b2;
        }

        /// <summary>
        /// remove the buff from the list when it is over
        /// the template do it for you, so you should not have to call this function
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(Spells.ST_Buff buff)
        {
            _buffs.Remove(buff);
        }

        /// <summary>
        /// contrary to Removebuff, this one delete a buff on an entity, and does not call OnDispel.
        /// use this to refresh a buff, for exemple.
        /// </summary>
        /// <param name="buffId"></param>
        public void DeleteBuff(string buffId)
        {
            Spells.ST_Buff buff = _buffs.LastOrDefault(x => x.BuffId == buffId);
            if (buff != null)
            {
                buff.GetComponent<Spells.BaseSpell>().ForceEnd();
                _buffs.Remove(buff);
            }
        }

        /// <summary>
        /// dispel the last occurence of the buff/debuff of type buffId;
        /// </summary>
        /// <param name="buffId"></param>
        /// <param name="forceDispel"></param>
        public bool DispelBuff(string buffId, bool forceDispel = false)
        {
            Spells.ST_Buff buff = _buffs.LastOrDefault(x => x.BuffId == buffId);
            if (buff != null)
            {
                if (buff.IsDispellable || forceDispel)
                {
                    buff.Dispel();
                    _buffs.Remove(buff);
                }
                return true;
            }
            else
            {
                //gui msg, maybe
                return false;
            }
        }

        public bool HasBuff(string buffId)
        {
            return _buffs.Any(x => x.BuffId == buffId);
        }

        public Spells.ST_Buff getBuff<T>()
        {
            return (_buffs.SingleOrDefault(x => x.GetType() == typeof(T)));
        }
    }
}