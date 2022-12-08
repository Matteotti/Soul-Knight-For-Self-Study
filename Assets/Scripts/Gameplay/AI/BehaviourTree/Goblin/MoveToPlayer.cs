using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveToPlayer : Action
{
    public SharedGameObject player;
    public SharedTransform target;
    public override void OnStart()
    {
        target.Value = GameObject.Find("Target").transform;
        player.Value = GameObject.Find("Player");
        target.Value.transform.position = player.Value.transform.position;
    }
}