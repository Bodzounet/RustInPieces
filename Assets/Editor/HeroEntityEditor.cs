using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(HeroEntity))]
[System.Serializable]
public class HeroEntityEditor : Editor
{

    public override void OnInspectorGUI()
    {
        HeroEntity entity = (HeroEntity)target;
        EditorGUILayout.Space();
        entity.ArmorType = (Entity.e_ArmorType)EditorGUILayout.EnumPopup("Armor Type", entity.ArmorType);
        entity.Team = (e_Team)EditorGUILayout.EnumPopup("Team", entity.Team);
        entity.EntityName = EditorGUILayout.TextField("Entity Name", entity.EntityName);
        entity.IconName = EditorGUILayout.TextField("Icon Name", entity.IconName);
        entity.Gold = EditorGUILayout.IntField("Gold", entity.Gold);
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
