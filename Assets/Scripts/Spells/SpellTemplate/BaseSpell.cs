using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Spells
{
    public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    public class BaseSpell : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnLaunchCallback = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnLifeTimeIsOver = new UnityEvent();
        [HideInInspector]
        public Void_D_GameObject OnTriggerEnterCallback;
        [HideInInspector]
        public UnityEvent_GameObject OnTriggerStayCallback = new UnityEvent_GameObject();
        [HideInInspector]
        public UnityEvent_GameObject OnTriggerExitCallback = new UnityEvent_GameObject();
        [HideInInspector]
        public UnityEvent OnEndCallback = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnForcedEndCallback = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnTic = new UnityEvent();

        private GameObject _caster;
        public GameObject Caster
        {
            get { return _caster; }
            set { _caster = value; }
        }

        // the spell is already instanciated at this pos.
        // yet you may need it anyway, si here it is.
        // only revelant for spells which take a position
        private Vector3 _spellPos;
        public Vector3 SpellPos
        {
            get { return _spellPos; }
            set { _spellPos = value; }
        }

        private List<GameObject> _spellTargets = new List<GameObject>();
        public List<GameObject> SpellTargets
        {
            get { return _spellTargets; }
        }

        private List<GameObject> _triggerTargets = new List<GameObject>();
        public List<GameObject> TriggerTargets
        {
            get { return _triggerTargets; }
        }

        /// <summary>
        /// the time before the spell will be launch its end effect.
        /// -1 means never, 0 means immediatly.
        /// it is useful for some spell, and you may use it for a simple delayed spell.
        /// 
        /// remember that the spell is not automatically destroyed at the end of this timer.
        /// </summary>
        [SerializeField]
        private float _lifeTime = -1;
        public float LifeTime
        {
            get { return _lifeTime; }
        }

        [SerializeField]
        private float _unFreezeDelay = -1;
        public float UnFreezeDelay
        {
            get { return _unFreezeDelay; }
        }

        [SerializeField]
        private bool _centerOnCaster = false;
        public bool CenterOnCaster
        {
            get { return (_centerOnCaster); }
        }

        [SerializeField]
        private bool _followCaster = false;
        public bool FollowCaster
        {
            get { return (_followCaster); }
        }

        private float _timeCharged = 0;
        public float TimeCharged
        {
            get { return _timeCharged; }
            set { _timeCharged = value; }
        }

        /// <summary>
        /// Time before the spell or the buff expires
        /// </summary>
        private float _remainingTime = -1;
        public float RemainingTime
        {
            get { return _remainingTime; }
        }

        protected void Start()
        {
            if (OnLaunchCallback != null)
            {
                OnLaunchCallback.Invoke();
            }

            if (LifeTime != -1)
            {
                _remainingTime = LifeTime;
                StartCoroutine("Co_LifeTime");
            }

            if (_unFreezeDelay > 0 && (_unFreezeDelay <= _lifeTime || _lifeTime < 0) && _caster.GetComponent<HeroController>() != null)
            {
                _caster.GetComponent<HeroController>().BlockDuringCast(_unFreezeDelay);
            }

            if (CenterOnCaster)
            {
                transform.parent.parent = _caster.transform.Find("Model");
                transform.parent.localPosition = Vector3.zero;
                transform.parent.rotation = transform.parent.parent.rotation;
                transform.localEulerAngles = Vector3.zero;
                transform.parent.parent = null;
            }

            if (FollowCaster)
            {
                transform.parent.parent = _caster.transform.Find("Model");
            }
        }

        /// <summary>
        /// call the registered callbacks, and give the new colliding gameobject as parameter
        /// also update the list of actual colliding object
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerEnter(Collider col)
        {
            if (((1 << col.gameObject.layer) & LayerMask.GetMask("Entity", "HeroEntity", "MinionEntity", "TowerEntity")) == 0)
                return;

            if (col.GetComponentInParent<Entity>().getRemainingStateTime(Entity.e_EntityState.BLOCK) > 0)
            {
                End();
                col.GetComponentInParent<Entity>().setRemainingStateTime(Entity.e_EntityState.BLOCK, 0);
                return;
            }

            //Debug.Log("OnTriggerEnter, with an Entity");
            TriggerTargets.Add(col.gameObject);

            if (OnTriggerEnterCallback != null)
            {
                OnTriggerEnterCallback.Invoke(col.gameObject);
            }
        }

        void OnTriggerStay(Collider col)
        {
            //Debug.Log("OnTriggerStay : " + col.name);

            if (((1 << col.gameObject.layer) & LayerMask.GetMask("Entity", "HeroEntity", "MinionEntity", "TowerEntity")) == 0)
                return;

            //Debug.Log("OnTriggerStay, with an Entity");
            if (OnTriggerStayCallback != null)
            {
                OnTriggerStayCallback.Invoke(col.gameObject);
            }
        }

        void OnTriggerExit(Collider col)
        {
            //Debug.Log("OnTriggerExit : " + col.name);

            if (((1 << col.gameObject.layer) & LayerMask.GetMask("Entity", "HeroEntity", "MinionEntity", "TowerEntity")) == 0)
                return;

            //Debug.Log("OnTriggerExit, with an Entity");
            TriggerTargets.Remove(col.gameObject);

            if (OnTriggerExitCallback != null)
            {
                OnTriggerExitCallback.Invoke(col.gameObject);
            }
        }

        /// <summary>
        /// this is NOT a unity callback.
        /// spell templates call this one for you when needed
        /// if you create your own template, you HAVE TO call it when your spell is over. (if you need it ofc)
        /// 
        /// Note that the spell IS NOT DESTROYED.
        /// this is because a long task can happen when the spell is over (for example, the AOE starts when the countdown is over)
        /// spell templates destroy the spell for you when it is over.
        /// if you create your own template, don't forget to destroy it too.
        /// 
        /// and DON'T use OnDestroy since it destroys scripts in a random order, so this one may exist while the one with the callback has already been erased
        /// 
        /// for passive or toogle spells, when the spell is remove for the player available spells, this one is call before destrying the spell.
        /// so, use it to clean your spell effect.
        /// </summary>
        public void End()
        {
            if (OnEndCallback != null)
            {
                //Debug.Log("SpellIsEnding");
                OnEndCallback.Invoke();
            }
        }

        /// <summary>
        /// force the end of this buff, and may not call the ending behaviour of this spell.
        /// also destroy this spell and its parent
        /// </summary>
        public void ForceEnd()
        {
            StopCoroutine("Co_LifeTime");
            if (OnForcedEndCallback != null)
            {
                OnForcedEndCallback.Invoke();
            }
            else
            {
                End();
            }
            Destroy(transform.parent.gameObject);
        }

        IEnumerator Co_LifeTime()
        {
            while (RemainingTime > 0)
            {
                yield return new WaitForEndOfFrame();
                _remainingTime -= Time.deltaTime;
            }
            OnLifeTimeIsOver.Invoke();
        }
    }
}