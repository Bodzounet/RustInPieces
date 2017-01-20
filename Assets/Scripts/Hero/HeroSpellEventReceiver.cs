using UnityEngine;
using System.Collections;

public class HeroSpellEventReceiver : MonoBehaviour
{

	void Start()
    {
        EventManager.Instance.addEventCallback(EventCode.HERO_SPELL_LAUNCHED, PhotonSpellEvent);
    }

    void PhotonSpellEvent(object spellInfo, int senderId)
    {
        HeroSpellLaunchedInfo info = (HeroSpellLaunchedInfo)spellInfo;
        GameObject caster = PhotonView.Find(info.casterPhotonId).gameObject;
        Spells.SpellLauncher casterSpellLauncher = caster.GetComponent<Spells.SpellLauncher>();

        caster.transform.position = info.casterPosition;
        caster.GetComponentInChildren<Animator>().transform.eulerAngles = info.casterRotation;
        if (casterSpellLauncher.IsSpellInCooldown(info.index))
        {
            HandleAnimator(info.index, PhotonView.Find(info.casterPhotonId).gameObject.GetComponentInChildren<Animator>());
        }
        switch (info.castType)
        {
            case Spells.SpellInfo.e_CastType.NO_TARGET:
                casterSpellLauncher.Launch(info.spellId);
                break;

            case Spells.SpellInfo.e_CastType.POSITION:
                casterSpellLauncher.Launch(info.spellId, info.spellPosition);
                break;

            case Spells.SpellInfo.e_CastType.TARGET:
                GameObject[] target = new GameObject[] { PhotonView.Find(info.target).gameObject };
                casterSpellLauncher.Launch(info.spellId, target, new Vector3[] { info.casterPosition });
                break;

            case Spells.SpellInfo.e_CastType.SELFBUFF:
                casterSpellLauncher.Launch(info.spellId, new GameObject[] { caster });
                break;
        }
    }

    void HandleAnimator(int index, Animator animator)
    {
        switch (index)
        {
            case 0:
                animator.SetTrigger("AutoAttack");
                break;
            case 2:
                animator.SetTrigger("Spell1");
                break;
            case 3:
                animator.SetTrigger("Spell2");
                break;
            case 4:
                animator.SetTrigger("Spell3");
                break;
            case 5:
                animator.SetTrigger("Ult");
                break;
        }
    }
}
