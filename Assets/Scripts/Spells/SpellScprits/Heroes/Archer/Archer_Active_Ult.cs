using UnityEngine;
using System.Collections;
using System;

public class Archer_Active_Ult : Spells.ST_Projectile
{
    Entity _casterEntity;
    Spells.SpellLauncher _spellLauncher;

    protected override void Start()
    {
        base.Start();

        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
        _spellLauncher = _baseSpell.Caster.GetComponent<Spells.SpellLauncher>();
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<HeroEntity>();

        if (collidingEntity != null && collidingEntity.Team != _casterEntity.Team)
        {
            _spellLauncher.Launch("Archer_Active_Ult_Proj", new GameObject[] { collidingObject });
        }
    }
}
