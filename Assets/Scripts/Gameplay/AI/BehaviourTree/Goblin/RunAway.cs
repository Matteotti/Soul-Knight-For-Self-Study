using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : Action
{
    public SharedGameObject player;
    public SharedTransform target;
    public SharedFloat speed;
    public SharedFloat massiveSpeed;
    public SharedFloat targetDestinationDistance;
    public SharedFloat runAwayDistance;

    private Vector3 direction;
    private Vector3 runAwayDirection;
    private Vector3 runAwayPosition;
    private bool isReverseOver;

    public override void OnStart()
    {
        direction = player.Value.transform.position - gameObject.transform.position;
        runAwayDirection = -direction.normalized;
        runAwayPosition = gameObject.transform.position + runAwayDirection * runAwayDistance.Value;
        target.Value = GameObject.Find("Target").transform;
    }

    public override TaskStatus OnUpdate()
    {
        RaycastHit2D wallCheck = Physics2D.Raycast(gameObject.transform.position, runAwayDirection, runAwayDistance.Value + 1, 1 << 3);
        Debug.DrawRay(gameObject.transform.position, runAwayDirection * (runAwayDistance.Value + 1), Color.red);
        if (wallCheck.collider != null || isReverseOver)
        {
            runAwayPosition = player.Value.transform.position - runAwayDirection * (runAwayDistance.Value + (float)gameObject.GetComponent<BehaviorTree>().GetVariable("maxAllowedPlayerDistance").GetValue());
            target.Value.position = runAwayPosition;
            gameObject.GetComponent<AILerp>().speed = massiveSpeed.Value;
            isReverseOver = true;
        }
        else
        {
            target.Value.position = runAwayPosition;
            gameObject.GetComponent<AILerp>().speed = speed.Value;
        }
        if (Vector3.Distance(gameObject.transform.position, runAwayPosition) < targetDestinationDistance.Value)
        {
            isReverseOver = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}