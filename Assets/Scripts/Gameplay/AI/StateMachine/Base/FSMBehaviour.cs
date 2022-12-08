using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour//状态机类，继承monobehaviour是因为要用update方法
{
    protected StateBase state;//object现在处于的状态基类
    private Dictionary<string, StateBase> states = new Dictionary<string, StateBase>();//字典：键值对
    public void ChangeState(Enum stateType, bool cover = false)//转换状态，cover代表是否覆盖相同状态
    {
        if (state != null && state.stateType.Equals(stateType) && !cover) return;
        if (state != null) state.OnExit();
        state = GetState(stateType);//给一个枚举，返回一个类
        state.OnEnter();
    }
    public StateBase GetState(Enum stateType)//这样就不至于每一次都新建一个类，节省性能
    {
        var stateKey = stateType.ToString();//字典索引
        if(states.ContainsKey(stateKey)) return states[stateKey];
        var newState = Activator.CreateInstance(Type.GetType(stateKey)) as StateBase;//状态转换
        newState.Init(stateType, this);
        states.Add(stateKey, newState);
        return newState;
    }
    protected virtual void Update()
    {
        if(state != null) state.OnUpdate();
    }
}