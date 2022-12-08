using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Dodge : Action
{
    public override void OnStart()
    {
        GetComponent<DodgeBehaviour>().dodge = true;
    }
}