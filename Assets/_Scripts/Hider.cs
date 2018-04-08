using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour {
	Transform rayStart;
	void Start () {
		rayStart = transform;
	}
	void Update () {
		var ray = new Ray (rayStart.position, Fox.instance.transform.position - rayStart.position);

		var raycastHits = Physics.RaycastAll (ray, Vector3.Distance (Fox.instance.transform.position, rayStart.position));
		foreach (var hit in raycastHits) {
			var hide = hit.collider.GetComponentInParent<OcclusionMeshFader> ();
			if (hide != null) {
				hide.Overlapping ();
			}
		}
	}
}