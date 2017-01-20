using UnityEngine;
using System.Collections;

public class Minions_Range_BounceHero_Attack : Spells.ST_Projectile
{
    private Entity _casterEntity;
    private int _bounce = 0;

    protected override void Start()
    {
        base.Start();
        _casterEntity = _baseSpell.Caster.GetComponent<Entity>();
    }

    protected override void DoAction(GameObject collidingObject)
    {
        Entity collidingEntity = collidingObject.GetComponent<Entity>();
        float damages = _casterEntity.BADamageBuff(_casterEntity.getStat(Entity.e_StatType.RANGE_ATT), Entity.e_AttackType.RANGE);

        if (collidingEntity != null && _baseSpell.SpellTargets[0].GetComponent<Entity>() == collidingEntity)
        {
            if (collidingEntity is HeroEntity)
                damages *= 2;
            collidingEntity.doDamages(damages, Entity.e_AttackType.RANGE, _casterEntity);
            if (_bounce == 0)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5f, transform.forward, 1f, LayerMask.GetMask("Entity", "MinionEntity", "HeroEntity"));
                foreach (RaycastHit hit in hits)
                {
                    Entity ent = hit.collider.GetComponent<Entity>();
                    if (ent != collidingEntity && ent != _casterEntity && ent.Team != _casterEntity.Team)
                    {
                        _baseSpell.SpellTargets[0] = ent.gameObject;
                        _bounce++;
                        return;
                    }
                }
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}