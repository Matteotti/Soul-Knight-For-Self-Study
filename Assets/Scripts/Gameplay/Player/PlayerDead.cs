using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    public float deadSpeed;
    public float minSpeed;
    public float slowDown;
    private Rigidbody2D rb;
    public GameObject deadMenu;
    public bool test;
    void OnEnable()
    {
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
        Destroy(transform.GetChild(2).gameObject);
        Destroy(GetComponent<Animator>());
        GetComponent<SpriteRenderer>().color = new Color (0.5f,0.5f,0.5f);
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        float angle = Random.Range(0, 2 * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * deadSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity *= slowDown;
        if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke(nameof(Destroy), 2);
        }
    }
    void Destroy()
    {
        Destroy(this);
        deadMenu.SetActive(true);
    }
}
