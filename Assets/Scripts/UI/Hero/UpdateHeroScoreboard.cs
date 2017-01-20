using UnityEngine;
using System.Collections;

public class UpdateHeroScoreboard : MonoBehaviour
{
    /*HeroEntity _entity;
    PhotonView _photonView;
    ScoreBoard _scoreBoard;

    void Start ()
    {
        _entity = this.GetComponent<HeroEntity>();
        _photonView = this.GetComponent<PhotonView>();
        _scoreBoard = GameObject.Find("HeroUI").GetComponent<ScoreBoard>();
        _photonView.RPC("UpdateScoreBoard", PhotonTargets.All, (int)_entity.Team, _photonView.viewID, _entity.IconName);
        _entity.OnKDAChanged += this.OnKDAChanged;
	}
	
    void OnKDAChanged(KDA kda)
    {
        string kdaToString = kda.Kills + "/" + kda.Deaths + "/" + kda.Assists;
        _photonView.RPC("UpdateScoreBoard", PhotonTargets.All, (int)_entity.Team, _photonView.viewID, kdaToString);
    }

    [PunRPC]
    public void UpdateScoreBoard(int team, int heroId, string kda)
    {
        _scoreBoard.UpdateScoreBoard(team, heroId, kda);
    }

    [PunRPC]
    public void InitializeScoreBoard(int team, int heroId, string iconName)
    {
        _scoreBoard.UpdateScoreBoard(team, heroId, iconName);
    }*/
}
