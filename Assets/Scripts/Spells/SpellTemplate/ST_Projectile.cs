using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// Here is the template to create a projectile.
    /// This template will move the root in his hierachy.
    /// So don't attach this one to a player or something like that
    /// </summary>
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_Projectile : MonoBehaviour
    {
        [SerializeField]
        private bool _homingMissile;
        public bool HomingMissile
        {
            get { return _homingMissile; }
            set { _homingMissile = value; }
        }

        [SerializeField]
        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        protected Spells.BaseSpell _baseSpell;

        /// <summary>
        /// virtual protected so that if you need to get some other stuff, override it
        /// </summary>
        virtual protected void Awake()
        {
            _baseSpell = this.GetComponent<Spells.BaseSpell>();
            _baseSpell.OnTriggerEnterCallback += OnTriggerEnterCallback;
            _baseSpell.OnEndCallback.AddListener(OnEndCallback);
            _baseSpell.OnLifeTimeIsOver.AddListener(OnEndCallback);
        }

        virtual protected void Start()
        {
            if (_homingMissile)
            {
                StartCoroutine("Co_HomingMissile");
            }
            else
            {
                StartCoroutine("Co_BasicMissile");
            }
        }

        protected virtual void OnTriggerEnterCallback(GameObject collidingObject)
        {
            DoAction(collidingObject);
            //StopAllCoroutines();
        }

        protected virtual void OnEndCallback()
        {
            Destroy(gameObject);
        }

        protected IEnumerator Co_HomingMissile()
        {
            while (true)
            {
                if (_baseSpell.SpellTargets[0] == null)
                {
                    _baseSpell.End();
                    yield break;
                }
                transform.root.position = Vector3.MoveTowards(this.transform.position, _baseSpell.SpellTargets[0].transform.position, Speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        protected IEnumerator Co_BasicMissile()
        {
            while (true)
            {
                transform.parent.position += this.transform.forward * Speed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Here you can only make an instant effect.
        /// if you start a coroutine, it will NOT work.
        /// if you need a projectile that make a lasting effect when it reaches its target, 2 solutions : 
        ///     instanciante this effect in the DoAction
        ///     create another spell template (or ask me...)
        ///     
        /// 
        /// the hit target should also be in _baseSpell.TriggerTarget[0]
        /// </summary>
        protected abstract void DoAction(GameObject collidingObject);
    }
}