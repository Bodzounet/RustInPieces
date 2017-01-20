using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RaiseEventStrategist : MonoBehaviour
{
    void Awake()
    {
        EventManager.Instance.addEventCallback(EventCode.STRAT_LAUNCH_INSTANT, LaunchInstant);
        EventManager.Instance.addEventCallback(EventCode.STRAT_LAUNCH_TO_TARGET, LaunchToTarget);
        EventManager.Instance.addEventCallback(EventCode.STRAT_LAUNCH_TO_POSITION, LaunchToPosition);
        EventManager.Instance.addEventCallback(EventCode.STRAT_MODIFY_NODE, UpdateTreeNodeState);
        EventManager.Instance.addEventCallback(EventCode.STRAT_CREATE_TREE, CreateTree);

        EventManager.Instance.addEventCallback(EventCode.STRAT_SPAWN_MINION, SpawnMinion);
        EventManager.Instance.addEventCallback(EventCode.STRAT_MODIFY_LANE_COMPOSITION, ModifyLaneComposition);

    }

    public void LaunchInstant(object data, int senderID)
    {
        LaunchInstantData lid = (LaunchInstantData)data;
        PlayersInfos.Instance.getGameObjectWithPhotonId(lid.pid).GetComponent<Spells.SpellLauncher>().Launch(lid.spellId);
    }

    public void LaunchToTarget(object data, int senderID)
    {
        LaunchToTargetData lttd = (LaunchToTargetData)data;

        List<GameObject> targets = new List<GameObject>();
        foreach (var v in PlayersInfos.Instance.heroList)
        {
            if (lttd.heroTargets.Contains(v.GetPhotonView().viewID))
                targets.Add(v);
        }

        foreach (var v in PlayersInfos.Instance.strategistList)
        {
            if (lttd.heroTargets.Contains(v.GetPhotonView().viewID))
                targets.Add(v);
        }

        foreach (var v in PlayersInfos.Instance.strategistList)
        {
            foreach (var w in v.GetComponent<MinionManager>().Minions)
            {
                if (lttd.minionTargets.Contains(w.GetComponent<MinionManager_CleanMinion>().IdMinion))
                    targets.Add(w);
            }
        }

        PlayersInfos.Instance.getGameObjectWithPhotonId(lttd.pid).GetComponent<Spells.SpellLauncher>().Launch(lttd.spellId, targets.ToArray());
    }

    public void LaunchToPosition(object data, int senderID)
    {
        LaunchToPositionData ltpd = (LaunchToPositionData)data;
        PlayersInfos.Instance.getGameObjectWithPhotonId(ltpd.pid).GetComponent<Spells.SpellLauncher>().Launch(ltpd.spellId, ltpd.position);
    }

    public void CreateTree(object data, int senderID)
    {
        BuildTreeData d = (BuildTreeData)data;

        var v = PlayersInfos.Instance.getGameObjectWithPhotonId(d.pid).GetComponentInChildren<SkillTree.TreeManager>();

        var tree = new NodeData_Serializable[d.treeId.Length];
        for (int i = 0; i < tree.Length; i++)
        {
            tree[i] = new NodeData_Serializable();
            tree[i].id = d.treeId[i];
            tree[i].rank = d.treeRank[i];
        }

        v.BuildTree(tree);
        v.ResetTree();
    }

    public void UpdateTreeNodeState(object data, int senderID)
    {
        UpdateTreeNodeStateData utnsd = (UpdateTreeNodeStateData)data;
        PlayersInfos.Instance.getGameObjectWithPhotonId(utnsd.pid).GetComponentInChildren<SkillTree.TreeManager>().nodes.Single(x => x.UniqueID == utnsd.nodeID).ChangeLevel(utnsd.newNodeLevel, utnsd.op);
    }

    public void SpawnMinion(object data, int senderID)
    {
        SpawnMinionData smd = (SpawnMinionData)data;

        StrategistManager sm = PlayersInfos.Instance.getGameObjectWithPhotonId(smd.pid).GetComponent<StrategistManager>();

        foreach (var v in sm.hq.spawningPoints)
        {
            foreach (var w in v.Squad)
            {
                for (int i = 0; i < w.Value; i++)
                {
                    sm.minionManager.CreateMinion(w.Key, v.transform.position, Quaternion.identity, v.lane);
                }
            }
        }
    }

    public void ModifyLaneComposition(object data, int senderID)
    {
        ModifyLaneCompositionData mlcd = (ModifyLaneCompositionData)data;

        Core_MinionManager.SpawningPoint sp = PlayersInfos.Instance.getGameObjectWithPhotonId(mlcd.pid).GetComponent<StrategistManager>().hq.spawningPoints.Single(x => x.SPnumber == mlcd.lineID);
        sp.ClearSpawnPoint();
        foreach (var v in mlcd.newCompo)
        {
            sp.AddUnitToThisSpawnPoint(v);
        }
    }

    [System.Serializable]
    public struct BuildTreeData
    {
        public int[] treeId;
        public int[] treeRank;
        public int pid;
    }

    [System.Serializable]
    public struct LaunchInstantData
    {
        public string spellId;
        public int pid;
    }

    [System.Serializable]
    public struct LaunchToTargetData
    {
        public string spellId;
        public int[] heroTargets;
        public int[] minionTargets;
        public int pid;
    }

    [System.Serializable]
    public struct LaunchToPositionData
    {
        public string spellId;
        public Vector3 position;
        public int pid;
    }

    [System.Serializable]
    public struct UpdateTreeNodeStateData
    {
        public int nodeID;
        public int newNodeLevel;
        public SkillTree.TreeNode.e_LevelOperation op;
        public int pid;
    }

    [System.Serializable]
    public struct ModifyLaneCompositionData
    {
        public int lineID;
        public UnitsInfo.e_UnitType[] newCompo;
        public int pid;
    }

    [System.Serializable]
    public struct SpawnMinionData
    {
        public int pid;
    }
}
