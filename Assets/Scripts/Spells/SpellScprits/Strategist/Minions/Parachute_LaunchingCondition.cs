using UnityEngine;
using System.Collections;
using System;

public class Parachute_LaunchingCondition : Spells.LaunchingConditions
{
    public override bool CheckConditions(GameObject caster, e_Team team, GameObject target)
    {
        Camera cam = caster.GetComponentInChildren<Camera>();

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("LanePath")))
            return true;
        return false;
    }   
}
