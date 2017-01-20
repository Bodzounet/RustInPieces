using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minions_Melee_Splash_Attack : Spells.ST_Projectile
{
    private Entity _casterEntity;
    public float splashRadius = 3f;

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
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, splashRadius, transform.forward, 1f, 1 << LayerMask.NameToLayer("Entity") | 1 << LayerMask.NameToLayer("MinionEntity") | 1 << LayerMask.NameToLayer("HeroEntity"));
            foreach (RaycastHit hit in hits)
            {
                //Debug.Log("hit : " + hit.collider.name);
                Entity ent = hit.collider.GetComponent<Entity>();
                if (ent != collidingEntity && ent != _casterEntity && ent.Team != _casterEntity.Team)
                    ent.doDamages(damages / 4, Entity.e_AttackType.MELEE, _casterEntity);
            }
            collidingEntity.doDamages(damages, Entity.e_AttackType.MELEE, _casterEntity);
            GameObject.Destroy(this.gameObject);
        }
    }
}