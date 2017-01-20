using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TeamFactory : MonoBehaviour
{
    public delegate GameObject GameObject_D_GameObject(GameObject go);

    public event GameObject_D_GameObject OnMinionIsCreated;

    [SerializeField]
    private e_Team _myTeam;
    public e_Team MyTeam
    {
        get { return _myTeam; }
    }

    /// <summary>
    /// All upgrade to be applied to the prefab identified by its name when it is created
    /// </summary>
    private Dictionary<string, List<PrefabUpgrade>> _prefabUpgrade = new Dictionary<string, List<PrefabUpgrade>>();
    private Dictionary<string, List<GameObject_D_GameObject>> _prefabCallBack = new Dictionary<string, List<GameObject_D_GameObject>>();

//    public static TeamFactory GetMyTeamFactory(e_Team team = e_Team.NEUTRAL)
//    {
//        if (team == e_Team.NEUTRAL && PhotonNetwork.connectionState == ConnectionState.Connected)
//			team = (e_Team)PhotonNetwork.player.customProperties["team"];
//#if UNITY_EDITOR
//			// a workaround, while waiting for the good way to do it. (need photon, etc...)

//		try
//        {
//            //Debug.Log("team : " + PhotonNetwork.player.customProperties["team"]);
//            if (PhotonNetwork.connectionState == ConnectionState.Connected)
//                return GameObject.FindObjectsOfType<TeamFactory>().Where(x => x.MyTeam == team).Single();
//            else
//                return GameObject.FindObjectsOfType<TeamFactory>().Where(x => x.MyTeam == GameObject.FindObjectOfType<TeamInfos>().MyTeam).Single();
//        }
//        catch
//        {
//            Debug.LogWarning("To make it work, 2 steps : \nAdd a TeamInfos Script to any GO and give your team in the inspector.\n fill this class property MyTeam.\n TeamInfos will soon be useless. Work in progress :p");
//        }
//#else
//        // Implement the good way to do it
//		 return GameObject.FindObjectsOfType<TeamFactory>().Where(x => x.MyTeam == team).Single();
//#endif
//        return null;
//    }

    /// <summary>
    /// Add an upgrade to the prefab.
    /// The upgrade must be a monobehaviour.
    /// An instance of this monobehaviour will be add to the prefab when it is created.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    public void AddUpgrade<T>(string prefabName)
        where T : MonoBehaviour
    {
        if (!_prefabUpgrade.ContainsKey(prefabName))
            _prefabUpgrade[prefabName] = new List<PrefabUpgrade>();

        if (_prefabUpgrade[prefabName].Any(x => x.upgradeType == typeof(T)))
        {
            Debug.LogWarning("The prefab : " + prefabName + " already has the upgrade : " + typeof(T).ToString() + ". Nothing to add");
            return;
        }

        _prefabUpgrade[prefabName].Add(new PrefabUpgrade(prefabName, typeof(T)));
    }

    /// <summary>
    /// Remove a previous added upgrade.
    /// You still need to give the prefab name of the concerned Prefab since multiple prefabs can have the same upgrade
    /// 
    /// /!\ THIS FUNCTION DO NOT REMOVE THE UPGRADE TO ANY EXISTING GAMEOBJECT WHICH CURRENTLY HAS IT.
    /// /!\ it just prevents any new prefab with the name "prefabName" to be creating with this upgrade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    public void RemoveUpgrade<T>(string prefabName)
        where T : MonoBehaviour
    {
        if (!_prefabUpgrade.ContainsKey(prefabName) || _prefabUpgrade[prefabName].Count == 0)
        {
            Debug.LogWarning("The prefab : " + prefabName + " don't have any Upgrade attached to. Nothing to remove");
            return;
        }

        if (!_prefabUpgrade[prefabName].Any(x => x.upgradeType == typeof(T)))
        {
            Debug.LogWarning("The prefab : " + prefabName + " don't have any Upgrade of type : " + typeof(T).ToString() + ". Nothing to remove");
            return;
        }

        _prefabUpgrade[prefabName].Remove(_prefabUpgrade[prefabName].Single(x => x.upgradeType == typeof(T)));
    }

    /// <summary>
    /// Another way to upgrade your prefab when it is created.
    /// Give a callback for one of your script, and it will be called each time the prefab "prefabName" will be instanciated.
    /// BE SURE THE SCRIPT HOLDING THE CALLBACK STILL EXIST OR YOU LL GET AN ERROR.
    /// also, the prefab must have this signature : GameObject MyCallBack(GameObject toto); (return the GameObject you got as parameter)
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="callBack"></param>
    public void AddCallBack(string prefabName, GameObject_D_GameObject callBack)
    {
        if (!_prefabCallBack.ContainsKey(prefabName))
            _prefabCallBack[prefabName] = new List<GameObject_D_GameObject>();

        if (_prefabCallBack[prefabName].Any(x => x == callBack))
        {
            Debug.Log("prefab : " + prefabName + " already has the callback you are trying to add. CB is : " + callBack.ToString());
            return;
        }

        _prefabCallBack[prefabName].Add(callBack);
    }

    /// <summary>
    /// Allow you to remove a callback
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="callBack"></param>
    public void RemoveCallBack(string prefabName, GameObject_D_GameObject callBack)
    {
        if (!_prefabCallBack.ContainsKey(prefabName) || _prefabCallBack[prefabName].Count == 0)
        {
            Debug.LogWarning("The prefab : " + prefabName + " don't have any callBack attached to. Nothing to remove");
            return;
        }

        if (!_prefabCallBack[prefabName].Any(x => x == callBack))
        {
            Debug.LogWarning("The prefab : " + prefabName + " don't have any callBack named " + callBack.ToString() + ". Nothing to remove");
            return;
        }

        _prefabCallBack[prefabName].Remove(_prefabCallBack[prefabName].Single(x => x == callBack));
    }

    private GameObject _CreateThing(string name, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        var thing = Factory.CreateInstanceOf(name, position, rotation);

        if (_prefabUpgrade.ContainsKey(name))
        {
            foreach (var v in _prefabUpgrade[name])
            {
                thing.AddComponent(v.upgradeType);
            }
            foreach (var v in _prefabCallBack[name])
            {
                thing = v(thing);
            }
        }

        return thing;
    }

    /// <summary>
    /// Use this one to create minions;
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject CreateMinion(UnitsInfo.e_UnitType type, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        GameObject minion = _CreateThing(UnitsInfo.TypeToString(type), position, rotation);
        minion.GetComponent<Entity>().Team = MyTeam;

        if (OnMinionIsCreated != null)
            OnMinionIsCreated(minion);

        return minion;
    }

    /// <summary>
    /// This one is for creating spellInfo
    /// </summary>
    /// <param name="spellId"></param>
    /// <returns></returns>
    public GameObject CreateSpellInfo(string spellId)
    {
        return _CreateThing(spellId);
    }

    private class PrefabUpgrade
    {
        public string prefabName;
        public System.Type upgradeType;

        public PrefabUpgrade(string prefabName_, System.Type upgradeType_)
        {
            prefabName = prefabName_;
            upgradeType = upgradeType_;
        }
    }
}
