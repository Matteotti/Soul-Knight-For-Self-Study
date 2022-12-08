using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBT;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MyBTTree : MonoBehaviour
{
    protected BTNode _root = null;
    [HideInInspector] public MyDatabase database;
    [HideInInspector] public bool isRunning = true;
    public const string RESET = "Rest";
    private static int _resetId;
    private void Awake()
    {
        Init();
        _root.Activate(database);
    }
    private void Update()
    {
        if (!isRunning) return;
        if (database.GetData<bool>(database.GetDataId(RESET)))
        {
            Reset();
            database.SetData(RESET, false);
        }

        if (_root.Evaluate())
        {
            _root.Tick();
        }
    }
    private void OnDestroy()
    {
        if (_root != null)
        {
            _root.Clear();
        }
    }

    protected virtual void Init()
    {
        database = GetComponent<MyDatabase>();
        if (database == null)
        {
            database = gameObject.AddComponent<MyDatabase>();
        }
        database.SetData<bool>(database.GetDataId(RESET), false);
    }
    protected void Reset()
    {
        if (_root != null)
        {
            _root.Clear();
        }
    }
}
