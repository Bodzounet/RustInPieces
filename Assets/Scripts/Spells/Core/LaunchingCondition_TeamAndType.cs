using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaunchingCondition_TeamAndType : Spells.LaunchingConditions
{
    public enum e_TeamConditionCheck
    {
        SAME_TEAM,
        DIFFERENT_TEAM,
        ALL_TEAM
    }

    public enum e_TargetType
    {
        HERO,
        UNIT,
        TOWER
    }

    public e_TeamConditionCheck conditionCheck;
    public List<e_TargetType> unitForbidden = new List<e_TargetType>();

    public override bool CheckConditions(GameObject caster, e_Team casterTeam, GameObject target)
    {
        bool isValid = true;
        Entity targetEntity = target.GetComponent<Entity>();

        if (targetEntity == null)
            return false;
        if ((conditionCheck == e_TeamConditionCheck.SAME_TEAM && targetEntity.Team != casterTeam) || (conditionCheck == e_TeamConditionCheck.DIFFERENT_TEAM && targetEntity.Team == casterTeam))
            isValid = false;
        if ((targetEntity is UnitEntity && unitForbidden.Contains(e_TargetType.UNIT))
            || (targetEntity is HeroEntity && unitForbidden.Contains(e_TargetType.HERO))
            || (targetEntity is TowerEntity && unitForbidden.Contains(e_TargetType.TOWER)))
            isValid = false;
        return isValid;
    }
}
