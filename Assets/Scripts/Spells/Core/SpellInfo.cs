using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells
{
    /// <summary>
    /// This class contains information about any spell you will create.
    /// this is this script you give to your Hero or to your strategist as a spell.
    /// the, when you launch a spell, it is this script again that you give as a parameter for any "Launch" function
    /// I designed it so that it work with all templates i created, but you may have to add some code here in the future.
    /// 
    /// </summary>
    [RequireComponent(typeof(SpellResources))]
    public class SpellInfo : MonoBehaviour
    {
        [System.Serializable]
        public enum e_CastType
        {
            NO_TARGET,
            TARGET,
            POSITION,
            TOGGLE,
            SELFBUFF
        }
        /// <summary>
        /// The cast type of the spell. Wether it need a target, a location or nothing to be casted.
        /// </summary>
        [SerializeField]
        private Spells.SpellInfo.e_CastType _type;
        public Spells.SpellInfo.e_CastType CastType
        {
            get { return _type; }
        }
        /// <summary>
        /// The name of the spell. Be sure that is is UNIQUE
        /// you also HAVE TO give this name to the clip corresponding to the animation 
        /// </summary>
        [SerializeField]
        private string _id;
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Each of these GameObjects MUST have at least a BaseSpell script attached to them (and probably a ST_XXX)
        /// if you give something else, you'll get a lot of error when you'll try to launch the spell...
        /// </summary>
        public GameObject spellActive;
        public GameObject passiveEffect;

        /// <summary>
        /// required to remove this effect when not needed anymore.
        /// </summary>
        private Spells.ST_EffectOnActivation _passiveEffectInstance;
        public Spells.ST_EffectOnActivation PassiveEffectInstance
        {
            get { return _passiveEffectInstance; }
            set { _passiveEffectInstance = value; }
        }

        public bool isBuff;
        public bool isPassive;
        public bool isPassiveWhileNotOnCooldown;
        public bool isToogled;
        public bool producesMovement = false;
        public bool useRange = false;

        private SpellResources _spellResources;
        public SpellResources SpellResources
        {
            get { return _spellResources; }
            set { _spellResources = value; }
        }

        /// <summary>
        /// May be null.
        /// </summary>
        private Spells.LaunchingConditions _launchingConditions;
        public Spells.LaunchingConditions LaunchingConditions
        {
            get { return _launchingConditions; }
            set { _launchingConditions = value; }
        }

        [SerializeField]
        private int _maxCharges; // 1 for most of the spells, more for spells which accumulates charges
        public int MaxCharges
        {
            get { return _maxCharges; }
            set { _maxCharges = value; }
        }

        private int _charges;
        public int Charges
        {
            get { return _charges; }
            set 
            {
                _charges = value;
            }
        }

        [SerializeField]
        private float _baseCooldown;
        public float BaseCooldown
        {
            get { return _baseCooldown; }
            set { _baseCooldown = value; }
        }

        private float _cooldown;
        public float Cooldown
        {
            get { return _cooldown; }
            set { _cooldown = value; }
        }

        /// <summary>
        /// Set it to 0 so that the spell is launch as soon as the player click on it.
        /// </summary>
        [SerializeField]
        private float _baseCastingTime;
        public float BaseCastingTime
        {
            get { return _baseCastingTime; }
        }

        private float _castingTime;
        public float CastingTime
        {
            get { return _castingTime; }
            set { _castingTime = value; }
        }

        [SerializeField]
        private float _baseMinRange;
        public float BaseMinRange
        {
            get { return _baseMinRange; }
        }

        private float _minRange;
        public float MinRange
        {
            get { return _minRange; }
            set { _minRange = value; }
        }

        [SerializeField]
        private float _baseMaxRange;
        public float BaseMaxRange
        {
            get { return _baseMaxRange; }
        }

        private float _maxRange;
        public float MaxRange
        {
            get { return _maxRange; }
            set { _maxRange = value; }
        }

        [SerializeField]
        string _spellName;
        public string SpellName
        {
            get { return (_spellName); }
            set { _spellName = value; }
        }

        [SerializeField]
        string _spellDesc;
        public string SpellDesc
        {
            get { return (_spellDesc); }
            set { _spellDesc = value; }
        }

        [SerializeField]
        string _manaCost;
        public string ManaCost
        {
            get { return (_manaCost); }
            set { _manaCost = value; }
        }

        [SerializeField]
        Sprite _icon;
        public Sprite Icon
        {
            get { return (_icon); }
            set { _icon = value; }
        }

        void Awake()
        {
            _spellResources = this.GetComponent<SpellResources>();
            _launchingConditions = this.GetComponent<LaunchingConditions>();
            Charges = MaxCharges;
            Cooldown = BaseCooldown;
            CastingTime = BaseCastingTime;
            MinRange = BaseMinRange;
            MaxRange = BaseMaxRange;
        }
    }
}