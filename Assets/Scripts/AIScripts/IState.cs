using UnityEngine;
using System.Collections;
using System;

public abstract class IState: MonoBehaviour
{
    [SerializeField]
    protected int priority;

    protected Entity _entity;
    protected AIManager _manager;
    protected NavMeshAgent _agent;
    protected Spells.SpellLauncher _launcher;
    protected Animator _anim;

    void OnDeath(GameObject self)
    {
		if (_anim != null)
			_anim.SetTrigger("Death");
    }

    void OnHit(float before, float after, Entity.e_StatType type)
    {
		if (_anim != null)
            if (before > after && type == Entity.e_StatType.HP_CURRENT)
                _anim.SetTrigger("TakeDamage");
    }

    protected virtual void Awake()
    {
        _anim = transform.parent.GetComponentInChildren<Animator>();
        _launcher = GetComponentInParent<Spells.SpellLauncher>();
        _manager = GetComponent<AIManager>();
        _entity = GetComponentInParent<Entity>();
        _agent = GetComponentInParent<NavMeshAgent>();
        _entity.OnDeath += OnDeath;
        _entity.OnHit += OnHit;
    }

    public int Priority
    {
        get
        {
            return priority;
        }
        set
        {
            priority = value;
            _manager.updateStates();
        }
    }

    public abstract void updateState();

    public abstract bool isTrigger();
}

