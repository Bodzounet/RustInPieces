using UnityEngine;
using System.Collections;
using System;

public class BasicAttackDragonAI : IState
{
    [SerializeField]
    private float fov = 180f;
    [SerializeField]
    private float dov = 1.5f;

    private SphereCollider col;
    private GameObject _target = null;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<SphereCollider>();
        if (col.radius < dov)
            col.radius = dov;
    }

    public override void updateState()
    {
        if (_agent != null)
            _agent.Stop();
        StartCoroutine("LaunchBasicAttack", 1.5f);
        if (_anim != null)
        {
            transform.parent.LookAt(_target.transform);
            _anim.SetBool("IsWalking", false);
            _anim.Play("Attack");
        }

    }

    IEnumerator LaunchBasicAttack(float time)
    {
        yield return new WaitForSeconds(time);

        if (_launcher.Launch(_launcher.GetSpellIDByIndex(0), new GameObject[] { _target }, new Vector3[] { transform.position }) == Spells.SpellLauncher.e_LaunchReturn.ok)
        {
            //transform.parent.eulerAngles = new Vector3(0, transform.parent.transform.rotation.y, transform.parent.transform.rotation.z);
        }
    }
    public override bool isTrigger()
    {
        UnitInfo info = _manager.getBestEnemy(_entity.Team, fov, dov);
        if (info == null)
        {
            _target = null;
            return false;
        }
        _target = info.go;
        return true;
    }
}
