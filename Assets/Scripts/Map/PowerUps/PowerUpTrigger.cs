using UnityEngine;
using System.Collections;

public class PowerUpTrigger : MonoBehaviour
{

    public enum e_PowerUpType
    {
        MANA,
        HEALTH,
        SPEED,
        SUPER,
        FAKE,
        RESET
    }

    public e_PowerUpType bonus;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HeroEntity"))
        {
            Debug.Log("COUCOU" + bonus);
            switch (bonus)
            {
                case e_PowerUpType.MANA:
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.MANA_CURRENT, 0.15f);
                    this.gameObject.SetActive(false);
                    break;

                case e_PowerUpType.HEALTH:
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.HP_CURRENT, 0.15f);
                    this.gameObject.SetActive(false);
                    break;

                case e_PowerUpType.SPEED:
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.SPEED, 0.15f);
                    StartCoroutine(SpeedUp(other.gameObject));
                    this.gameObject.SetActive(false);
                    break;

                case e_PowerUpType.SUPER:
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.MANA_CURRENT, 0.15f);
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.HP_CURRENT, 0.15f);
                    other.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.SPEED, 0.15f);
                    StartCoroutine(SpeedUp(other.gameObject));
                    this.gameObject.SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }

    IEnumerator SpeedUp(GameObject target)
    {
        yield return new WaitForSeconds(5);
        target.GetComponent<HeroEntity>().addStatBoostValue(Entity.e_StatType.SPEED, -0.15f);

    }
}
