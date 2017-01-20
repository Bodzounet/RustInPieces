using UnityEngine;
using System.Collections;

public class AvoidAI : IState
{
    [SerializeField]
    private float fov = 180f;
    [SerializeField]
    private float dov = 4.5f;

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
        UnitInfo info = _manager.getFurthestAlly(_entity.Team, 360);
        if (info == null || info.distance > dov * 2)
        {
            _agent.Stop();
            return;
        }
        Vector3 norm = (info.go.transform.position - _target.transform.position).normalized * 4;
        _agent.SetDestination(new Vector3(norm.x + transform.position.x, transform.position.y, norm.z + transform.position.z));
        if (_anim != null)
            _anim.SetBool("IsWalking", true);
        _manager.FrozeAI(0.2f);
    }

    public override bool isTrigger()
    {
        UnitInfo info = _manager.getClosestEnemy(_entity.Team, fov);
        UnitInfo ally = _manager.getFurthestAlly(_entity.Team, 360);
        if (info == null || info.distance > dov || ally == null)
        {
            _target = null;
            return false;
        }
        _target = info.go;
        return true;
    }
}
