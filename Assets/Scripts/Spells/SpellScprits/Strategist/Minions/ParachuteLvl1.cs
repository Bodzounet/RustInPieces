using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ParachuteLvl1 : Spells.ST_Instant
{
    protected override void DoAction()
    {
        Camera cam = _baseSpell.Caster.GetComponentInChildren<Camera>();

        RaycastHit hit;

        Physics.Linecast(_baseSpell.SpellPos, Vector3.up * 1000f, out hit, 1 << LayerMask.NameToLayer("LanePath"));
        int lane = hit.collider.transform.root.GetComponent<LaneId>().id;

        _baseSpell.Caster.GetComponent<StrategistManager>().minionManager.CreateMinion(UnitsInfo.e_UnitType.MAGIC, _baseSpell.SpellPos, Quaternion.identity, lane);
    }
}
