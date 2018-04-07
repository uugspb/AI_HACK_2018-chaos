using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : Singleton<ChickenManager> {
	public List<Transform> waypoints;

	void OnDrawGizmos () {
		foreach (var waypoint in waypoints) {
			Gizmos.DrawSphere (waypoint.position, 0.25f);
		}
	}
}