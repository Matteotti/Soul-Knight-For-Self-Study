using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<MandalaController> components = new List<MandalaController>();
    public List<EnemyAIController> enemies = new List<EnemyAIController>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (var item in gameObjects)
            {
                item.SetActive(true);
            }
            foreach (var item in components)
            {
                item.enabled = true;
            }
            foreach (var item in enemies)
            {
                item.enabled = true;
            }
        }
    }
}
