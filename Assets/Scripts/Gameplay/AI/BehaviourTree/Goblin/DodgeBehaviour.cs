using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBehaviour : MonoBehaviour
{
    //角色在这个状态时，会向着自己的左边或者右边移动，依据是左右的射线是否检测到了墙体
    //如果检测到了墙体，就会向着另一边移动，如果两边都检测到了墙体，就不移动
    //如果没有检测到墙体，就会往右边移动
    public float moveSpeed, initialMoveTime, moveTime;
    public float rayCheckDistance;
    public GameObject player;
    public bool dodge;

    private Vector3 forward, left;
    void Start()
    {
        player = GameObject.Find("Player");
        moveTime = initialMoveTime;
    }
    void Update()
    {
        forward = player.transform.position - transform.position;
        left = new Vector3(-forward.y, forward.x, 0);
        if (dodge)
        {
            GetComponent<AIDestinationSetter>().enabled = false;
            GetComponent<AILerp>().enabled = false;
            //检测左边的射线
            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, left, rayCheckDistance, 1 << 3);
            //检测右边的射线
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, -left, rayCheckDistance, 1 << 3);
            //如果左边的射线检测到了墙体，就向右边移动
            if (leftHit.collider != null)
            {
                //用刚体方法移动
                GetComponent<Rigidbody2D>().velocity = -left * moveSpeed;
            }
            //如果右边的射线检测到了墙体，就向左边移动
            else if (rightHit.collider != null)
            {
                //用刚体方法移动
                GetComponent<Rigidbody2D>().velocity = left * moveSpeed;
            }
            //如果两边的射线都没有检测到墙体，就向右边移动
            else
            {
                //用刚体方法移动
                GetComponent<Rigidbody2D>().velocity = -left * moveSpeed;
            }
            //如果移动的时间大于设定的时间，就返回成功
            if (moveTime > 0)
            {
                moveTime -= Time.deltaTime;
            }
            else
            {
                moveTime = initialMoveTime;
                dodge = false;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                Debug.Log("MoveEnd");
            }
        }
        else
        {
            GetComponent<AIDestinationSetter>().enabled = true;
            GetComponent<AILerp>().enabled = true;
        }
    }
}
