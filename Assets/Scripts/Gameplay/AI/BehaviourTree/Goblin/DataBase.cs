using BehaviorDesigner.Runtime.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public bool isPlayerDead;
    public bool isBulletAhead;
    public float playerDistance;
    public GameObject rayTarget;

    public GameObject player;
    public float maxBulletCheckDistance;
    public float bulletCheckDeltaDistance;
    public float bulletCheckFrontDistance;

    public BehaviorTree goblinBehaviorTree;
    private void Start()
    {
        player = GameObject.Find("Player");
        goblinBehaviorTree = GetComponent<BehaviorTree>();
        goblinBehaviorTree.SetVariableValue("player", player);
    }
    private void Update()
    {
        Vector3 forward, left, right;
        forward = (player.transform.position - transform.position).normalized;
        left = new Vector3(-forward.y, forward.x).normalized;
        right = -left;
        if (player.GetComponent<PlayerHPMPShield>() != null && player.GetComponent<PlayerHPMPShield>().HP > 0)
            isPlayerDead = false;
        else
            isPlayerDead = true;
        
        RaycastHit2D rayForBulletLeft, rayForBulletRight;
        rayForBulletLeft = Physics2D.Raycast(transform.position + bulletCheckDeltaDistance * left + bulletCheckFrontDistance * forward, left, maxBulletCheckDistance, 1 << 7);
        Debug.DrawRay(transform.position + bulletCheckDeltaDistance * left + bulletCheckFrontDistance * forward,
            left * maxBulletCheckDistance);
        rayForBulletRight = Physics2D.Raycast(transform.position + bulletCheckDeltaDistance * right + bulletCheckFrontDistance * forward, right, maxBulletCheckDistance, 1 << 7);
        Debug.DrawRay(transform.position + bulletCheckDeltaDistance * right + bulletCheckFrontDistance * forward,
            right * maxBulletCheckDistance);
        if ((rayForBulletLeft.collider != null && rayForBulletLeft.collider.CompareTag("Bullet")) || (rayForBulletRight.collider != null && rayForBulletRight.collider.CompareTag("Bullet")))
        {
            isBulletAhead = true;
        }
        else
            isBulletAhead = false;
        //if(rayForBulletLeft.collider != null)
        //    Debug.Log("Left " + rayForBulletLeft.collider);
        //if (rayForBulletRight.collider != null)
        //    Debug.Log("Right " + rayForBulletRight.collider);
        playerDistance = (player.transform.position - transform.position).magnitude;
        RaycastHit2D rayForPlayer;
        rayForPlayer = Physics2D.Raycast(transform.position, forward, int.MaxValue, ~(1 << 8 | 1 << 2 | 1 << 9));
        //Debug.DrawRay(transform.position, forward * 500, Color.red);
        rayTarget = rayForPlayer.collider.gameObject;

        goblinBehaviorTree.SetVariableValue("isPlayerDead", isPlayerDead);
        goblinBehaviorTree.SetVariableValue("isBulletAhead", isBulletAhead);
        goblinBehaviorTree.SetVariableValue("playerDistance", playerDistance);
        goblinBehaviorTree.SetVariableValue("rayTarget", rayTarget);
    }
}
