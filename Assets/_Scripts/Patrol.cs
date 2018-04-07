using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public List<Vector3> points;
    private LineRenderer line;
    private NavMeshAgent agent;
    private int nextPointIndex = 1;
    [SerializeField] private FovSource _me;

    void Start()
    {
        _me = GetComponentInChildren<FovSource>();
        _me.OnOpponentVisible += ActivateAlarm ;
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.positionCount = points.Count;
        line.SetPositions(points.Select(x=>x+Vector3.up).ToArray());
    }

    private bool foxkilled = false;
    public void ActivateAlarm(FovTarget first, FovTarget second)
    {
       //AlarmScript.instance.AlarmActivate();
        if (!foxkilled)
        {
            GameManager.instance.KillFox();
            foxkilled = true;
        }

        print("ALARM");
    }

    public void StartAgentPatrol()
    {
        line.enabled = false;
        agent.isStopped = false;
        StartCoroutine(ResetFoxKilledDelay());
        agent.SetDestination(points[nextPointIndex % 3]);
    }

    public void StopAgentPatrol()
    {
        agent.Warp(points[0]);
        agent.speed = 3.5f;
        line.enabled = true;
        nextPointIndex = 1;
        agent.isStopped = true;
        StartCoroutine(ResetFoxKilledDelay());

    }

    IEnumerator ResetFoxKilledDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, points[nextPointIndex % 3]) < 1.5f)
        {
            nextPointIndex++;
            agent.SetDestination(points[nextPointIndex % 3]);
        }

        if (AlarmScript.instance.isAlarm)
            agent.speed = 6f;
        else
        {
            agent.speed = 3.5f;
        }

    }
}