using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// create your AOE with this template.
    /// the effect is launched in the OnEnd function, to allow you to create delayed AOE.
    /// </summary>
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_AOE : MonoBehaviour
    {
        [SerializeField]
        protected int _ticCount; // set it to 1 if the aoe just do it effects once, when launched
        public int TicCount
        {
            get { return _ticCount; }
            set { _ticCount = value; }
        }

        [SerializeField]
        protected float _ticInterval; // set it to 0 if the aoe just do it effects once, when launched
        public float TicInterval
        {
            get { return _ticInterval; }
            set { _ticInterval = value; }
        }

        protected Spells.BaseSpell _baseSpell;

        [SerializeField]
        private float _radius;
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// virtual protected so that if you need to get some other stuff, override it
        /// </summary>
        virtual protected void Awake()
        {
            _baseSpell = this.GetComponent<Spells.BaseSpell>();
            if (_baseSpell.LifeTime != -1)
                _baseSpell.OnLifeTimeIsOver.AddListener(TriggerAOE);
            else
                _baseSpell.OnLaunchCallback.AddListener(TriggerAOE);
            _baseSpell.OnTic.AddListener(DoAction);
        }

        protected virtual void TriggerAOE()
        {
            StartCoroutine("Co_AOEAction");
        }

        protected IEnumerator Co_AOEAction()
        {
            for (int i = 0; i < TicCount; i++)
            {
                var cols = Physics.OverlapSphere(transform.position, _radius);
                _baseSpell.TriggerTargets.Clear();
                foreach (var v in cols)
                {
                    _baseSpell.TriggerTargets.Add(v.gameObject);
                }

                _baseSpell.OnTic.Invoke();

                if (i < TicCount - 1)
                    yield return new WaitForSeconds(TicInterval);
            }
            _baseSpell.End();
            Destroy(this.gameObject);
        }

        /// <summary>
        /// the effect of the trap when it "tics"
        /// GOs which are currently affected by the AOE are listed in TriggerTargets, and are updated while the AOE is active
        /// </summary>
        protected abstract void DoAction();

        /// <summary>
        /// no Collider for this one but an Overlapping Sphere (spell needs to know who collide on creation, but this is update after, so...)
        /// use this function to visualise the "collider"
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}