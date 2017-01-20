using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(UnitEntity))]
[System.Serializable]
public class UnitEntityEditor : Editor
{

    public override void OnInspectorGUI()
    {
        UnitEntity entity = (UnitEntity)target;
        EditorGUILayout.Space();
        entity.type = (UnitsInfo.e_UnitType)EditorGUILayout.EnumPopup("Unit Type", entity.type);
        entity.ArmorType = (Entity.e_ArmorType)EditorGUILayout.EnumPopup("Armor Type", entity.ArmorType);
        entity.Team = (e_Team)EditorGUILayout.EnumPopup("Team", entity.Team);
        entity.EntityName = EditorGUILayout.TextField("Entity Name", entity.EntityName);
        entity.GoldWorth = EditorGUILayout.IntField("Gold Worth", entity.GoldWorth);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("STATS");
        EditorGUILayout.Space();
        Dictionary<Entity.e_StatType, float> stats = new Dictionary<Entity.e_StatType, float>();
        foreach (string name in System.Enum.GetNames(typeof(Entity.e_StatType)))
        {
            stats[(Entity.e_StatType)System.Enum.Parse(typeof(Entity.e_StatType), name)] = EditorGUILayout.FloatField(name, entity.editorGetStat((Entity.e_StatType)System.Enum.Parse(typeof(Entity.e_StatType), name)));
            entity.editorSetStat((Entity.e_StatType)System.Enum.Parse(typeof(Entity.e_StatType), name), stats[(Entity.e_StatType)System.Enum.Parse(typeof(Entity.e_StatType), name)]);
        }
        EditorUtility.SetDirty(entity);
    }
}
