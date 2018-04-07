using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolManager : Singleton<PatrolManager>
{
    public int maxPatrolAmount;
    public GameObject patrolPrefab;
    public Camera cam;

    public List<Patrol> patrols = new List<Patrol>();


    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    [EditorButton]
    public void CreatePatrol()
    {
        StartCoroutine(CreatePatrolCoroutine());
    }

    IEnumerator CreatePatrolCoroutine()
    {
        print("kek");
        List<Vector3> points = new List<Vector3>();
        while (points.Count != 3)
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
            print("kek1");

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
            {
                points.Add(hit.point);
                line.positionCount = points.Count;
                line.SetPositions(points.Select(x => x + Vector3.up).ToArray());
            }
            print("kek2");

            line.enabled = true;
            yield return null;
        }

        var patrol = Instantiate(patrolPrefab, points[0] + Vector3.up, Quaternion.identity).GetComponent<Patrol>();
        patrol.points = points;
        line.enabled = false;
        patrols.Add(patrol);
    }

    public void StartPatrol()
    {
        patrols.ForEach(x => x.StartAgentPatrol());
    }

    public void StopPatrol()
    {
        patrols.ForEach(x => x.StopAgentPatrol());
    }

    [EditorButton]
    public void ClearAllPatrols()
    {
        patrols.ForEach(x => Destroy(x.gameObject));
        patrols = new List<Patrol>();
    }
}