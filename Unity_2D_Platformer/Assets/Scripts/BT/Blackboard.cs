using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Blackboard
{
    private Dictionary<Type, IDictionary> blackboardDictionary = new Dictionary<Type, IDictionary>();

    public void InitializeBlackBoard(GameObject owner)
    {
        this.SetData("Owner", owner);
    }

    public void SetData<T>(string keyName, T value)
    {
        if (!blackboardDictionary.ContainsKey(typeof(T)))
        {
            blackboardDictionary.Add(typeof(T), new Dictionary<string, T>());
        }

        IDictionary dic = blackboardDictionary[typeof(T)];

        if (dic.Contains(keyName))
        {
            dic[keyName] = value;
        }
        else
        {
            dic.Add(keyName, value);
        }
    }

    public T GetData<T>(string keyName)
    {
        if (!blackboardDictionary.ContainsKey(typeof(T)))
        {
            Debug.LogError("There is no Key " + keyName);
            return default(T);
        }

        IDictionary dic = blackboardDictionary[typeof(T)];

        if (!dic.Contains(keyName))
        {
            Debug.LogError("There is no Key: " + keyName);
            return default(T);
        }

        return (T)dic[keyName];
    }
}
