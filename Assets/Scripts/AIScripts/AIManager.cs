using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class UnitInfo : System.IEquatable<UnitInfo>
{
    public Entity entity;
    public GameObject go;
    public float angle;
    public float distance;
    public UnitsInfo.e_UnitType unitType;

    public UnitInfo(Entity ent, GameObject go, float angle, float distance, UnitsInfo.e_UnitType unit)
    {
        this.entity = ent;
        this.angle = angle;
        this.distance = distance;
        this.go = go;
        this.unitType = unit;
    }

    public bool Equals(UnitInfo other)
    {
       if (other == null)
           return false;
       return (this.go.Equals(other.go));
    }
}

[RequireComponent(typeof(Collider))]
public class AIManager : MonoBehaviour
{
    private List<IState> _stateList;
    private List<UnitInfo> _unitList = new List<UnitInfo>();
    private float _freezeAI = 0;

    void Awake()
    {
        updateStates();
    }

    public void updateStates()
    {
        _stateList = GetComponentsInChildren<IState>().OrderByDescending(x => x.Priority).ToList();
    }

    void Start()
    {
        StartCoroutine("updateAI");
    }

    private IEnumerator updateAI()
    {
        while (true)
        {
            for (int i = _unitList.Count - 1; i >= 0; i--)
            {
                UnitInfo unit = _unitList[i];
                if (unit.go == null)
                {
                    _unitList.Remove(unit);
                    continue;
                }
                Vector3 direction = unit.go.transform.position - transform.position;
                unit.angle = Vector3.Angle(direction, transform.forward);
                unit.distance = Vector3.Distance(unit.go.transform.position, transform.position);
            }
            if (_freezeAI <= 0)
            {
                _freezeAI = 0;
                foreach (IState state in _stateList)
                {
                    if (state.isTrigger() == true)
                    {
                        state.updateState();
                        break;
                    }
                }
            }
            else
                _freezeAI -= Time.deltaTime;
            for (int i = 0; i < 10; ++i)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void FrozeAI(float seconds)
    {
        _freezeAI += seconds;
    }

    void OnTriggerEnter(Collider other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            float distance = Vector3.Distance(other.transform.position, transform.position);
            UnitsInfo.e_UnitType type = UnitsInfo.e_UnitType.NONE;
            if (entity is UnitEntity)
            {
                UnitEntity unitEnt = (UnitEntity)entity;
                type = unitEnt.type; //.unitType;
            }
            if (entity is HeroEntity)
                type = UnitsInfo.e_UnitType.HERO;
            UnitInfo info = new UnitInfo(entity, other.gameObject, angle, distance, type);
            _unitList.Add(info);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            _unitList.Remove(new UnitInfo(entity, other.gameObject, 0, 0, UnitsInfo.e_UnitType.NONE));
        }
    }

    public List<UnitInfo> getUnitList()
    {
        return _unitList;
    }

    public UnitInfo getBestEnemy(e_Team team, float angle, float maxDist)
    {
        UnitInfo info = null;
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team != team && unit.angle < angle / 2 && unit.distance <= maxDist)
            {
                if (info == null)
                    info = unit;
                else if ((float)unit.unitType / (unit.distance * 0.1) > (float)info.unitType / (info.distance * 0.1))
                    info = unit;
            }
        }
        return info;
    }

    public UnitInfo getClosestAlly(e_Team team, float angle)
    {
        UnitInfo info = null;
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team == team && unit.angle < angle / 2)
            {
                if (info == null)
                    info = unit;
                else if (unit.distance < info.distance)
                    info = unit;
            }
        }
        return info;
    }

    public UnitInfo getFurthestAlly(e_Team team, float angle)
    {
        UnitInfo info = null;
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team == team && unit.angle < angle / 2)
            {
                if (info == null)
                    info = unit;
                else if (unit.distance > info.distance)
                    info = unit;
            }
        }
        return info;
    }

    public UnitInfo getClosestEnemy(e_Team team, float angle)
    {
        UnitInfo info = null;
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team != team && unit.angle < angle / 2)
            {
                if (info == null)
                    info = unit;
                else if (unit.distance < info.distance)
                    info = unit;
            }
        }
        return info;
    }

    public UnitInfo getFurthestEnemy(e_Team team, float angle)
    {
        UnitInfo info = null;
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team != team && unit.angle < angle / 2)
            {
                if (info == null)
                    info = unit;
                else if (unit.distance > info.distance)
                    info = unit;
            }
        }
        return info;
    }

    public List<UnitInfo> getAllies(e_Team team, float angle)
    {
        List<UnitInfo> list = new List<UnitInfo>();
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team == team && unit.angle < angle / 2)
                list.Add(unit);
        }
        return list;
    }

    public List<UnitInfo> getEnemies(e_Team team, float angle)
    {
        List<UnitInfo> list = new List<UnitInfo>();
        foreach (UnitInfo unit in _unitList)
        {
            if (unit.entity.Team != team && unit.angle < angle / 2)
                list.Add(unit);
        }
        return list;
    }
}