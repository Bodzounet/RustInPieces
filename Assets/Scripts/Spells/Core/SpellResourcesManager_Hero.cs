using UnityEngine;
using System.Collections;

namespace Spells
{
    /// <summary>
    /// Be sure to attach this script to the GO holding the Entity
    /// </summary>
    public class SpellResourcesManager_Hero : Spells.SpellResourcesManager
    {
        private Entity _entity;

        void Awake()
        {
            _entity = this.GetComponent<Entity>();
#if UNITY_EDITOR
            // just a check, if you mess with this script. IT HAS TO BE on the same GO that the one which has the Entity
            if (_entity == null)
            {
                if ((_entity = this.GetComponentInChildren<Entity>()) == null || (_entity = this.GetComponentInParent<Entity>()) == null)
                {
                    Debug.LogError("An Entity has been found, but this script is not on the same GO. Fix it.");
                }
                else
                {
                    Debug.LogError("No Entity has been founded. Add one.");
                }
            }
#endif
        }

        public override bool HasResources(SpellResources.e_SpellResources resourceType, int amount)
        {
#if UNITY_EDITOR
            if (resourceType == Spells.SpellResources.e_SpellResources.GOLD)
                Debug.LogError("Hero don't use gold as a spell resource");
#endif
            switch (resourceType)
            {
                case SpellResources.e_SpellResources.LIFE:
                    return _entity.getStat(Entity.e_StatType.HP_CURRENT) > amount;
                case SpellResources.e_SpellResources.MANA:
                    return _entity.getStat(Entity.e_StatType.MANA_CURRENT) > amount;
                default:
                    break;
            }
            return false;
        }

        public override bool UseResources(SpellResources.e_SpellResources resourceType, int amount)
        {
#if UNITY_EDITOR
            if (resourceType == Spells.SpellResources.e_SpellResources.GOLD)
                Debug.LogError("Hero don't use gold as a spell resource");
#endif
            switch (resourceType)
            {
                case SpellResources.e_SpellResources.LIFE:
                    _entity.modifyStat(Entity.e_StatType.HP_CURRENT, Entity.e_StatOperator.SUBTRACT, amount, _entity);
                    break;
                case SpellResources.e_SpellResources.MANA:
                    _entity.modifyStat(Entity.e_StatType.MANA_CURRENT, Entity.e_StatOperator.SUBTRACT, amount, _entity);
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}