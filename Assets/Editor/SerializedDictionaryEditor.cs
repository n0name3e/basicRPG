using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SerializedDictionaryHolder))]
public class SerializedDictionaryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedDictionaryHolder serializdedDictionary = (SerializedDictionaryHolder)target;

        // If the dictionary is null, initialize it
        if (serializdedDictionary.serializedDictionary.dictionary == null)
            serializdedDictionary.serializedDictionary.dictionary = new Dictionary<string, float>();

        // Display the dictionary in a better format
        foreach (var item in serializdedDictionary.serializedDictionary.dictionary)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(item.Key.ToString());
            EditorGUILayout.LabelField(item.Value.ToString());
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("meow");
        EditorGUILayout.LabelField("kitten");
        EditorGUILayout.EndHorizontal();

        // Add controls for adding new items or other functionalities

        // Always call this at the end
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}