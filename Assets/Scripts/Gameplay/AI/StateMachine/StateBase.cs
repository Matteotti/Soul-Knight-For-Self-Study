using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase//状态基类，待继承
{
    public Enum stateType;//状态的枚举
    public abstract void Init(Enum stateType, FSMBehaviour behaviour);//初始化，指定该状态基类用什么状态机以及自身状态
    public abstract void OnEnter();//进入状态时
    public abstract void OnExit();//退出状态时
    public abstract void OnUpdate();//处于状态时
}