using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomPropertyDrawer(typeof(MyDico))]
//[CustomPropertyDrawer(typeof(Toto))]
public class SerializableDicoEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        

        var rect = new Rect(position.x, position.y, 30, position.height);
        //var rect2 = new Rect(position.x + 50, position.y, 30, position.height);

        //EditorGUI.LabelField(rect, "size : ");
        //toto = EditorGUI.IntField(rect2, toto);

        //var keys = property.FindPropertyRelative("_keys");

        //var rect3 = new Rect(position.x, position.y + 20, 300, position.height);
        //var enumvalue = EditorGUI.EnumPopup(rect3, UnitsInfo.e_UnitType.FOOTY);

        ////keys.arraySize += 1;
        ////keys.InsertArrayElementAtIndex(0);

        var _keys = property.FindPropertyRelative("_keys");

        EditorGUI.PropertyField(rect, _keys);

        if (_keys.isExpanded)
        {
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < _keys.arraySize; i++)
            {
                Rect newPos = new Rect(position.x + 10, position.y + 20 * (i + 1), 50, position.y + 20 * (i + 1) + 20);
                EditorGUI.PropertyField(newPos, _keys.GetArrayElementAtIndex(i));
            }
            EditorGUI.indentLevel -= 1;
        }

        EditorGUI.EndProperty();
    }
}