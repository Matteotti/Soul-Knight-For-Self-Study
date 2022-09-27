using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsGroup : MonoBehaviour
{
    public float flySpeed;
    public float spinSpeed;
    public Vector3 direction;
    private void Start()
    {
        direction = transform.right;
    }
    private void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity != flySpeed * (Vector2)direction)
            GetComponent<Rigidbody2D>().velocity = flySpeed * direction;
        transform.eulerAngles -= new Vector3(0, 0, spinSpeed * Time.deltaTime);
    }
}
