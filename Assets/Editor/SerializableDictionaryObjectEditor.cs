using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SerializableDictionaryObject))]
public class SerializableDictionaryObjectEditor : Editor
{
    private string newKey;
    private float newValue;

    public override void OnInspectorGUI()
    {
        float keyWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 60f) / 2.25f;

        serializedObject.Update();
        // SerializedProperty dictionary = serializedObject.FindProperty("dictionary");
        SerializedProperty keys = serializedObject.FindProperty("keys");
        SerializedProperty values = serializedObject.FindProperty("values");

        EditorGUILayout.LabelField("Dictionary Contents", EditorStyles.boldLabel);

        for (int i = 0; i < keys.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i), GUIContent.none, GUILayout.Width(keyWidth));
            EditorGUILayout.PropertyField(values.GetArrayElementAtIndex(i), GUIContent.none, GUILayout.Width(keyWidth));

            //EditorGUILayout.LabelField(keysList[i]);
            //EditorGUILayout.LabelField(valuesList[i].ToString());

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(40)))
            {
                RemoveItem(keys, values, i);
                //keys.DeleteArrayElementAtIndex(i);
                //values.DeleteArrayElementAtIndex(i);
                // dictionary.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Add New Entry", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        newKey = EditorGUILayout.TextField("Key", newKey); //, GUILayout.Width(keyWidth));
        // string newKey = EditorGUILayout.TextField("Key", "", GUILayout.Width(keyWidth));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        newValue = EditorGUILayout.FloatField("Value", newValue); //, GUILayout.Width(keyWidth));
        if (newValue < 0 || newValue > 1)
            newValue = 0;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add", GUILayout.Width(keyWidth)))
        {
            AddItem(keys, values);
        }

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
    private void AddItem(SerializedProperty keys, SerializedProperty values)
    {
        keys.InsertArrayElementAtIndex(keys.arraySize);
        values.InsertArrayElementAtIndex(values.arraySize);
        keys.GetArrayElementAtIndex(keys.arraySize - 1).stringValue = newKey;
        values.GetArrayElementAtIndex(values.arraySize - 1).floatValue = newValue;

        //keysList.Add(newKey);
        //valuesList.Add(newValue);
    }
    private void RemoveItem(SerializedProperty keys, SerializedProperty values, int i)
    {
        keys.DeleteArrayElementAtIndex(i);
        values.DeleteArrayElementAtIndex(i);

        //keysList.Remove(keysList[i]);
        //valuesList.Remove(valuesList[i]);
    }
}