using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpheres : MonoBehaviour
{
    [SerializeField] int bubbleScore;
    [SerializeField] float x2timer;
    [SerializeField] float x3timer;
    [SerializeField] float x5timer;

    GameManager gm;

    float timer;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            gm.score = gm.score + bubbleScore;
            gm.ammo++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }
}
