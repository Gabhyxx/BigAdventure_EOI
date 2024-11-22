using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Script : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] Transform player;

    [Header("Patrol")]
    [SerializeField] Transform[] points;

    Vector3 nextPoint;

    int indexPoints;

    [Header("Alert")]
    [SerializeField] float distance;

    NavMeshAgent agent;

    enum State { Patrol, Alert, Attack }
    State currentState = State.Patrol;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(Patrol());
    }

    private void Update()
    {
        ControlState();
    }

    void ControlState()
    {
        if (currentState == State.Patrol)
        {
            if (Vector3.Distance(transform.position, player.position) < distance)
            {
                StopAllCoroutines();
                StartCoroutine(Alert());
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) > distance)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            nextPoint = points[indexPoints].position + new Vector3(0, 0, Random.Range(-2.5f, 2.5f));


            agent.destination = nextPoint;

            while (Vector3.Distance(transform.position, nextPoint) > 1)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2);
            indexPoints++;

            if (indexPoints >= points.Length)
            {
                indexPoints = 0;
            }
        }
    }

    IEnumerator Alert()
    {
        currentState = State.Alert;

        while (true)
        {
            transform.LookAt(player.position);
            yield return new WaitForSeconds(1);

            Vector3 playerDirection = player.position - transform.position;
            if (Vector3.Angle(playerDirection, transform.forward) < 20)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attack;
        while (true)
        {
            while (Vector3.Distance(transform.position, player.position) > 1)
            {
                agent.SetDestination(player.position);
                yield return new WaitForEndOfFrame();
            }

            agent.velocity = Vector3.zero;
            yield return new WaitForSeconds(3);
        }
    }
}
