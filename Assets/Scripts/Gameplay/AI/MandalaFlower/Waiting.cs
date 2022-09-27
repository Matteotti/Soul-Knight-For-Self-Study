using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting : StateBase
{
    public MandalaController controller;
    public override void Init(Enum stateType, FSMBehaviour behaviour)
    {
        this.stateType = stateType;
        controller = behaviour as MandalaController;
    }
    public override void OnEnter()
    {

    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        controller.UpdateState();
    }
}