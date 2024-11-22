using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyCurrentHealth;

    [SerializeField] Transform enemyFront;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] float enemyMaxHealth;
    [SerializeField] float enemyDamage;
    [SerializeField] float damageOverTimeValue;
    [SerializeField] float timerDoT;

    GameObject player;

    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        StartCoroutine(EnemyAttack());
    }

    private void Update()
    {
        EnemyDeath();
    }

    void EnemyDeath()
    {
        if (enemyCurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator EnemyAttack()
    {
        while (true)
        {
            Debug.DrawLine(enemyFront.position, enemyFront.position + transform.forward * 1f, Color.green);


            if (Physics.Raycast(enemyFront.position, transform.forward, out RaycastHit hit, 1f, playerLayerMask))
            {
                hit.collider.GetComponent<Player>().GetDamage(enemyDamage, damageOverTimeValue, timerDoT);
                yield return new WaitForSeconds(5f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
