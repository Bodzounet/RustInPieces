using UnityEngine;
using System.Collections;

public class Minions_Melee_Basic_Attack : Spells.ST_Projectile
{
    private Entity _casterEntity;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();
        float damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.MELEE_ATT), Entity.e_AttackType.MELEE);

        if (collidingEntity != null && _baseSpell.SpellTargets[0].GetComponent<Entity>() == collidingEntity)
        {
            collidingEntity.doDamages(damages, Entity.e_AttackType.MELEE, _casterEntity);
            GameObject.Destroy(this.gameObject);
        }
    }
}