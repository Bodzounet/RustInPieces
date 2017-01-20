using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoard : MonoBehaviour
{
    GameObject _scoreBoard;
    //PhotonView _photonView;
    Team[] _teams = new Team[2];

    void Awake()
    {
        _scoreBoard = transform.Find("ScoreBoard").gameObject; ;
        _teams[0] = new Team(_scoreBoard.transform.GetChild(0));
        _teams[1] = new Team(_scoreBoard.transform.GetChild(1));
        _scoreBoard.SetActive(false);
        //_photonView = this.GetComponent<PhotonView>();
    }
	
	void Update ()
    {
	    if (Input.GetButtonDown("ScoreBoard"))
        {
            _scoreBoard.SetActive(true);
        }
        if (Input.GetButtonUp("ScoreBoard"))
        {
            _scoreBoard.SetActive(false);
        }
	}

    public class Team
    {
        Hero[] _heroes;
        Strategist _strategist;

        public Team(Transform teamUI)
        {
            _heroes = new Hero[3];
            _strategist = new Strategist(teamUI.GetChild(0));
            for (int i = 0; i < 3; i++)
            {
                _heroes[i] = new Hero(teamUI.GetChild(i + 1));
            }
        }

        public Hero[] heroes
        {
            get { return (_heroes); }
            set { _heroes = value;  }
        }

        public Strategist strategist
        {
            get { return (_strategist); }
            set { _strategist = value; }
        }
    }

    public class Hero
    {
        int _heroID;
        Image _icon;
        Text _kda;
		Text _name;
		public int kill = 0;
		public int death = 0;
		public int assist = 0;

		public Hero(Transform heroUI)
        {
            _heroID = -1;
            _icon = heroUI.GetComponentInChildren<Image>();
            _kda = heroUI.Find("KDA").GetComponent<Text>();
			_name = heroUI.Find("NAME").GetComponent<Text>();
		}

        public int heroID
        {
            get { return (_heroID); }
            set { _heroID = value; }
        }

        public Image icon
        {
            get { return (_icon); }
            set { _icon = value; }
        }

        public Text kda
        {
            get { return (_kda); }
            set { _kda = value; }
        }

		public Text name
		{
			get { return (_name); }
			set { _name = value; }
		}
	}

    public class Strategist
    {
        Image _icon;

        public Strategist(Transform strategistUI)
        {
            _icon = strategistUI.GetComponentInChildren<Image>();
        }

        public Image icon
        {
            get { return (_icon); }
            set { _icon = value; }
        }
    }

    public void UpdateScoreBoard(int team, int heroId, int kill = 0, int death = 0, int assist = 0)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_teams[team].heroes[i].heroID == heroId)
            {
				_teams[team].heroes[i].kill += kill;
				_teams[team].heroes[i].death += death;
				_teams[team].heroes[i].assist += assist;
				_teams[team].heroes[i].kda.text = _teams[team].heroes[i].kill + "/" + _teams[team].heroes[i].death + "/" + _teams[team].heroes[i].assist;
                break;
            }
        }
    }

    public void InitializeScoreBoard(int team, int heroId, string iconName, string heroName)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_teams[team].heroes[i].heroID == -1)
            {
                _teams[team].heroes[i].heroID = heroId;
                _teams[team].heroes[i].kda.text = "0/0/0";
				_teams[team].heroes[i].name.text = heroName;
                _teams[team].heroes[i].icon.sprite = Resources.Load<Sprite>("HeroIcons/" + iconName);
                break;
            }
        }
    }
}
