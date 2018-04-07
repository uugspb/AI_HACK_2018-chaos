using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : Singleton<Fox>
{
    public Transform target;
    public NavMeshSurface foxSurface;
    private NavMeshAgent agent;

    private Vector3 startingPoint;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        startingPoint = transform.position;
    }

    [EditorButton]
    public void Kill()
    {
        DeathStrandingManager.instance.SetKillingPoint(transform.position - Vector3.up);
        agent.Warp(startingPoint);
        agent.SetDestination(target.position);
    }

    public void ResetDestination()
    {
        agent.SetDestination(target.position);
    }

    void Update()
    {
    }
}