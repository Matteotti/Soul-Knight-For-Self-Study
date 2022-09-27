using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour//״̬���࣬�̳�monobehaviour����ΪҪ��update����
{
    protected StateBase state;//object���ڴ��ڵ�״̬����
    private Dictionary<string, StateBase> states = new Dictionary<string, StateBase>();//�ֵ䣺��ֵ��
    public void ChangeState(Enum stateType, bool cover = false)//ת��״̬��cover�����Ƿ񸲸���ͬ״̬
    {
        if (state != null && state.stateType.Equals(stateType) && !cover) return;
        if (state != null) state.OnExit();
        state = GetState(stateType);//��һ��ö�٣�����һ����
        state.OnEnter();
    }
    public StateBase GetState(Enum stateType)//�����Ͳ�����ÿһ�ζ��½�һ���࣬��ʡ����
    {
        var stateKey = stateType.ToString();//�ֵ�����
        if(states.ContainsKey(stateKey)) return states[stateKey];
        var newState = Activator.CreateInstance(Type.GetType(stateKey)) as StateBase;//״̬ת��
        newState.Init(stateType, this);
        states.Add(stateKey, newState);
        return newState;
    }
    protected virtual void Update()
    {
        if(state != null) state.OnUpdate();
    }
}