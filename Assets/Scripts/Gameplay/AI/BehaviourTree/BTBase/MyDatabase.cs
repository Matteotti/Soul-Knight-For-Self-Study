using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Database is the blackboard in a classic blackboard system. 
/// (I found the name "blackboard" a bit hard to understand so I call it database ;p)
/// 
/// It is the place to store data from local nodes, cross-tree nodes, and even other scripts.
/// Nodes can read the data inside a database by the use of a string, or an int id of the data.
/// The latter one is prefered for efficiency's sake.
/// </summary>
public class MyDatabase : MonoBehaviour
{
    private List<object> database = new List<object>();
    private List<String> dataNames = new List<String>();
    //感觉像个字典，为什么不用字典呢？
    public T GetData<T>(int dataId)
    {
        if (MyBT.BTConfiguration.ENABLE_DATABASE_LOG)
        {
            Debug.Log("Database: getting data for " + dataNames[dataId]);
        }
        return (T)database[dataId];
    }
    public void SetData<T>(string dataName, T data)
    {
        int dataId = GetDataId(dataName);
        database[dataId] = data;
    }

    public void SetData<T>(int dataId, T data)
    {
        database[dataId] = data;
    }
    public int GetDataId(string dataName)
    {
        int dataId = dataNames.IndexOf(dataName);
        if (dataId == -1)
        {
            dataId = dataNames.Count;
            dataNames.Add(dataName);
            database.Add(null);
        }
        return dataId;
    }
    public bool ContainsData(string dataName)
    {
        return dataNames.Contains(dataName);
    }
}