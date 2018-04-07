using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public List<Vector3> points;
    private LineRenderer line;
    private NavMeshAgent agent;
    private int nextPointIndex = 1;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.positionCount = points.Count;
        line.SetPositions(points.Select(x=>x+Vector3.up).ToArray());
    }

    public void StartAgentPatrol()
    {
        line.enabled = false;
        agent.isStopped = false;
        agent.SetDestination(points[nextPointIndex % 3]);
    }

    public void StopAgentPatrol()
    {
        agent.Warp(points[0]);
        line.enabled = true;
        nextPointIndex = 1;
        agent.isStopped = true;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, points[nextPointIndex % 3]) < 1.5f)
        {
            nextPointIndex++;
            agent.SetDestination(points[nextPointIndex % 3]);
        }
    }
}