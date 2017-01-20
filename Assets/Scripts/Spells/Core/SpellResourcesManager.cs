using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells
{
    public abstract class SpellResourcesManager : MonoBehaviour
    {
        public abstract bool HasResources(SpellResources.e_SpellResources resourceType, int amount);
        public abstract bool UseResources(SpellResources.e_SpellResources resourceType, int amount);
        //public abstract bool tryUseResources(SpellResources.SpellResourcesAux[] resources);
    }
}