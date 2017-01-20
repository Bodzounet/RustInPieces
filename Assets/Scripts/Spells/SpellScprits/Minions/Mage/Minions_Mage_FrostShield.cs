using UnityEngine;
using System.Collections;


public class Minions_Mage_FrostShield : Spells.ST_Buff
{
    [SerializeField]
    private GameObject SlowDebuff;

    protected override void CancelEffect()
    {
        Entity targetEntity = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        targetEntity.addStatBoostValue(Entity.e_StatType.DEFENSE, 0.15f);
        targetEntity.removeBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.FROST_SHIELD);
    }

    protected override void DoEffect()
    {
        Entity targetEntity = _baseSpell.SpellTargets[0].GetComponent<Entity>();

        targetEntity.addStatBoostValue(Entity.e_StatType.DEFENSE, 0.15f);
        targetEntity.addBuff(Entity.e_StatType.HP_CURRENT, Spells.BuffKeys.FROST_SHIELD, OnHPCurrentUp);
    }

    protected override void OnDispel()
    {
        CancelEffect();
    }

    float OnHPCurrentUp(float bef, float aft)
    {
        if (aft < bef)
        {
            Entity targetEntity = _baseSpell.SpellTargets[0].GetComponent<Entity>();

            targetEntity.Hitter.GetComponent<Spells.BuffManager>().AddBuff(SlowDebuff, _baseSpell.Caster, targetEntity.Hitter.gameObject);
        }
        return aft;
    }

}
