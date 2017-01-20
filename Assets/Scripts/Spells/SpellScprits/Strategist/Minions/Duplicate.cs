using UnityEngine;
using System.Collections;
using System;

public class Duplicate : Spells.ST_Instant
{
    [SerializeField]
    GameObject animationO;
    protected override void DoAction()
    {
        MinionManager mm = _baseSpell.Caster.GetComponent<StrategistManager>().minionManager;
       

        var copy = mm.Minions.ToArray().Clone() as GameObject[];

        foreach (var v in copy)
        {
            UnitEntity ue = v.GetComponent<UnitEntity>();

            GameObject minion = mm.CreateMinion(ue.type, v.transform.position, v.transform.rotation, ue.GetComponentInChildren<PatrolAI>().Lane);

            minion.GetComponent<Entity>().setStat(Entity.e_StatType.HP_CURRENT, ue.getStat(Entity.e_StatType.HP_CURRENT) / 2);
            Instantiate(animationO, minion.transform.position, animationO.transform.rotation);
        }
    }
}
