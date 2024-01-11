using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SerializedDictionary : SerializableDictionary<string, float> { }

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // The actual dictionary
    public Dictionary<TKey, TValue> dictionary { get; set; } = new Dictionary<TKey, TValue>();

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
        dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            dictionary[keys[i]] = values[i]; // Or Add, depending on your needs
        }
    }

    // Implement methods to interact with the dictionary
    public TValue GetValue(TKey key)
    {
        return dictionary[key];
    }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
    }

    // Other dictionary operations like Remove, ContainsKey, etc.
}
