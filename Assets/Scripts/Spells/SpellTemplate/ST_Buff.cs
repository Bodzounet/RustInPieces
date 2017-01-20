using UnityEngine;
using System.Collections;

namespace Spells
{
    [RequireComponent(typeof(Spells.BaseSpell))]
    public abstract class ST_Buff : Spells.ST_EffectOnActivation
    {
        public enum e_Type
        {
            BUFF,
            DEBUFF
        }

        [SerializeField]
        private e_Type _type;
        public e_Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _isDispellable;
        public bool IsDispellable
        {
            get { return _isDispellable; }
            set { _isDispellable = value; }
        }

        /// <summary>
        /// unique for each buff class
        /// autocreated by this class 
        /// </summary>
        public string BuffId
        {
            get { return MD5Helper.GetMd5Hash(this.GetType().ToString()); }
        }

        /// <summary>
        /// this should be the id of the entity who is the source of this (de)buff
        /// this allows to add multiple instances of this (de)buff, one for each entity.
        /// of course you can also just override the existing one.
        /// </summary>
        private string _playerId;
        public string PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            _baseSpell.OnEndCallback.AddListener(() => { GetComponentInParent<BuffManager>().RemoveBuff(this); /*Destroy(this.transform.parent.gameObject); */});
            _baseSpell.OnLifeTimeIsOver.AddListener(Remove);
        }

        /// <summary>
        /// if the spell is dispelled, this one will be called, instead of CancelEffect
        /// </summary>
        protected abstract void OnDispel();

        public void Dispel()
        {
            _baseSpell.OnEndCallback.RemoveListener(CancelEffect);
            _baseSpell.OnEndCallback.AddListener(OnDispel);
            Remove();
        }

        public void Remove()
        {
            _baseSpell.OnEndCallback.Invoke();
            Destroy(gameObject);
        }
    }
}