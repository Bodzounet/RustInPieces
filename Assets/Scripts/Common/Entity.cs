using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Void_D_Void();
public delegate void Void_D_GameObject(GameObject go);
public delegate void Void_D_IntInt(int i1, int i2);
public delegate void Void_D_KDA(KDA i);
public delegate void Void_D_EntityFloat(Entity ent, float dmg);
public delegate void Void_D_Int(int i);
public delegate void Void_D_FloatFloat(float f1, float f2);
public delegate void Void_D_FloatFloatType(float f1, float f2, Entity.e_StatType type);
public delegate void Void_D_TeamTeam(e_Team t1, e_Team t2);
public delegate void Void_D_ArmorTypeArmorType(Entity.e_ArmorType at1, Entity.e_ArmorType at2);
public delegate void Void_D_AttackTypeAttackType(Entity.e_AttackType at1, Entity.e_AttackType at2);
public delegate void Void_D_AttackTypeFloatFloat(Entity.e_AttackType atVal, float f1, float f2);
public delegate float Float_D_FloatFloat(float f1, float f2);
public delegate float Float_D_Float(float f);
public delegate float Float_D_FloatAttackType(float f, Entity.e_AttackType type);
public delegate void Void_D_Float(float remaining);

public enum e_Team
{
    TEAM1,
    TEAM2,
    NEUTRAL
};

[System.Serializable]
public class StatValue
{
    public Entity.e_StatType type;
    public float value;

    public StatValue(Entity.e_StatType Stype, float Svalue)
    {
        type = Stype;
        value = Svalue;
    }
}

public class Entity : MonoBehaviour
{
    #region enums
    [System.Serializable]
    public enum e_ArmorType
    {
        LIGHT,
        MEDIUM,
        HEAVY
    };

    public enum e_AttackType
    {
        MELEE,
        RANGE,
        MAGIC,
        NEUTRAL
    };

    public enum e_StatType
    {
        MELEE_ATT = 0,
        RANGE_ATT,
        MAGIC_ATT,
        DEFENSE_PEN,
        SPEED,
        DEFENSE,
        ATTACK_SPEED,
        HP_MAX,
        MANA_MAX,
        HP_CURRENT,
        MANA_CURRENT,
        RANGE,
        REGEN_HP,
        REGEN_MANA,
        FOCUS,
        FOCUS_STACKS,
        LIFE_DRAIN,
    }

    public enum e_EntityState
    {
        STUN, // No auto-attacks, spell casting or moving can be done
        SILENCE, // No spells can be cast
        ROOT, // No movement allowed
        BLIND, // No auto-attacks can be launched;
        BLOCK // Will block and destroy the next spell hitting it
    }

    public enum e_StatOperator
    {
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE
    }
    #endregion

    #region events

    // DO NOT IMPLEMENT DAMAGE BEHAVIOR IN CALLBACK YOU REGISTER TO THOSE EVENTS
    // There is one callback for each attribute, use those callbacks to get informed if this attribute has been modified.
    // otherwise, use function dedicated to attribute modification.

    private Dictionary<Entity.e_StatType, Void_D_FloatFloat> _statsCallbacks = new Dictionary<Entity.e_StatType, Void_D_FloatFloat>();

    public void addCallbackStat(Entity.e_StatType type, Void_D_FloatFloat callback)
    {
        _statsCallbacks[type] += callback;
    }

    public void removeCallbackStat(Entity.e_StatType type, Void_D_FloatFloat callback)
    {
        _statsCallbacks[type] -= callback;
    }

    private Dictionary<Entity.e_EntityState, Void_D_Float> _stateCallbacks = new Dictionary<e_EntityState, Void_D_Float>();

    public void addCallbackState(Entity.e_EntityState type, Void_D_Float callback)
    {
        _stateCallbacks[type] += callback;
    }
    public void removeCallbackState(Entity.e_EntityState type, Void_D_Float callback)
    {
        _stateCallbacks[type] -= callback;
    }

    public event Void_D_FloatFloatType OnHit; // Called when the entity receives a hit
    public event Void_D_EntityFloat OnDoDamage; // Called when the entity deals damages or "hit" a spell.
    public event Void_D_ArmorTypeArmorType OnArmorTypeChanged;
    public event Void_D_AttackTypeFloatFloat OnShieldChanged;

    public event Void_D_GameObject OnDeath;        // go : the dying Entity
	public event Void_D_GameObject OnRespawn;        // go : the dying Entity

	public event Void_D_TeamTeam OnTeamChanged;    // t : the new team. Strategist Manager is easy to make all requiered change after.

    #endregion

    #region attribute modifier callback stacks
    // those dictionnaries hold specific effects, sorted by their application priority.

    public SortedDictionary<int, Float_D_FloatAttackType> DamageAmplification = new SortedDictionary<int, Float_D_FloatAttackType>();
    public SortedDictionary<int, Float_D_FloatAttackType> DamageReduction = new SortedDictionary<int, Float_D_FloatAttackType>();

    private Dictionary<Entity.e_StatType, SortedDictionary<int, Float_D_FloatFloat>> _buffDict = new Dictionary<Entity.e_StatType, SortedDictionary<int, Float_D_FloatFloat>>();
    private Dictionary<Entity.e_StatType, SortedDictionary<int, Float_D_FloatFloat>> _tmpBuff = new Dictionary<Entity.e_StatType, SortedDictionary<int, Float_D_FloatFloat>>();

    public void addBuff(Entity.e_StatType type, int priority, Float_D_FloatFloat callback)
    {
        _buffDict[type].Add(priority, callback);
    }

    public void removeBuff(Entity.e_StatType type, int priority)
    {
        _buffDict[type].Remove(priority);
    }
    #endregion

    #region attribute modifier functions

    public void didDamagesTo(Entity other, float damagesDone)
    {
        if (OnDoDamage != null)
            OnDoDamage(other, damagesDone);
    }

    /// <summary>
    /// This function may be used to calculate ratio of offensive stats for spell damages, heal or whatever you want.
    /// </summary>
    /// <param name="type">Type cannot be NEUTRAL</param>
    /// <param name="ratio">Float percentage as 1.f equal 100%</param>
    /// <returns></returns>
    public float getPercentageOf(Entity.e_AttackType type, float ratio)
    {
        float value;
        switch (type)
        {
            case e_AttackType.MELEE:
                value = ratio * _stats[Entity.e_StatType.MELEE_ATT];
                break;
            case e_AttackType.MAGIC:
                value = ratio * _stats[Entity.e_StatType.MAGIC_ATT];
                break;
            case e_AttackType.RANGE:
                value = ratio * _stats[Entity.e_StatType.RANGE_ATT];
                break;
            default:
                value = 0;
                break;
        }
        return value;
    }

    /// <summary>
    /// Apply the entity's damage amplification callbacks to the initials damages
    /// </summary>
    /// <param name="initDamages">Damage value</param>
    /// <param name="type">Damage type</param>
    /// <returns>Return the potentially modified damage value</returns>
    public float buffDamages(float initDamages, Entity.e_AttackType type)
    {
        foreach (KeyValuePair<int, Float_D_FloatAttackType> entry in DamageAmplification)
        {
            initDamages = entry.Value(initDamages, type);
        }
        return initDamages;
    }

    /// <summary>
    /// Same as buffDamages except that it updates and uses the focus system.s
    /// </summary>
    /// <param name="initDamages">Damage value</param>
    /// <param name="type">Damage type</param>
    /// <returns>Potentially modified damage value</returns>
    public float BADamageBuff(float initDamages, Entity.e_AttackType type)
    {
        if (getStat(e_StatType.FOCUS_STACKS) >= 100)
        {
            setStat(e_StatType.FOCUS_STACKS, 0);
            initDamages *= 1.5f;
        }
        initDamages = buffDamages(initDamages, type);
        return (initDamages);
    }

    public float GetFinalDamage(float baseDamage, float damageRatio, e_AttackType type)
    {
        float damages = this.buffDamages(baseDamage + this.getPercentageOf(type, damageRatio), type);

        return (damages);
    }

    public void lifeDrain(Entity entityTouched, float damagesDealt)
    {
        float drained = damagesDealt * getStat(Entity.e_StatType.LIFE_DRAIN) / 100;
        modifyStat(Entity.e_StatType.HP_CURRENT, e_StatOperator.ADD, drained, this);
    }

    private float useShield(Entity.e_AttackType type, float initDamages)
    {
        if (_shieldDict[type] > 0 && _shieldDict[type] >= initDamages)
        {
            _shieldDict[type] -= initDamages;
            initDamages = 0;
        }
        else if (_shieldDict[type] > 0 && _shieldDict[type] < initDamages)
        {
            initDamages -= _shieldDict[type];
            _shieldDict[type] = 0;
        }
        return initDamages;
    }

    /// <summary>
    /// This function apply damages to an entity's HP, before that, it applies all damage reduction's callbacks and uses potential shields on the recipient entity.
    /// </summary>
    /// <param name="initDamages">Damage value</param>
    /// <param name="type">Damage type</param>
    /// <param name="otherEnt">Entity who's responsible for the damage</param>
    /// <param name="isCallback">Whether to enable callbacks or not on this modification</param>
    public void doDamages(float initDamages, Entity.e_AttackType type, Entity otherEnt, bool isCallback = true)
    {
        foreach (KeyValuePair<int, Float_D_FloatAttackType> entry in DamageReduction)
        {
            initDamages = entry.Value(initDamages, type);
        }
        initDamages = useShield(type, initDamages);
        initDamages = useShield(Entity.e_AttackType.NEUTRAL, initDamages);
        //Debug.Log("DMG = " + initDamages);
        modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, initDamages, otherEnt, isCallback);
        if (otherEnt != null)
            otherEnt.didDamagesTo(this, initDamages);
    }

    private float baseReduction(float damages, Entity.e_AttackType type)
    {
        float reducDef = (1 - (Mathf.Log(_stats[Entity.e_StatType.DEFENSE] + 1) / 10)) * _defenseAttackChart[type][_armorType];
        //Debug.Log("DAMAGES REDUCTION = " + reducDef + " due to " + _stats[Entity.e_StatType.DEFENSE] + " defense with a multiplier of " + _defenseAttackChart[type][_armorType]);
        return damages * reducDef;
    }

    private float checkDeath(float bfr, float aft)
    {
        if (bfr > 0 && aft <= 0 && OnDeath != null)
        {
            OnDeath(this.gameObject);
			if (this is HeroEntity == false)
				Invoke("DestroySelf", 0.8f);
			else
				Invoke("Respawn", 8.0f);
        }
        return aft;
    }

	private void Respawn()
	{
		OnRespawn(this.gameObject);
	}

    private void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }

    private float maxOutHp(float before, float after)
    {
        if (after > getStat(Entity.e_StatType.HP_MAX))
            return getStat(Entity.e_StatType.HP_MAX);
        return after;
    }
    private float maxOutMana(float before, float after)
    {
        if (after > getStat(Entity.e_StatType.MANA_MAX))
            return getStat(Entity.e_StatType.MANA_MAX);
        return after;
    }

    private void updateStateRemainingTime()
    {
        foreach (Entity.e_EntityState state in System.Enum.GetValues(typeof(Entity.e_EntityState)))
        {
            if (_stateRemainingTime[state] > 0f)
            {
                _stateRemainingTime[state] -= Time.deltaTime;
                if (_stateRemainingTime[state] < 0f)
                {
                    _stateRemainingTime[state] = 0f;
                }
                if (_stateCallbacks[state] != null)
                    _stateCallbacks[state](_stateRemainingTime[state]);
            }
        }
    }

    private IEnumerator regenStats()
    {
        while (true)
        {
            if (_stats[Entity.e_StatType.HP_CURRENT] < _stats[Entity.e_StatType.HP_MAX])
            {
                modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.ADD, _stats[Entity.e_StatType.REGEN_HP], this);
            }
            if (_stats[Entity.e_StatType.MANA_CURRENT] < _stats[Entity.e_StatType.MANA_MAX])
            {
                modifyStat(Entity.e_StatType.MANA_CURRENT, Entity.e_StatOperator.ADD, _stats[Entity.e_StatType.REGEN_MANA], this);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    #endregion

    #region properties

    [SerializeField]
    private List<StatValue> _serializedStats = new List<StatValue>();

    public float editorGetStat(Entity.e_StatType type)
    {
        foreach (StatValue stat in _serializedStats)
        {
            if (stat.type == type)
                return stat.value;
        }
        _serializedStats.Add(new StatValue(type, -1));
        return -1;
    }

    public void editorSetStat(Entity.e_StatType type, float value)
    {
        foreach (StatValue stat in _serializedStats)
        {
            if (stat.type == type)
                stat.value = value;
        }
    }

    private Dictionary<Entity.e_StatType, float> _stats = new Dictionary<Entity.e_StatType, float>();

    public float getStat(Entity.e_StatType type)
    {
        return _stats[type];
    }

    /// <summary>
    /// Directly set the stat's value for the entity. You shouldn't use that except for particular behaviour.
    /// Only GUI callbacks will be applied.
    /// </summary>
    /// <param name="type">Type of the modified stat</param>
    /// <param name="value">New value</param>
    public void setStat(Entity.e_StatType type, float value)
    {
        editorSetStat(type, value);
        var before = _stats[type];
        _stats[type] = value;
        foreach (var entry in _buffDict[type])
        {
            if (_tmpBuff[type].ContainsValue(entry.Value) == false)
            {
                _stats[type] = entry.Value(before, _stats[type]);
                _tmpBuff[type][entry.Key] = entry.Value;
            }
        }

        editorSetStat(type, _stats[type]);
        if (_statsCallbacks[type] != null)
            _statsCallbacks[type](before, value);
    }

    /// <summary>
    /// Main modification function for any entity's stat.
    /// </summary>
    /// <param name="type">Stat type</param>
    /// <param name="op">Operation type</param>
    /// <param name="value">Value to be operated</param>
    /// <param name="mofidier">The entity reponsible for the modification (often itself, used for callbacks)</param>
    /// <param name="isCallback">Whether the stat's callbacks should be called or not</param>
    public void modifyStat(Entity.e_StatType type, Entity.e_StatOperator op, float value, Entity modifier, bool isCallback = true)
    {
        this.Hitter = modifier;
        value *= _statBoost[type];
        var before = _stats[type];
        switch (op)
        {
            case e_StatOperator.ADD:
                _stats[type] += value;
                break;
            case e_StatOperator.SUBTRACT:
                _stats[type] -= value;
                break;
            case e_StatOperator.MULTIPLY:
                _stats[type] *= value;
                break;
            case e_StatOperator.DIVIDE:
                _stats[type] /= value;
                break;
        };
        if (isCallback)
        {
            foreach (var entry in _buffDict[type])
            {
                if (_tmpBuff[type].ContainsValue(entry.Value) == false)
                {
                    _stats[type] = entry.Value(before, _stats[type]);
                    _tmpBuff[type][entry.Key] = entry.Value;
                }
            }
        }
        editorSetStat(type, _stats[type]);
        if (_statsCallbacks[type] != null)
            _statsCallbacks[type](before, _stats[type]);
        _tmpBuff = new Dictionary<e_StatType, SortedDictionary<int, Float_D_FloatFloat>>();
        foreach (e_StatType stat in System.Enum.GetValues(typeof(e_StatType)))
        {
            _tmpBuff[stat] = new SortedDictionary<int, Float_D_FloatFloat>();
        }
        if (OnHit != null)
            OnHit(before, _stats[type], type);
    }

    private Dictionary<Entity.e_StatType, float> _statBoost = new Dictionary<Entity.e_StatType, float>();

    public float getStatBoost(Entity.e_StatType type)
    {
        return _statBoost[type];
    }

    /// <summary>
    /// Stat boost are multiplier of regular stats. Every statBoost starts at 1.0f. Everytime a stats is modified the statBoost is also applied.
    /// This function modifies the current stats with the new statBoost.
    /// </summary>
    /// <param name="type">Stat type</param>
    /// <param name="value">Value to be added or removed (negative value)</param>
    public void addStatBoostValue(Entity.e_StatType type, float value)
    {
        var before = _stats[type];
        var baseValue = before / _statBoost[type];
        _statBoost[type] += value;
        setStat(type, baseValue * _statBoost[type]);
    }

    private Dictionary<Entity.e_AttackType, float> _shieldDict = new Dictionary<Entity.e_AttackType, float>()
        {
            {Entity.e_AttackType.MELEE, 0},
            {Entity.e_AttackType.MAGIC, 0},
            {Entity.e_AttackType.RANGE, 0},
            {Entity.e_AttackType.NEUTRAL, 0}
        };

    public float getShieldValue(Entity.e_AttackType type)
    {
        return _shieldDict[type];
    }

    public void addShieldValue(Entity.e_AttackType type, float value)
    {
        var before = _shieldDict[type];
        _shieldDict[type] += value;
        if (OnShieldChanged != null)
            OnShieldChanged(type, before, value);
    }

    [SerializeField]
    private int _goldWorth = 10000;

    public int GoldWorth
    {
        get { return _goldWorth; }
        set { _goldWorth = value; }
    }



	[SerializeField]
    private Entity.e_ArmorType _armorType;
    public Entity.e_ArmorType ArmorType
    {
        get { return _armorType; }
        set
        {
            var before = _armorType;
            _armorType = value;
            if (OnArmorTypeChanged != null)
                OnArmorTypeChanged(before, value);
        }
    }

    [SerializeField]
    private e_Team _team = e_Team.NEUTRAL;
    public e_Team Team
    {
        get { return _team; }
        set
        {
            e_Team tmp = _team;
            _team = value;
            if (OnTeamChanged != null)
                OnTeamChanged(tmp, _team);
        }
    }

	private Dictionary<Entity, float> _lastHitters;
	private float _assistTime = 10f;

	private Entity _hitter = null;

	public Dictionary<Entity, float> LastHitters
	{
		get
		{
			return _lastHitters;
		}
	}

	public Entity Hitter
    {
        get
        {
            return _hitter;
        }
		set
		{
			if (value != this)  //Ici on enregistre le hit pour l'assist
			{
                if (value != null)
                {
                    float time = Time.realtimeSinceStartup;
                    if (_lastHitters.ContainsKey(value))
                    {
                        _lastHitters[value] = time;
                    }
                    else
                    {
                        _lastHitters.Add(value, time);
                    }

                    List<Entity> _toRemove = new List<Entity>();

                    foreach (var e in _lastHitters)
                    {

                        if (time - e.Value > _assistTime)
                        {
                            _toRemove.Add(e.Key);
                        }
                    }


                    foreach (Entity e in _toRemove)
                    {
                        _lastHitters.Remove(e);
                    }
                    // fin de l'assist

                }
				bool changed = _hitter == value;
				_hitter = value;
			}
		}
	}
    [SerializeField]
    private string _entityName = null; // let this empty to be fill by the script name, which is HIGHLY recommanded.
    public string EntityName
    {
        get { return _entityName; }
        set { _entityName = value; }
    }

    private Dictionary<Entity.e_EntityState, float> _stateRemainingTime = new Dictionary<e_EntityState, float>();

    public float getRemainingStateTime(Entity.e_EntityState type)
    {
        return _stateRemainingTime[type];
    }

    public void setRemainingStateTime(Entity.e_EntityState type, float value)
    {
        _stateRemainingTime[type] = value;
    }

    public void addStateTime(Entity.e_EntityState type, float value)
    {
        _stateRemainingTime[type] += value;
        if (_stateCallbacks[type] != null)
            _stateCallbacks[type](_stateRemainingTime[type]);
    }

    private Dictionary<Entity.e_AttackType, Dictionary<Entity.e_ArmorType, float>> _defenseAttackChart = new Dictionary<Entity.e_AttackType, Dictionary<Entity.e_ArmorType, float>>()
        {
            {Entity.e_AttackType.MELEE, new Dictionary<Entity.e_ArmorType, float>() { {Entity.e_ArmorType.LIGHT, 1.3f}, {Entity.e_ArmorType.MEDIUM, 1.0f}, {Entity.e_ArmorType.HEAVY, 0.7f} }},
            {Entity.e_AttackType.MAGIC, new Dictionary<Entity.e_ArmorType, float>() { {Entity.e_ArmorType.LIGHT, 1.0f}, {Entity.e_ArmorType.MEDIUM, 0.7f}, {Entity.e_ArmorType.HEAVY, 1.3f} }},
            {Entity.e_AttackType.RANGE, new Dictionary<Entity.e_ArmorType, float>() { {Entity.e_ArmorType.LIGHT, 0.7f}, {Entity.e_ArmorType.MEDIUM, 1.3f}, {Entity.e_ArmorType.HEAVY, 1.0f} }},
            {Entity.e_AttackType.NEUTRAL, new Dictionary<Entity.e_ArmorType, float>() { {Entity.e_ArmorType.LIGHT, 1.0f}, {Entity.e_ArmorType.MEDIUM, 1.0f}, {Entity.e_ArmorType.HEAVY, 1.0f} }}
        };

    #endregion

    protected virtual void Awake()
    {
        _tmpBuff = new Dictionary<e_StatType, SortedDictionary<int, Float_D_FloatFloat>>();
        foreach (StatValue stat in _serializedStats)
        {
            _tmpBuff[stat.type] = new SortedDictionary<int, Float_D_FloatFloat>();
            _stats[stat.type] = stat.value;
            _statsCallbacks[stat.type] = null;
            _statBoost[stat.type] = 1.0f;
            _buffDict[stat.type] = new SortedDictionary<int, Float_D_FloatFloat>();
        }
        foreach (Entity.e_EntityState state in System.Enum.GetValues(typeof(Entity.e_EntityState)))
        {
            _stateRemainingTime[state] = 0f;
            _stateCallbacks[state] = null;
        }
        _buffDict[e_StatType.HP_CURRENT].Add(9999, maxOutHp);
        _buffDict[e_StatType.MANA_CURRENT].Add(9999, maxOutMana);
        _buffDict[e_StatType.HP_CURRENT].Add(10000, checkDeath);
        OnDoDamage += lifeDrain;
        DamageReduction[0] = baseReduction;
        if (_entityName == null)
            _entityName = this.gameObject.name;
        OnDeath += GiveGold;
		_lastHitters = new Dictionary<Entity, float>();
	}

    protected virtual void Start()
    {
        StartCoroutine("regenStats");
    }

    protected virtual void Update()
    {
        updateStateRemainingTime();
    }

    ///
    /// here is solution for the OnDeath.
    /// bugs appear if we try to instanciate stuff on the OnDestroy and the Application is quitting, or another scene is loading.
    /// here it may prevent bugs to appear when the player leave, but not when the Scene change. it may be a big problem at the end of a match.
    /// it must be fixed before the FPP.
    ///

    bool _applicationIsQuitting = false;
    void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }


    void GiveGold(GameObject e)
    {
        if (Hitter != null)
        {

            if (Hitter is HeroEntity)
            {
                int money = GoldWorth;//(int)getStat(e_StatType.GOLD_REWARD);
                ((HeroEntity)Hitter).Gold += money;
            }

			foreach (var h in _lastHitters)  //Et la c'est assists
			{
				if (h.Key.Team != this.Team)
				{
					if (h.Key != Hitter)
					{
						if (h.Key is HeroEntity)
						{
							((HeroEntity)h.Key).Gold += GoldWorth / 2;
						}
					}
				}
			}

		}
    }
}


