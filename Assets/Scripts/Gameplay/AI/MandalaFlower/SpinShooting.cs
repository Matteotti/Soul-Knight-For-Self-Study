using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShooting : StateBase
{
    public MandalaController controller;
    private float overallCounter, shootCounter;
    public override void Init(Enum stateType, FSMBehaviour behaviour)
    {
        this.stateType = stateType;
        controller = behaviour as MandalaController;
    }
    public override void OnEnter()
    {
        controller.mandalaFlowerAttack.targetAngle = 0;
        controller.mandalaFlowerAttack.targetAngle = 0;
    }
    public override void OnExit()
    {
        overallCounter = 0;
        shootCounter = 0;
    }
    public override void OnUpdate()
    {
        if (overallCounter <= controller.spinShootMaintainTime - controller.mandalaFlower.foreSwingTime)
        {
            overallCounter += Time.deltaTime;
            shootCounter += Time.deltaTime;
            if (shootCounter >= controller.spinShootGap)
            {
                shootCounter = 0;
                controller.SpinShootBullet();
            }
        }
        else if (overallCounter > controller.spinShootMaintainTime - controller.mandalaFlower.foreSwingTime && overallCounter <= controller.spinShootMaintainTime)
        {
            overallCounter += Time.deltaTime;
            shootCounter += Time.deltaTime;
            if (shootCounter >= controller.spinShootGap)
            {
                shootCounter = 0;
                controller.mandalaFlowerAttack.targetAngle += controller.deltaAngle;
            }
        }
        else
        {
            controller.ChangeState(MandalaController.MandalaAttackState.Waiting);
        }
    }
}
