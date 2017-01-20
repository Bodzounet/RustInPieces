using UnityEngine;
using System.Collections;
using System.IO;
using ExitGames.Client.Photon;

public class SerializeDataStructure : MonoBehaviour
{
    void Awake()
    {
        PhotonPeer.RegisterType(typeof(HeroSpellLaunchedInfo), EventCode.HERO_SPELL_LAUNCHED, SerializeHeroSpellLaunchedInfo, DeserializeHeroSpellLaunchedInfo);
        PhotonPeer.RegisterType(typeof(PowerUpInfo), EventCode.POWER_UP, SerializePowerUpInfo, DeserializePowerUpInfo);
        PhotonPeer.RegisterType(typeof(DoorInfo), EventCode.Door, SerializeDoorInfo, DeserializeDoorInfo);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.ModifyLaneCompositionData), EventCode.STRAT_MODIFY_LANE_COMPOSITION, SerializeModifyLaneCompositionData, DeserializeModifyLaneCompositionData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.UpdateTreeNodeStateData), EventCode.STRAT_MODIFY_NODE, SerializeUpdateTreeNodeStateData, DeserializeUpdateTreeNodeStateData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.LaunchInstantData), EventCode.STRAT_LAUNCH_INSTANT, SerializeLaunchInstantData, DeserializeLaunchInstantData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.LaunchToPositionData), EventCode.STRAT_LAUNCH_TO_POSITION, SerializeLaunchToPositionData, DeserializeLaunchToPositionData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.LaunchToTargetData), EventCode.STRAT_LAUNCH_TO_TARGET, SerializeLaunchToTargetData, DeserializeLaunchToTargetData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.SpawnMinionData), EventCode.STRAT_SPAWN_MINION, SerializeSpawnMinionData, DeserializeSpawnMinionData);
        PhotonPeer.RegisterType(typeof(RaiseEventStrategist.BuildTreeData), EventCode.STRAT_CREATE_TREE, SerializeBuildTreeData, DeserializeBuildTreeData);
    }

    #region Serialization

    static short SerializeHeroSpellLaunchedInfo(MemoryStream outStream, object customobject)
    {
        HeroSpellLaunchedInfo info = (HeroSpellLaunchedInfo)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + 4 + 4 + info.spellId.Length + sizeof(Spells.SpellInfo.e_CastType) + 12 * 3 + 4 + 4];

        Protocol.Serialize(info.casterPhotonId, bytes, ref index);
        Protocol.Serialize(info.index, bytes, ref index);
        Protocol.Serialize(info.spellId.Length, bytes, ref index);
        for (int i = 0; i < info.spellId.Length; i++)
        {
            bytes[index++] = (byte)info.spellId[i];
        }
        Protocol.Serialize((int)info.castType, bytes, ref index);
        WriteVector3ToBytesArray(bytes, info.casterPosition, ref index);
        WriteVector3ToBytesArray(bytes, info.casterRotation, ref index);
        WriteVector3ToBytesArray(bytes, info.spellPosition, ref index);
        Protocol.Serialize(info.target, bytes, ref index);
        Protocol.Serialize(info.chargeStart, bytes, ref index);
        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }

    static short SerializeModifyLaneCompositionData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.ModifyLaneCompositionData mlcd = (RaiseEventStrategist.ModifyLaneCompositionData)customobject;

        int index = 0;
        byte[] bytes = new byte[sizeof(int) + 4 + mlcd.newCompo.Length * sizeof(UnitsInfo.e_UnitType) + 4];

        Protocol.Serialize(mlcd.lineID, bytes, ref index);
        Protocol.Serialize(mlcd.newCompo.Length, bytes, ref index);
        foreach (var v in mlcd.newCompo)
        {
            Protocol.Serialize((int)v, bytes, ref index);
        }

        Protocol.Serialize(mlcd.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }

    static short SerializeUpdateTreeNodeStateData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.UpdateTreeNodeStateData utnsd = (RaiseEventStrategist.UpdateTreeNodeStateData)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + 4 + 4 + 4]; // int, int, int, int

        Protocol.Serialize(utnsd.newNodeLevel, bytes, ref index);
        Protocol.Serialize(utnsd.nodeID, bytes, ref index);
        Protocol.Serialize((int)utnsd.op, bytes, ref index);

        Protocol.Serialize(utnsd.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);
        return ((short)bytes.Length);
    }

    static short SerializeLaunchInstantData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.LaunchInstantData lid = (RaiseEventStrategist.LaunchInstantData)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + lid.spellId.Length + 4];

        Protocol.Serialize(lid.spellId.Length, bytes, ref index);
        for (int i = 0; i < lid.spellId.Length; i++)
        {
            bytes[index++] = (byte)lid.spellId[i];
        }

        Protocol.Serialize(lid.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);
        return ((short)bytes.Length);
    }

    static short SerializeLaunchToPositionData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.LaunchToPositionData ltpd = (RaiseEventStrategist.LaunchToPositionData)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + ltpd.spellId.Length + 12 + 4]; // str + V3 + int

        Protocol.Serialize(ltpd.spellId.Length, bytes, ref index);
        for (int i = 0; i < ltpd.spellId.Length; i++)
        {
            bytes[index++] = (byte)ltpd.spellId[i];
        }

        WriteVector3ToBytesArray(bytes, ltpd.position, ref index);

        Protocol.Serialize(ltpd.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);
        return ((short)bytes.Length);
    }

    static short SerializeLaunchToTargetData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.LaunchToTargetData lttd = (RaiseEventStrategist.LaunchToTargetData)customobject;

        if (lttd.heroTargets == null)
        {
            lttd.heroTargets = new int[0];
        }
        if (lttd.minionTargets == null)
        {
            lttd.minionTargets = new int[0];
        }

        int index = 0;
        byte[] bytes = new byte[4 + lttd.spellId.Length + 4 + lttd.heroTargets.Length * 4 + 4 + lttd.minionTargets.Length * 4 + 4]; // str + array1[int] + array2[int] + int

        Protocol.Serialize(lttd.spellId.Length, bytes, ref index);
        for (int i = 0; i < lttd.spellId.Length; i++)
        {
            bytes[index++] = (byte)lttd.spellId[i];
        }

        Protocol.Serialize(lttd.heroTargets.Length, bytes, ref index);
        foreach (var v in lttd.heroTargets)
        {
            Protocol.Serialize(v, bytes, ref index);
        }

        Protocol.Serialize(lttd.minionTargets.Length, bytes, ref index);
        foreach (var v in lttd.minionTargets)
        {
            Protocol.Serialize(v, bytes, ref index);
        }

        Protocol.Serialize(lttd.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);
        return ((short)bytes.Length);
    }

    static short SerializeSpawnMinionData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.SpawnMinionData smd = (RaiseEventStrategist.SpawnMinionData)customobject;

        int index = 0;
        byte[] bytes = new byte[4];

        Protocol.Serialize(smd.pid, bytes, ref index);
        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }

    static short SerializeBuildTreeData(MemoryStream outStream, object customobject)
    {
        RaiseEventStrategist.BuildTreeData data = (RaiseEventStrategist.BuildTreeData)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + data.treeId.Length * 2 * 4 + 4];

        Protocol.Serialize(data.treeId.Length, bytes, ref index);
        for (int i = 0; i < data.treeId.Length; i++)
        {
            Protocol.Serialize(data.treeId[i], bytes, ref index);
        }
        for (int i = 0; i < data.treeRank.Length; i++)
        {
            Protocol.Serialize(data.treeRank[i], bytes, ref index);
        }

        Protocol.Serialize(data.pid, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }

    static short SerializePowerUpInfo(MemoryStream outStream, object customobject)
    {
        PowerUpInfo info = (PowerUpInfo)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + 4];

        Protocol.Serialize(info.pwuId, bytes, ref index);
        Protocol.Serialize(info.pwuType, bytes, ref index);

        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }

    static short SerializeDoorInfo(MemoryStream outStream, object customobject)
    {
        DoorInfo info = (DoorInfo)customobject;

        int index = 0;
        byte[] bytes = new byte[4 + 4];

        Protocol.Serialize(info.doorId, bytes, ref index);
        outStream.Write(bytes, 0, bytes.Length);

        return ((short)bytes.Length);
    }
    #endregion

    #region Deserialization

    static object DeserializeHeroSpellLaunchedInfo(MemoryStream inStream, short length)
    {
        HeroSpellLaunchedInfo info = new HeroSpellLaunchedInfo();

        int index = 0;
        byte[] bytes = new byte[length];
        int spellIdLength;
        int tmpCastType;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out info.casterPhotonId, bytes, ref index);
        Protocol.Deserialize(out info.index, bytes, ref index);
        Protocol.Deserialize(out spellIdLength, bytes, ref index);
        info.spellId = "";
        for (int i = 0; i < spellIdLength; i++)
        {
            info.spellId += (char)bytes[index++];
        }
        Protocol.Deserialize(out tmpCastType, bytes, ref index);
        info.castType = (Spells.SpellInfo.e_CastType)tmpCastType;
        info.casterPosition = ReadVector3FromBytesArray(bytes, ref index);
        info.casterRotation = ReadVector3FromBytesArray(bytes, ref index);
        info.spellPosition = ReadVector3FromBytesArray(bytes, ref index);
        Protocol.Deserialize(out info.target, bytes, ref index);
        Protocol.Deserialize(out info.chargeStart, bytes, ref index);

        return info;
    }

    static object DeserializeModifyLaneCompositionData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.ModifyLaneCompositionData mlcd = new RaiseEventStrategist.ModifyLaneCompositionData();

        int index = 0;
        byte[] bytes = new byte[length];

        int arrayLength;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out mlcd.lineID, bytes, ref index);
        Protocol.Deserialize(out arrayLength, bytes, ref index);

        UnitsInfo.e_UnitType[] newCompo = new UnitsInfo.e_UnitType[arrayLength];

        for (int i = 0; i < arrayLength; i++)
        {
            int tmp;
            Protocol.Deserialize(out tmp, bytes, ref index);
            newCompo[i] = (UnitsInfo.e_UnitType)tmp;
        }

        mlcd.newCompo = newCompo.Clone() as UnitsInfo.e_UnitType[];

        Protocol.Deserialize(out mlcd.pid, bytes, ref index);

        return mlcd;
    }

    static object DeserializeUpdateTreeNodeStateData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.UpdateTreeNodeStateData utnsd = new RaiseEventStrategist.UpdateTreeNodeStateData();

        int index = 0;
        byte[] bytes = new byte[length];

        int tmp;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out utnsd.newNodeLevel, bytes, ref index);
        Protocol.Deserialize(out utnsd.nodeID, bytes, ref index);
        Protocol.Deserialize(out tmp, bytes, ref index);

        Protocol.Deserialize(out utnsd.pid, bytes, ref index);

        utnsd.op = (SkillTree.TreeNode.e_LevelOperation)tmp;

        return utnsd;
    }

    static object DeserializeLaunchInstantData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.LaunchInstantData lid = new RaiseEventStrategist.LaunchInstantData();

        int index = 0;
        byte[] bytes = new byte[length];

        int spellIdLength;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out spellIdLength, bytes, ref index);
        lid.spellId = "";
        for (int i = 0; i < spellIdLength; i++)
        {
            lid.spellId += (char)bytes[index++];
        }

        Protocol.Deserialize(out lid.pid, bytes, ref index);

        return lid;
    }

    static object DeserializeLaunchToPositionData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.LaunchToPositionData ltpd = new RaiseEventStrategist.LaunchToPositionData();

        int index = 0;
        byte[] bytes = new byte[length];

        int spellIdLength;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out spellIdLength, bytes, ref index);
        ltpd.spellId = "";
        for (int i = 0; i < spellIdLength; i++)
        {
            ltpd.spellId += (char)bytes[index++];
        }

        ltpd.position = ReadVector3FromBytesArray(bytes, ref index);

        Protocol.Deserialize(out ltpd.pid, bytes, ref index);

        return ltpd;
    }

    static object DeserializeLaunchToTargetData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.LaunchToTargetData lttd = new RaiseEventStrategist.LaunchToTargetData();

        int index = 0;
        byte[] bytes = new byte[length];

        int spellIdLength;

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out spellIdLength, bytes, ref index);
        lttd.spellId = "";
        for (int i = 0; i < spellIdLength; i++)
        {
            lttd.spellId += (char)bytes[index++];
        }

        int tmp;
        int[] tab;

        Protocol.Deserialize(out tmp, bytes, ref index);
        tab = new int[tmp];
        for (int i = 0; i < tmp; i++)
        {
            int tmp2;
            Protocol.Deserialize(out tmp2, bytes, ref index);
            tab[i] = tmp2;
        }
        lttd.heroTargets = tab.Clone() as int[];

        Protocol.Deserialize(out tmp, bytes, ref index);
        tab = new int[tmp];
        for (int i = 0; i < tmp; i++)
        {
            int tmp2;
            Protocol.Deserialize(out tmp2, bytes, ref index);
            tab[i] = tmp2;
        }
        lttd.minionTargets = tab.Clone() as int[];

        Protocol.Deserialize(out lttd.pid, bytes, ref index);

        return lttd;
    }

    static object DeserializeSpawnMinionData(MemoryStream inStream, short length)
    {
        RaiseEventStrategist.SpawnMinionData smd = new RaiseEventStrategist.SpawnMinionData();

        int index = 0;
        byte[] bytes = new byte[length];

        inStream.Read(bytes, 0, length);

        Protocol.Deserialize(out smd.pid, bytes, ref index);

        return smd;
    }

    static object DeserializeBuildTreeData(MemoryStream inStream, short length)
    {
        int index = 0;
        byte[] bytes = new byte[length];

        inStream.Read(bytes, 0, length);

        int arraySize;
        Protocol.Deserialize(out arraySize, bytes, ref index);

        RaiseEventStrategist.BuildTreeData data = new RaiseEventStrategist.BuildTreeData();
        data.treeId = new int[arraySize];
        data.treeRank = new int[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            Protocol.Deserialize(out data.treeId[i], bytes, ref index);
        }
        for (int i = 0; i < arraySize; i++)
        {
            Protocol.Deserialize(out data.treeRank[i], bytes, ref index);
        }

        Protocol.Deserialize(out data.pid, bytes, ref index);

        return data;
    }

    static object DeserializePowerUpInfo(MemoryStream inStream, short length)
    {
        PowerUpInfo info = new PowerUpInfo();

        int index = 0;
        byte[] bytes = new byte[length];

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out info.pwuId, bytes, ref index);
        Protocol.Deserialize(out info.pwuType, bytes, ref index);

        return info;
    }

    static object DeserializeDoorInfo(MemoryStream inStream, short length)
    {
        DoorInfo info = new DoorInfo();

        int index = 0;
        byte[] bytes = new byte[length];

        inStream.Read(bytes, 0, length);
        Protocol.Deserialize(out info.doorId, bytes, ref index);

        return info;
    }

    #endregion

    static void WriteVector3ToBytesArray(byte[] bytes, Vector3 v3, ref int index)
    {
        Protocol.Serialize(v3.x, bytes, ref index);
        Protocol.Serialize(v3.y, bytes, ref index);
        Protocol.Serialize(v3.z, bytes, ref index);
    }

    static Vector3 ReadVector3FromBytesArray(byte[] bytes, ref int index)
    {
        Vector3 v3 = new Vector3();

        Protocol.Deserialize(out v3.x, bytes, ref index);
        Protocol.Deserialize(out v3.y, bytes, ref index);
        Protocol.Deserialize(out v3.z, bytes, ref index);
        return (v3);
    }
}
