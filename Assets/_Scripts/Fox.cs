using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : Singleton<Fox>
{
    
    public event Action OnFoxKilled;

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
        if(OnFoxKilled != null)
        {
            OnFoxKilled.Invoke();
        }

        DeathStrandingManager.instance.SetKillingPoint(transform.position - Vector3.up);
        agent.Warp(startingPoint);
        agent.SetDestination(target.position);
    }
[EditorButton]
    public void ResetDestination()
    {
        agent.SetDestination(target.position);
    }

    void Update()
    {
    }
}