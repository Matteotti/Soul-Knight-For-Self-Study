using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPlayer : StateBase
{
    public MandalaController controller;
    private float counter;
    public override void Init(Enum stateType, FSMBehaviour behaviour)
    {
        this.stateType = stateType;
        controller = behaviour as MandalaController;
    }
    public override void OnEnter()
    {
        counter = 0;
    }
    public override void OnExit()
    {
        counter = 0;
        controller.GrabPlayerEnd();
    }
    public override void OnUpdate()
    {
        counter += Time.deltaTime;
        controller.GrabPlayer();
        if(counter > controller.grabContinuousTime)
        {
            controller.ChangeState(MandalaController.MandalaAttackState.Waiting);
        }
    }
}
