using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Spells
{
    public class SpellLauncher : MonoBehaviour
    {
        public enum e_LaunchReturn
        {
            IsAlreadyLaunching,
            SpellInCooldown,
            NotEnoughResources,
            NotInRange,
            InvalidTarget,

            ok
        }

        /// <summary>
        /// The Hero or the Strategist.
        /// Don't mess with this one, because the caster is the source of many spells, and so many spells rely on its transform.
        /// Each spell has a reference to its caster, so it allow you to do whatever you want with it, if you have set it correctly :p
        /// 
        /// Be SURE that it has an animator, which contains the clips related to his spells
        /// </summary>
        public GameObject caster;
        private Animator _animator;

        /// <summary>
        /// Don't mess with this one too.
        /// I can't use RequireComponent because the class is abstract, but it should come with the SpellLauncher.
        /// So just put it along with this Script, and give a ref.
        /// </summary>
        public Spells.SpellResourcesManager spellResourceManager;

        [SerializeField]
        private string[] _spells; // use the IDs

        /// <summary>
        /// Never ever put the same spell twice in this list.
        /// If you need to to this for any reason, ask me.
        /// 
        /// in fact, never use this list directly.
        /// use addSpell or removeSpell
        /// </summary>
        ObservableCollection<Spells.SpellInfo> _availableSpells = new ObservableCollection<SpellInfo>();
        public ObservableCollection<Spells.SpellInfo> AvailableSpells
        {
            get { return _availableSpells; }
        }

        private List<Spells.CooldownManager> _currentCooldowns = new List<CooldownManager>();

        public Void_D_Int LaunchCallback;

        private TeamFactory _factory;

        void Awake()
        {
            e_Team myTeam;
            Entity e = GetComponent<Entity>();
            if (e != null)
                myTeam = e.Team;
            else
                myTeam = GetComponent<StrategistManager>().Team;

            _factory = FindObjectsOfType<TeamFactory>().Single(x => x.MyTeam == myTeam);
        }

        void Start()
        {
            //#if UNITY_EDITOR
            //            if (caster == null)
            //                Debug.LogError("You forgot to assign a caster to this SpellLauncher");
            //#endif
            if (caster == null)
                caster = this.gameObject;
            _animator = this.GetComponentInParent<Animator>();
            //#if UNITY_EDITOR
            //            if (_animator == null)
            //                Debug.LogWarning("there is no animator on this GameObject parent. It could (will) create problems");
            //#endif

            _availableSpells.CollectionChanged += OnAvailableSpellIsModified;

            // init the spells
            foreach (string id in _spells)
            {
                AddSpell(id);
            }
        }

        public void AddSpell(string spellId)
        {
            if (_availableSpells.Any(x => x.Id == spellId))
                Debug.LogError("this Launcher already contains this spell : " + spellId);

            var si = _factory.CreateSpellInfo(spellId);
            si.transform.parent = caster.transform;
            _availableSpells.Add(si.GetComponent<Spells.SpellInfo>());
        }

        public void RemoveSpell(string spellId)
        {
            var si = _availableSpells.SingleOrDefault(x => x.Id == spellId);

            if (si == null)
                Debug.LogError("this Launcher does not contain this spell : " + spellId);

            _availableSpells.Remove(si);
            Destroy(si.gameObject);
        }

        public bool HasSpell(string spellId)
        {
            return _availableSpells.SingleOrDefault(x => x.Id == spellId) != null;
        }

        #region Launch Functions

        private bool _isLaunching = false;
        public bool IsLaunching
        {
            get
            {
                return _isLaunching;
            }
            set
            {
                _isLaunching = value;
            }
        }

        public e_LaunchReturn isTargetValid(string spellId, GameObject caster, e_Team casterTeam, GameObject target)
        {
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

            return (si.LaunchingConditions == null || si.LaunchingConditions.CheckConditions(caster, casterTeam, target) ? e_LaunchReturn.ok : e_LaunchReturn.InvalidTarget);
        }

        public e_LaunchReturn LaunchToogle(string spellId)
        {
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (!si.isToogled)
                Debug.LogError("use this function only to start or stop toogle");
#endif
            Spells.ST_ToogleOnActivation toogle = si.PassiveEffectInstance as Spells.ST_ToogleOnActivation;

            if (toogle.CurrentState == Spells.ST_EffectOnActivation.e_State.DISABLED)
            {
                Debug.Log("Launcher already in action");
                var ret = LaunchAux_CanLaunch(si);
                if (ret != e_LaunchReturn.ok)
                    return ret;
                toogle.CurrentState = Spells.ST_EffectOnActivation.e_State.ENABLED;
            }
            else
            {
                toogle.CurrentState = Spells.ST_EffectOnActivation.e_State.DISABLED;
            }
            return e_LaunchReturn.ok;
        }

        /// <summary>
        /// for spells without target nor spawning position (caster != target)
        /// </summary>
        /// <param name="si"></param> the spellinfo of the spell to launch
        /// <returns></returns>
        [PunRPC]
        public e_LaunchReturn Launch(string spellId)
        {
            //Debug.Log(spellId + " a été casté ! ");
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (si.isPassive)
                Debug.LogError("You are trying to cast a passive spell...");

            if (si.isBuff)
                Debug.LogError("You are trying to cast a buff/debuff, use Launch(string spellId, GameObject[] targets) instead");

            if (si.isToogled)
                Debug.LogError("Use LaunchToogle to start or stop toogles");
#endif
            if (IsLaunching)
            {
                Debug.Log("Launcher already in action");
                return e_LaunchReturn.IsAlreadyLaunching;
            }
            var ret = LaunchAux_CanLaunch(si);
            if (ret != e_LaunchReturn.ok)
                return ret;

            StartCoroutine("Co_Launch", new LaunchHelper(LaunchAux_LaunchV1, new LaunchArgs(si)));

            if (LaunchCallback != null)
            {
                LaunchCallback.Invoke(_availableSpells.IndexOf(si));
            }
            return e_LaunchReturn.ok;
        }

        /// <summary>
        /// for spells with a spawning position but without target 
        /// mainly design for AOE and Traps
        /// </summary>
        /// <param name="si"></param> the spellinfo of the spell to launch
        /// <param name="pos"></param> the position where the spell has to spawn (and not what it is aiming at.)
        /// <returns></returns>
        [PunRPC]
        public e_LaunchReturn Launch(string spellId, Vector3 pos)
        {
            //Debug.Log(spellId + " a été casté a la pos ! " + pos);
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (si.isPassive)
                Debug.LogError("You are trying to cast a passive spell...");

            if (si.isBuff)
                Debug.LogError("You are trying to cast a buff/debuff, use Launch(string spellId, GameObject[] targets) instead");

            if (si.isToogled)
                Debug.LogError("Use LaunchToogle to start or stop toogles");
#endif
            if (IsLaunching)
            {
                Debug.Log("Launcher already in action");
                return e_LaunchReturn.IsAlreadyLaunching;
            }
            var ret = LaunchAux_CanLaunch(si);
            if (ret != e_LaunchReturn.ok)
                return ret;
            ret = LaunchAux_IsInRange(spellId, caster, pos);
            if (ret != e_LaunchReturn.ok)
                return ret;

            StartCoroutine("Co_Launch", new LaunchHelper(LaunchAux_LaunchV2, new LaunchArgs(si, pos)));

            if (LaunchCallback != null)
                LaunchCallback.Invoke(_availableSpells.IndexOf(si));
            return e_LaunchReturn.ok;
        }

        /// <summary>
        /// mainly designed for projectiles, or mutli target instant spells
        /// note that with this launch function, each spell is instanciated at the caster position by default.
        /// for each go in traget, a spell will be instanciated at the caster position and will aim to a single target.
        /// this means that if you have set 3 targets and you are calling this function, 3 projectiles will be instanciated, one for each target.
        /// Yet the cost of the spell is deduce only once, and so it's the charge.
        /// if you need a Launcher allowing to target multi target and removing the spell cost for each of them, just ask me and i'll add it
        /// </summary>
        /// <param name="si"></param> the spell to be launched
        /// <param name="targets"></param> target is not the potential targets the spell may hit, but the targets the spell effectively hits.
        /// <returns></returns>
        public e_LaunchReturn Launch(string spellId, GameObject[] targets)
        {
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (si.isPassive)
                Debug.LogError("You are trying to cast a passive spell...");

            if (si.isToogled)
                Debug.LogError("Use LaunchToogle to start or stop toogles");

            if (targets.Length == 0)
                Debug.LogWarning("Your spell has no target");
#endif
            if (IsLaunching)
            {
                Debug.Log("Launcher already in action");
                return e_LaunchReturn.IsAlreadyLaunching;
            }
            var ret = LaunchAux_CanLaunch(si);

            if (ret != e_LaunchReturn.ok)
                return ret;
            foreach (var v in targets)
            {
                ret = LaunchAux_IsInRange(spellId, caster, v.transform.position);
                if (ret != e_LaunchReturn.ok)
                    return ret;
            }

            StartCoroutine("Co_Launch", new LaunchHelper(LaunchAux_LaunchV3, new LaunchArgs(si, targets)));

            if (LaunchCallback != null)
            {
                LaunchCallback.Invoke(_availableSpells.IndexOf(si));
            }

            return e_LaunchReturn.ok;
        }

        [PunRPC]
        public e_LaunchReturn Launch(string spellId, int[] photonId, int[] minionId)
        {
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

            GameObject[] targets;

            List<GameObject> tarTemp = new List<GameObject>();
            foreach (int id in photonId)
            {
                tarTemp.Add(PhotonView.Find(id).gameObject);
            }

            targets = tarTemp.ToArray();

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (si.isPassive)
                Debug.LogError("You are trying to cast a passive spell...");

            if (si.isToogled)
                Debug.LogError("Use LaunchToogle to start or stop toogles");

            if (targets.Length == 0)
                Debug.LogWarning("Your spell has no target");
#endif
            if (IsLaunching)
            {
                Debug.Log("Launcher already in action");
                return e_LaunchReturn.IsAlreadyLaunching;
            }
            var ret = LaunchAux_CanLaunch(si);
            if (ret != e_LaunchReturn.ok)
                return ret;
            foreach (var v in targets)
            {
                ret = LaunchAux_IsInRange(spellId, caster, v.transform.position);
                if (ret != e_LaunchReturn.ok)
                    return ret;
            }

            StartCoroutine("Co_Launch", new LaunchHelper(LaunchAux_LaunchV3, new LaunchArgs(si, targets)));

            return e_LaunchReturn.ok;
        }

        /// <summary>
        /// if none of the previous Launch function fits, try using this one.
        /// </summary>
        /// <param name="si"></param> the spell to be Launched
        /// <param name="targets"></param> same as before
        /// <param name="pos"></param> the spawning position of each spell. spell will spawn for targets[0] at pos[0], etc...
        /// <returns></returns>
        [PunRPC]
        public e_LaunchReturn Launch(string spellId, GameObject[] targets, Vector3[] pos)
        {
            Spells.SpellInfo si = getSpellInfoFromId(spellId);

#if UNITY_EDITOR
            if (spellId == null)
                Debug.LogError("this launcher has not the required spell : " + spellId);

            if (si.isPassive)
                Debug.LogError("You are trying to cast a passive spell...");

            if (si.isBuff)
                Debug.LogError("You are trying to cast a buff/debuff, use Launch(string spellId, GameObject[] targets) instead");

            if (si.isToogled)
                Debug.LogError("Use LaunchToogle to start or stop toogles");

            if (targets.Length == 0 || pos.Length == 0)
                Debug.LogWarning("Your spell has no target or no spawning position");

            if (targets.Length != pos.Length)
                Debug.LogError("the two arrays must strictly have the same size");
#endif
            if (IsLaunching)
            {
                return e_LaunchReturn.IsAlreadyLaunching;
            }
            var ret = LaunchAux_CanLaunch(si);
            if (ret != e_LaunchReturn.ok)
                return ret;

            for (int i = 0; i < targets.Length; i++)
            {
                if (((ret = LaunchAux_IsInRange(spellId, caster, targets[i].transform.position)) == e_LaunchReturn.NotInRange) ||
                    ((ret = LaunchAux_IsInRange(spellId, caster, pos[i])) == e_LaunchReturn.NotInRange))
                {
                    return ret;
                }
            }
            StartCoroutine("Co_Launch", new LaunchHelper(LaunchAux_LaunchV4, new LaunchArgs(si, targets, pos)));

            return e_LaunchReturn.ok;
        }

        /// <summary>
        /// call this one to cancel the animation this animation (for example, player was incantating and move to cancel)
        /// don'try something else
        /// CALL THIS ONE.
        /// </summary>
        [PunRPC]
        public void CancelLaunch()
        {
            //_animator.Play("Idle"); // subject to modifications

            // in cd ?
            //_launchArgs.spellInfo.Charges--;
            //StartSpellCooldown(_launchArgs.spellInfo);

            StopCoroutine("Co_Launch");
            IsLaunching = false;
        }

        private e_LaunchReturn LaunchAux_CanLaunch(Spells.SpellInfo si)
        {
            if (si.Charges == 0)
            {
                //Debug.Log("Spell in cooldown");
                return e_LaunchReturn.SpellInCooldown;
            }
            foreach (var v in si.SpellResources.CurrentSpellCost)
            {
                //Debug.Log("check for ressources: " + v.Key + " " + v.Value);
                if (!spellResourceManager.HasResources(v.Key, v.Value))
                {
                    //Debug.Log("not enough Resources");
                    return e_LaunchReturn.NotEnoughResources;
                }
            }
            return e_LaunchReturn.ok;
        }

        private void LaunchAux_ConsumeResourcesAndCharge(Spells.SpellInfo si)
        {
            foreach (var v in si.SpellResources.CurrentSpellCost)
            {
                spellResourceManager.UseResources(v.Key, v.Value);
            }

            si.Charges--;
            StartSpellCooldown(si);
        }

        public delegate void NewSpellIsCreated(BaseSpell newSpell);
        public event NewSpellIsCreated OnNewSpellIsCreated;

        private void LaunchAux_LaunchV1(LaunchArgs args) // public bool Launch(Spells.SpellInfo si)
        {
            BaseSpell spell = (GameObject.Instantiate(args.spellInfo.spellActive, Vector3.zero, Quaternion.identity) as GameObject).GetComponentInChildren<BaseSpell>();
            spell.Caster = caster;

            if (OnNewSpellIsCreated != null)
                OnNewSpellIsCreated.Invoke(spell);
        }

        private void LaunchAux_LaunchV2(LaunchArgs args) // public bool Launch(Spells.SpellInfo si, Vector3 pos)
        {
            BaseSpell spell = (GameObject.Instantiate(args.spellInfo.spellActive, args.singlePos.Value, Quaternion.identity) as GameObject).GetComponentInChildren<BaseSpell>();
            spell.SpellPos = args.singlePos.Value;
            spell.Caster = caster;

            if (OnNewSpellIsCreated != null)
                OnNewSpellIsCreated.Invoke(spell);
        }

        private void LaunchAux_LaunchV3(LaunchArgs args) // public bool Launch(Spells.SpellInfo si, GameObject[] targets)
        {
            foreach (GameObject go in args.targets)
            {
                BaseSpell spell;

                if (args.spellInfo.isBuff)
                {
                    spell = go.GetComponent<BuffManager>().AddBuff(args.spellInfo.spellActive, caster, go).GetComponent<BaseSpell>();
                }
                else
                {
                    spell = (GameObject.Instantiate(args.spellInfo.spellActive, caster.transform.position, Quaternion.identity) as GameObject).GetComponentInChildren<BaseSpell>();
                    spell.Caster = caster;
                    spell.SpellTargets.Add(go);
                }

                if (OnNewSpellIsCreated != null)
                    OnNewSpellIsCreated.Invoke(spell);
            }
        }

        private void LaunchAux_LaunchV4(LaunchArgs args) // public bool Launch(Spells.SpellInfo si, GameObject[] targets, Vector3[] pos)
        {
            for (int i = 0; i < args.targets.Length; i++)
            {
                BaseSpell spell = (GameObject.Instantiate(args.spellInfo.spellActive, args.eachTargetPos[i], Quaternion.identity) as GameObject).GetComponentInChildren<BaseSpell>();
                spell.Caster = caster;
                spell.SpellTargets.Add(args.targets[i]);

                if (OnNewSpellIsCreated != null)
                    OnNewSpellIsCreated.Invoke(spell);
            }
        }

        /// <summary>
        /// be sure the spell is available.
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public e_LaunchReturn LaunchAux_IsInRange(string spellId, GameObject caster, Vector3 requiredSpellPosition)
        {
            SpellInfo si = getSpellInfoFromId(spellId);
            if (si == null)
            {
                Debug.LogError("no such spell available in this Launcher. Spell : " + spellId);
            }

            var deltaDistance = Vector3.Distance(caster.transform.position, requiredSpellPosition);

            return ((!si.useRange || (deltaDistance >= si.MinRange && deltaDistance <= si.MaxRange)) ? e_LaunchReturn.ok : e_LaunchReturn.NotInRange);
        }

        /// <summary>
        /// wait for the casting time, then launch the spell
        /// calling functions and its args are in the helper
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        private IEnumerator Co_Launch(LaunchHelper helper)
        {
            // player may not have ressources after the casting, so it should not be able to cast the spell.
            // there should be a callback attached to OnManaChange or something like that, that stop the casting if there is no more resources.
            // in the same time, he might have get them back at the end of the cast, so it should not be a good idea to stop the casting.

            //IsLaunching = true;

            //_animator.SetBool("Casting", true);
            //yield return new WaitForSeconds(helper.args.spellInfo.CastingTime);
            //_animator.SetBool("Casting", false);
            if (LaunchAux_CanLaunch(helper.args.spellInfo) != e_LaunchReturn.ok)
                yield break;

            helper.launchFunction(helper.args);

            LaunchAux_ConsumeResourcesAndCharge(helper.args.spellInfo);
            IsLaunching = false;
        }

        /// <summary>
        /// this class holds the function the coroutine has to call and its args
        /// </summary>
        private class LaunchHelper
        {
            public delegate void Void_D_LaunchArgs(LaunchArgs args);
            public Void_D_LaunchArgs launchFunction;
            public LaunchArgs args;

            public LaunchHelper(Void_D_LaunchArgs launchFunction_, LaunchArgs args_)
            {
                launchFunction = launchFunction_;
                args = args_;
            }
        }

        /// <summary>
        /// this class holds the args for the differents version of the Coroutine auxilieary function
        /// </summary>
        private class LaunchArgs
        {
            public Spells.SpellInfo spellInfo;
            public Vector3? singlePos;
            public GameObject[] targets;
            public Vector3[] eachTargetPos;

            public LaunchArgs(Spells.SpellInfo si_)
            {
                spellInfo = si_;
            }

            public LaunchArgs(Spells.SpellInfo si_, Vector3? singlePos_)
            {
                spellInfo = si_;
                singlePos = singlePos_;
            }

            public LaunchArgs(Spells.SpellInfo si_, GameObject[] targets_)
            {
                spellInfo = si_;
                targets = targets_;
            }

            public LaunchArgs(Spells.SpellInfo si_, GameObject[] targets_, Vector3[] eachTargetPos_)
            {
                spellInfo = si_;
                targets = targets_;
                eachTargetPos = eachTargetPos_;
            }
        }

        #endregion

        #region Cooldown related stuff

        private void StartSpellCooldown(Spells.SpellInfo si)
        {
            GameObject cdHolder = new GameObject();
            cdHolder.transform.parent = caster.transform;
            CooldownManager cd = cdHolder.AddComponent<CooldownManager>();

            cd.StartCooldown(new Spells.StartCooldownData(si.Id, si.Cooldown, OnEndSpellCooldown));
            _currentCooldowns.Add(cd);

            if (si.isPassiveWhileNotOnCooldown && si.Charges == 0)
            {
                si.PassiveEffectInstance.CurrentState = ST_EffectOnActivation.e_State.DISABLED;
            }
        }

        private void OnEndSpellCooldown(CooldownManager cdm)
        {
            _currentCooldowns.Remove(cdm);

            SpellInfo si = _availableSpells.SingleOrDefault(x => x.Id == cdm.SpellId);

            if (si != null)
            {
                si.Charges++;

                if (si.isPassiveWhileNotOnCooldown)
                {
                    Spells.ST_EffectOnActivation effect = si.PassiveEffectInstance;
                    if (effect.CurrentState != ST_EffectOnActivation.e_State.ENABLED) // may already be enabled if the spell has several charges
                    {
                        si.PassiveEffectInstance.CurrentState = ST_EffectOnActivation.e_State.ENABLED;
                    }
                }
            }
        }

        public CooldownManager GetSpecificCooldown(string spellId)
        {
            return _currentCooldowns.SingleOrDefault(x => x.SpellId == spellId);
        }

        #endregion

        #region convenient functions

        public float GetSpellMaxRangeByIndex(int index)
        {
            return (AvailableSpells[index].MaxRange);
        }
        public float GetSpellMinRangeByIndex(int index)
        {
            return (AvailableSpells[index].MinRange);
        }

        public string GetSpellIDByIndex(int index)
        {
            return (AvailableSpells[index].Id);
        }

        public Spells.SpellInfo.e_CastType GetSpellCastTypeByIndex(int index)
        {
            return (AvailableSpells[index].CastType);
        }

        public bool IsSpellInCooldown(int index)
        {
            return AvailableSpells[index].Charges > 0;
        }

        public bool IsSpellInCooldown(string id)
        {
            return AvailableSpells.SingleOrDefault(x => x.Id == id).Charges > 0;
        }

        public bool HasSpellRessources(int index)
        {
            return LaunchAux_CanLaunch(AvailableSpells[index]) != e_LaunchReturn.NotEnoughResources;
        }

        public bool IsSpellInRange(int index, Vector3 from, Vector3 to)
        {
            SpellInfo si = AvailableSpells[index];
            if (!si.useRange)
                return true;

            float distance = Mathf.Abs(Vector3.Distance(from, to));
            return distance >= si.MinRange && distance < si.MaxRange;
        }

        #endregion

        public Spells.SpellInfo getSpellInfoFromId(string id)
        {
            return AvailableSpells.SingleOrDefault(x => x.Id == id);
        }

        private void OnAvailableSpellIsModified(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:

                // setting remaining cooldown for this spell (if it has any)
                Spells.SpellInfo newSpell = e.NewItems[0] as Spells.SpellInfo;

                int activeCooldown = _currentCooldowns.Count(x => x.SpellId == newSpell.Id);

                newSpell.Charges -= activeCooldown;

                // setting passive effect, if the spell is passive or is passive when reloading
                if (newSpell.isPassive || newSpell.isPassiveWhileNotOnCooldown)
                {
                    if (newSpell.passiveEffect.GetComponentInChildren<ST_Aura>(true))
                    {
                        var aura = GameObject.Instantiate(newSpell.passiveEffect);
                        aura.transform.SetParent(this.transform);
                        aura.transform.localPosition = Vector3.zero;
                        Spells.BaseSpell baseSpell = aura.GetComponentInChildren<Spells.BaseSpell>();
                        baseSpell.Caster = caster;
                    }
                    else
                    {
                        Spells.BuffManager bm = this.GetComponent<Spells.BuffManager>();
                        var buff = bm.AddBuff(newSpell.passiveEffect, caster, caster);

                        //if (OnNewSpellIsCreated != null)
                        //    OnNewSpellIsCreated(buff._baseSpell);

                        //buff.BuffId = newSpell.Id;
                        newSpell.PassiveEffectInstance = buff;

                        if (newSpell.isPassiveWhileNotOnCooldown && newSpell.Charges == 0)
                        {
                            newSpell.PassiveEffectInstance.CurrentState = ST_EffectOnActivation.e_State.DISABLED; // may have been enable during creation
                        }
                    }
                }
                // managing toogles
                else if (newSpell.isToogled)
                {
                    newSpell.MaxCharges = 1;
                    newSpell.Charges = 1;

                    Spells.ST_ToogleOnActivation toogle = (GameObject.Instantiate(newSpell.passiveEffect) as GameObject).GetComponentInChildren<Spells.ST_ToogleOnActivation>();

                    //if (OnNewSpellIsCreated != null)
                    //    OnNewSpellIsCreated(toogle._baseSpell);

                    newSpell.BaseCooldown = toogle.TicInterval;
                    newSpell.Cooldown = toogle.TicInterval;

                    Spells.BaseSpell toogleBaseSpell = toogle.GetComponent<BaseSpell>();
                    toogleBaseSpell.Caster = caster;

                    toogleBaseSpell.OnTic.AddListener(() =>
                        {
                            newSpell.Charges--;
                            StartSpellCooldown(newSpell);
                        });


                    toogle.transform.root.parent = caster.transform;
                    toogle.transform.parent.localPosition = Vector3.zero;

                    toogle.SpellResources = newSpell.SpellResources; // this one to know what to use.
                    toogle.SpellResourcesManager = spellResourceManager; // this one to effectively use resources.

                    newSpell.PassiveEffectInstance = toogle;
                }
                break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:

                Spells.SpellInfo oldSpell = e.OldItems[0] as Spells.SpellInfo;

                // we have to remove its effect, if it has any
                if (oldSpell.PassiveEffectInstance != null)
                {
                    if (!oldSpell.isToogled)
                    {
                        this.GetComponent<Spells.BuffManager>().RemoveBuff(oldSpell.PassiveEffectInstance as Spells.ST_Buff);
                    }
                    oldSpell.GetComponent<BaseSpell>().End();
                    GameObject.Destroy(oldSpell.PassiveEffectInstance);
                }
                break;

                default:
                break;

            }
        }
    }
}