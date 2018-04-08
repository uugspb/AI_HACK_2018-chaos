using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : Singleton<Fox>
{    
    public event Action OnFoxKilled;
    public event Action OnFoxGoalReached;
    
    [SerializeField] private List<Transform> _startPositions;
    [SerializeField] private ParticleSystem _spawnParticles;
    [SerializeField] private AudioSource _shot;

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
        _spawnParticles.Play();
        agent.isStopped = true;
    }
    
    [EditorButton]
    public void Kill()
    {
        if(_killRoutine == null)
        {
            _shot.Play();
            _killRoutine = StartCoroutine(KillDelayed());
        }
    }

    private Coroutine _killRoutine;

    private IEnumerator KillDelayed()
    {        
        yield return new WaitForSeconds(1f);

        if (OnFoxKilled != null)
        {
            OnFoxKilled.Invoke();
        }

        PatrolManager.instance.StopPatrol();
        PatrolManager.instance.StartPatrol();
        DeathStrandingManager.instance.SetKillingPoint(transform.position - Vector3.up);
        agent.Warp(GetRandomPosition());
        _spawnParticles.Play();
        agent.SetDestination(target.position);

        _killRoutine = null;
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
        if(_killRoutine == null && Vector3.Distance(transform.position, target.transform.position) < 2f)
        {
            if(OnFoxGoalReached != null)
            {
                GameManager.instance.ChangeTimeScale(1.0f);
                OnFoxGoalReached.Invoke();
            }
        }
    }
}