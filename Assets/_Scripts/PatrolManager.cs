using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolManager : Singleton<PatrolManager>
{
    public int MaxPatrolAmount = 5;
    public GameObject patrolPrefab;
    public Camera cam;
    [SerializeField] private GameObject _flagPrefab;
	[SerializeField] private GameObject _protectorPrefab;
	
	public delegate void FreePatrolsCountChanged(int count);

	public event FreePatrolsCountChanged OnPatrolCountChanged;
    

    public List<Patrol> patrols = new List<Patrol>();
	public List<GameObject> protectors = new List<GameObject>();


    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
		MaxPatrolAmount = 5;
    }

    [EditorButton]
    public void CreatePatrol()
    {
	    if (patrols.Count <= MaxPatrolAmount)
	    {
		    if (OnPatrolCountChanged != null)
		    {
			    OnPatrolCountChanged(MaxPatrolAmount - patrols.Count);
		    }
	    }
	    if (patrols.Count < MaxPatrolAmount)
	    {
		    StartCoroutine(CreatePatrolCoroutine());
	    }
    }

    IEnumerator CreatePatrolCoroutine()
    {
        List<Vector3> points = new List<Vector3>();
        List<GameObject> flags = new List<GameObject>();
        while (points.Count != 3)
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 1000, -1) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                points.Add(hit.point);
                line.positionCount = points.Count;
                line.SetPositions(points.Select(x => x + Vector3.up).ToArray());
                var flagInstance = Instantiate(_flagPrefab);
                flagInstance.transform.position = hit.point;
                flags.Add(flagInstance);

				var protectorInstance = Instantiate(_protectorPrefab);
				protectorInstance.transform.position = hit.point;
				protectors.Add(protectorInstance);
            }

            line.enabled = true;
            yield return null;
        }

        var patrol = Instantiate(patrolPrefab, points[0] + Vector3.up, Quaternion.identity).GetComponent<Patrol>();
        patrol.points = points;
        line.enabled = false;
        
        patrols.Add(patrol);
        foreach (var flag in flags)
        {
            Destroy(flag.gameObject);
        }

		CreatePatrol ();
    }

    public bool CheckCoordinatesForNewPatrol(Vector3 coordinate)
    {
        foreach (var patrol in patrols)
        {
            if ((patrol.gameObject.transform.position - coordinate).magnitude < LevelEditor.MinimalDiff)
            {
                return false;
            }
        }

        return true; ;
    }

    public void StartPatrol()
    {
		DisableProtectors ();
        patrols.ForEach(x =>
        {
            x.StartAgentPatrol();
            x.DisableProtector();
        });
		
    }

	public void DestroyProtectors ()
	{
		foreach (var protect in protectors)
		{
			Destroy(protect.gameObject);
		}
		protectors.Clear ();
	}

	private void DisableProtectors()
	{
		foreach (var protect in protectors)
		{
			protect.gameObject.SetActive (false);;
		}
	}

	private void EnableProtectors()
	{
		foreach (var protect in protectors)
		{
			protect.gameObject.SetActive (false);;
		}
	}

    public void StopPatrol()
    {
        patrols.ForEach(x => x.StopAgentPatrol());
		EnableProtectors ();
    }

    [EditorButton]
    public void ClearAllPatrols()
    {
	    if (OnPatrolCountChanged != null)
	    {
		    OnPatrolCountChanged(MaxPatrolAmount);
	    }
        patrols.ForEach(x => Destroy(x.gameObject));
        patrols = new List<Patrol>();

		DestroyProtectors ();
    }
}