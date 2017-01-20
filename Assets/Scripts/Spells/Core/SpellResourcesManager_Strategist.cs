using UnityEngine;
using System.Collections;

public class SpellResourcesManager_Strategist : Spells.SpellResourcesManager
{
    //private ResourcesManager _resourcesManager;

    protected void Awake()
    {
        //_resourcesManager = StrategistManager.GetMyTeamStrategistManager().resourcesManager;
    }

    public override bool HasResources(Spells.SpellResources.e_SpellResources resourceType, int amount)
    {
#if UNITY_EDITOR
        if (resourceType != Spells.SpellResources.e_SpellResources.GOLD)
            Debug.LogError("Strategist can only use gold as resource for laucnhing a spell");
#endif
        //return _resourcesManager.hasEnoughGold(amount);
        return true;
    }

    public override bool UseResources(Spells.SpellResources.e_SpellResources resourceType, int amount)
    {
#if UNITY_EDITOR
        if (resourceType != Spells.SpellResources.e_SpellResources.GOLD)
            Debug.LogError("Strategist can only use gold as resource for laucnhing a spell");
#endif
        //if (!_resourcesManager.HasEnoughGold(amount))
        //    return false;

        //_resourcesManager.UseGold(amount);
        return true;
    }
}
