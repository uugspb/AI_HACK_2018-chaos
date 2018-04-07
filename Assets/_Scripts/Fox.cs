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
        startingPoint = transform.position;
    }

    public void StartWalking()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void StopWalking()
    {
        agent.Warp(startingPoint);
        agent.isStopped = true;
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