using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    private string newKey;   // Replace TKey with the actual type you are using for keys
    private float newValue; // Replace TValue with the actual type you are using for values
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // This draws the default inspector

        Enemy enemy = (Enemy)target;

        if (enemy.dropTable == null)
        {
            enemy.dropTable = new SerializedDictionary();
        }

        // Custom UI for SerializedDictionary
        if (enemy.dropTable != null)
        {
            EditorGUILayout.LabelField("Serialized Dictionary", EditorStyles.boldLabel);

            // Example: Display dictionary as labels, or add controls to modify it
            foreach (var item in enemy.dropTable.dictionary)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item.Key.ToString());
                EditorGUILayout.LabelField(item.Value.ToString());         
                EditorGUILayout.EndHorizontal();
            }
            /*for (int i = 0; i < enemy.dropTable.dictionary.Count; i++)
            {
                if (i >= enemy.dropTable.dictionary.Count) break;                
                var item = enemy.dropTable.dictionary.ElementAt(i);
                Debug.Log(item.Key.ToString());

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item.Key.ToString());
                EditorGUILayout.LabelField(item.Value.ToString());
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Remove"))
                {
                    RemoveItem(enemy.dropTable, item.Key.ToString());
                    i--;
                }
            }*/
            // Add more controls as needed...

            if (GUI.changed)
            {
                EditorUtility.SetDirty(enemy);
            }
        }
        newKey = EditorGUILayout.TextField("Item name", newKey);
        newValue = EditorGUILayout.FloatField("Chance", newValue);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            AddItem(enemy.dropTable, newKey, newValue);
        }
        if (GUILayout.Button("Remove"))
        {
            RemoveItem(enemy.dropTable, newKey);
        }
        if (GUILayout.Button("Remove Last"))
        {
            RemoveLastItem(enemy.dropTable);
        }
        EditorGUILayout.EndHorizontal();
    }
    private void AddItem(SerializedDictionary dictionary, string key, float value)
    {
        if (!dictionary.dictionary.ContainsKey(key))
        {
            Debug.Log(dictionary.dictionary.Count + " pieces");
            dictionary.dictionary.Add(key, value);            
            EditorUtility.SetDirty(target); // Mark the object as dirty to ensure changes are saved
        }
        else
        {
            Debug.LogWarning("Key already exists.");
        }
    }
    private void RemoveItem(SerializedDictionary dictionary, string key)
    {
        if (dictionary.dictionary.ContainsKey(key))
        {
            dictionary.dictionary.Remove(dictionary.dictionary.ElementAt(dictionary.dictionary.Count - 1).Key);
            EditorUtility.SetDirty(target); // Mark the object as dirty to ensure changes are saved
        }
        else
        {
            Debug.LogWarning("Key not found.");
        }
    }
    private void RemoveLastItem(SerializedDictionary dictionary)
    {
        if (dictionary.dictionary.Count <= 0) return;
        Debug.Log("me");
        dictionary.dictionary.Remove(dictionary.dictionary.ElementAt(dictionary.dictionary.Count - 1).Key);
        EditorUtility.SetDirty(target); // Mark the object as dirty to ensure changes are saved
        /*if (dictionary.dictionary.ContainsKey(key))
        {
            dictionary.dictionary.Remove(dictionary.dictionary.ElementAt(dictionary.dictionary.Count - 1).Key);
            EditorUtility.SetDirty(target); // Mark the object as dirty to ensure changes are saved
        }
        else
        {
            Debug.LogWarning("Key not found.");
        }*/
    }
}