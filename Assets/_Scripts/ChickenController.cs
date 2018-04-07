using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour {
	NavMeshAgent m_agent;

	void Start () {
		m_agent = GetComponent<NavMeshAgent> ();
		MoveToWaypoint ();
	}

	void MoveToWaypoint () {
		m_agent.destination = ChickenManager.instance.waypoints[Random.Range (0, ChickenManager.instance.waypoints.Count)].position;
		Invoke ("MoveToWaypoint", Random.Range (5f, 10f));
	}
}