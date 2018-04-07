﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : Singleton<Fox>
{    
    public event Action OnFoxKilled;

    [SerializeField] private List<Transform> _startPositions;

    public Transform target;
    public NavMeshSurface foxSurface;
    private NavMeshAgent agent;
    private int _lastSpawnPointIndex = -1;
    private Vector3 _defaultStartPosition;


    void Start()
    {
        if(_startPositions == null || _startPositions.Count == 0)
        {
            _defaultStartPosition = transform.position;
        }
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartWalking()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void StopWalking()
    {
        agent.Warp(GetRandomPosition());
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
        agent.Warp(GetRandomPosition());
        agent.SetDestination(target.position);
    }
[EditorButton]
    public void ResetDestination()
    {
        agent.SetDestination(target.position);
    }

    private Vector3 GetRandomPosition()
    {
        if(_startPositions.Count == 0)
        {
            return _defaultStartPosition;
        }
        else
        {
            int index = _lastSpawnPointIndex;
            do
            {
                index = UnityEngine.Random.Range(0, _startPositions.Count);
            }
            while (_lastSpawnPointIndex == index && _startPositions.Count > 1);
            _lastSpawnPointIndex = index;

            return _startPositions[index].position;
        }        
    }

    void Update()
    {
    }
}