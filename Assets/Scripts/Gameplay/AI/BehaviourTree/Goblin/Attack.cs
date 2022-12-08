using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    public override void OnStart()
    {
        GetComponent<EnemyAttack>().StartAttack();
    }
}