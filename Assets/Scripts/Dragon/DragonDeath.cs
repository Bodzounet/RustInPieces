using UnityEngine;
using System.Collections;

public class DragonDeath : MonoBehaviour
{

    Entity entity;

    void Start()
    {
        entity = GetComponentInParent<Entity>();
        entity.OnDeath += Death;
    }

    void Death(GameObject dead)
    {
        if (dead.GetComponent<Entity>().Hitter != null)
        {
            GameObject strat = PlayersInfos.Instance.GetStrategistOfTeam(dead.GetComponent<Entity>().Hitter.Team);
            strat.GetComponentInChildren<SkillTree.TreeManager>().UnlockEliteLevel();
        }
    }
}
