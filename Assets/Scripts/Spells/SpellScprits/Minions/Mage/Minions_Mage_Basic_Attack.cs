using UnityEngine;
using System.Collections;

public class Minions_Mage_Basic_Attack : Spells.ST_Projectile
{
    Entity _casterEntity;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();
        float damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.MAGIC_ATT), Entity.e_AttackType.MAGIC);

        if (collidingEntity != null && _baseSpell.SpellTargets[0].GetComponent<Entity>() == collidingEntity)
        {
            collidingEntity.doDamages(damages, Entity.e_AttackType.MAGIC, _casterEntity);
            GameObject.Destroy(this.gameObject);
        }
    }
}