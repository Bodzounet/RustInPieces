using UnityEngine;
using System.Collections;
using System.Linq;

public class SecondaryObjective : MonoBehaviour
{
    private bool unlock1 = false;
    private bool unlock2 = false;
    [SerializeField]
    private ParticleSystem[] _particles;
    [SerializeField]
    private GameObject _pivot;
    private float speed1;
    private bool active = true;
    private bool negative = false;
    private int coutingmodulo;
    [SerializeField]
    private int goldeveryXframes;

    [SerializeField]
    private int _goldStaking = 0;

    public float Speed1
    {
        get
        {
            return speed1;
        }

        set
        {
            speed1 = value;
        }
    }

    private float speed2;

    public float Speed2
    {
        get
        {
            return speed2;
        }

        set
        {
            speed2 = value;
        }
    }

    [SerializeField]
    float time = 0;
    public float Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HeroEntity"))
        {
            HeroEntity e = other.gameObject.GetComponent<HeroEntity>();

            if (PhotonNetwork.connectionState == ConnectionState.Connected)
            {
                PhotonView _pView = other.gameObject.GetComponent<PhotonView>();
                if (_pView != null)
                {
                    if (_pView.isMine)
                        _pView.RPC("heroEnterPoint", PhotonTargets.All, (e.Team == e_Team.TEAM1 ? 1 : 2));

                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HeroEntity"))
        {
            HeroEntity e = other.gameObject.GetComponent<HeroEntity>();

            if (PhotonNetwork.connectionState == ConnectionState.Connected)
            {
                PhotonView _pView = other.gameObject.GetComponent<PhotonView>();
                if (_pView != null)
                {
                    if (_pView.isMine)
                        _pView.RPC("heroExitPoint", PhotonTargets.All, (e.Team == e_Team.TEAM1 ? 1 : 2));

                }
            }
        }
    }



    void FixedUpdate()
    {
        coutingmodulo++;
        if (coutingmodulo > goldeveryXframes)
        {
            _goldStaking++;
            coutingmodulo = 0;
        }
        if (Speed1 <= 0 || Speed2 <= 0)
        {
            Time += Speed1 * 0.2f;
            Time -= Speed2 * 0.2f;

            if (Time > 50)
                Time = 50;
            else if (Time < -50)
                Time = -50;

            if (_pivot != null)
            {
                if (Time < 1 && Time > -1)
                {
                    if (active)
                    {
                        _pivot.SetActive(false);
                        active = false;
                    }
                }
                else
                {
                    if (!active)
                    {
                        _pivot.SetActive(true);
                        active = true;
                    }
                    if (Speed1 > 0 || Speed2 > 0)
                    {
                        if ((Time < 0) && (negative == false))
                        {
                            foreach (ParticleSystem particles in _particles)
                            {
                                particles.startColor = Color.blue;
                                negative = true;
                                unlock1 = false;
                                unlock2 = false;
                            }
                        }
                        else if ((Time > 0) && (negative == true))
                        {
                            foreach (ParticleSystem particles in _particles)
                            {
                                particles.startColor = Color.red;
                                negative = false;
                                unlock1 = false;
                                unlock2 = false;
                            }
                        }
                        if (Speed1 != Speed2)
                        {
                            _pivot.transform.localScale = new Vector3( (Time < 0 ? -Time : Time) / 50, 0, (Time < 0 ? -Time : Time) / 50) ;
                        }
                    }
                }
            }

           
            if (!unlock1 && Time == 50)
            {
                unlock1 = true;
            }
            else if (!unlock2 && Time == -50)
            {
                unlock2 = true;
            }
        }

        if (_goldStaking > 0)
        {
            if (unlock1)
            {

                foreach (var heros in PlayersInfos.Instance.heroList.Where(x => (x != null)).Where(x=> x.GetComponent<Entity>().Team == e_Team.TEAM1))
                {
                        heros.GetComponent<HeroEntity>().Gold += _goldStaking;
                }
                foreach (GameObject strat in PlayersInfos.Instance.strategistList)
                {
                    if (strat != null)
                    {
                        if (strat.GetComponent<StrategistManager>().Team == e_Team.TEAM1)
                        {
                            strat.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gold].AddCurrency(_goldStaking);
                        }
                    }
                }
                _goldStaking = 0;
            }
            else if (unlock2)
            {
                foreach (var heros in PlayersInfos.Instance.heroList.Where(x => (x != null)).Where(x => x.GetComponent<Entity>().Team == e_Team.TEAM2))
                {
                    heros.GetComponent<HeroEntity>().Gold += _goldStaking;
                }
                foreach (GameObject strat in PlayersInfos.Instance.strategistList)
                {
                    if (strat != null)
                    {
                        if (strat.GetComponent<StrategistManager>().Team == e_Team.TEAM2)
                        {
                            strat.GetComponent<CurrenciesManager>().currencies[CurrenciesManager.e_Currencies.Gold].AddCurrency(_goldStaking);
                        }
                    }
                }
                _goldStaking = 0;
            }
        }     
    }
}
