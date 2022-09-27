using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase//״̬���࣬���̳�
{
    public Enum stateType;//״̬��ö��
    public abstract void Init(Enum stateType, FSMBehaviour behaviour);//��ʼ����ָ����״̬������ʲô״̬���Լ�����״̬
    public abstract void OnEnter();//����״̬ʱ
    public abstract void OnExit();//�˳�״̬ʱ
    public abstract void OnUpdate();//����״̬ʱ
}