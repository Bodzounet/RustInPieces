using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public delegate void Void_D_String(string s);

namespace Spells
{
    public delegate void Void_D_CoolDownManager(CooldownManager cdm);

    public class CooldownManager : MonoBehaviour
    {
        public UnityEvent_Float OnCooldownIsUpdated = new UnityEvent_Float();

        private Void_D_CoolDownManager _cb;

        private string _spellId;
        public string SpellId
        {
            get { return _spellId; }
        }

        private float _remainingTime;
        public float RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                _remainingTime = value;
                StopCoroutine("Co_CoolDown");
                StartCoroutine("Co_CoolDown", new StartCooldownData(_spellId, _remainingTime, _cb));
            }
        }

        public void StartCooldown(StartCooldownData cdd)
        {
            _spellId = cdd.id;
            _remainingTime = Mathf.FloorToInt(cdd.cooldown * 10) / 10;
            _cb = cdd.OnCooldownEnd;
            StartCoroutine("Co_CoolDown", cdd);
        }

        IEnumerator Co_CoolDown(StartCooldownData cdd)
        {
            int iterations = Mathf.FloorToInt(cdd.cooldown * 10);

            while (iterations > 0)
            {
                yield return new WaitForSeconds(0.1f);
                iterations--;
                _remainingTime = (float)iterations / 10;
                if (OnCooldownIsUpdated != null)
                    OnCooldownIsUpdated.Invoke(RemainingTime);
            }
            cdd.OnCooldownEnd(this);
            Destroy(this.gameObject);
        }
    }

    public class StartCooldownData
    {
        public string id;
        public float cooldown;
        public Void_D_CoolDownManager OnCooldownEnd;

        public StartCooldownData(string MD5Id, float cd, Void_D_CoolDownManager callback)
        {
            id = MD5Id;
            cooldown = cd;
            OnCooldownEnd = callback;
        }
    }

    /// <summary>
    /// Unity asks for it...
    /// </summary>
    [System.Serializable]
    public class UnityEvent_Float : UnityEvent<float>
    {
    }
}