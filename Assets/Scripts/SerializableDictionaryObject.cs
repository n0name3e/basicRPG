using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Serializable Dictionary", menuName = "Custom/Serializable Dictionary")]
public class SerializableDictionaryObject : ScriptableObject
{
    public List<string> keys = new List<string>();
    public List<float> values = new List<float>();

    // The actual dictionary
    public Dictionary<string, float> dictionary = new Dictionary<string, float>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in dictionary)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<string, float>();
        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            dictionary[keys[i]] = values[i]; // Or Add, depending on your needs
        }
    }
}
