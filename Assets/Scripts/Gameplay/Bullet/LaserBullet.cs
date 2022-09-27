using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public int damageToken;
    public float critChance;
    public float critDamage;
    public bool isCrit;
    public List<GameObject> targetEnemy = new List<GameObject>();
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!targetEnemy.Contains(collision.gameObject))
            {
                targetEnemy.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (targetEnemy.Contains(collision.gameObject))
            {
                targetEnemy.Remove(collision.gameObject);
            }
        }
    }
    public void CheckAttack()//攻击相应的敌人 造成伤害 调用gethurt函数 传递暴击信息
    {
        for (int i = 0; i < targetEnemy.Count; i++)
        {
            int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < critChance) ? critDamage : 1) * damageToken);
            if (bulletDamage == critDamage * damageToken)
                isCrit = true;
            if (targetEnemy[i] != null)
            {
                targetEnemy[i].GetComponent<EnemyBehaviour>().isCrit = isCrit;
                targetEnemy[i].SendMessage("BeHit", bulletDamage, SendMessageOptions.DontRequireReceiver);
            }
            isCrit = false;
        }
    }
}