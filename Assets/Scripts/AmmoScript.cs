using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<Enemy>().enemyCurrentHealth--;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}
