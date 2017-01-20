using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Spells
{
    public class SpellResources : MonoBehaviour
    {
        public enum e_SpellResources
        {
            // for heroes
            LIFE,
            MANA,

            // for strategist
            GOLD
        }

        [SerializeField]
        private Spells.SpellResources.SpellResourcesAux[] _baseSpellCost;
        public Spells.SpellResources.SpellResourcesAux[] BaseSpellCost
        {
            get { return _baseSpellCost; }
        }

        private Dictionary<e_SpellResources, int> _currentSpellCost = new Dictionary<e_SpellResources, int>();
        public Dictionary<e_SpellResources, int> CurrentSpellCost
        {
            get { return _currentSpellCost; }
            set { _currentSpellCost = value; }
        }



        void Start()
        {
            foreach (var v in _baseSpellCost)
            {
                _currentSpellCost[v.resources] = v.amount;
            }
        }

        [System.Serializable]
        public class SpellResourcesAux
        {
            public e_SpellResources resources;
            public int amount;

            SpellResourcesAux(e_SpellResources resources_, int amount_)
            {
                resources = resources_;
                amount = amount_;
            }
        }
    }
}