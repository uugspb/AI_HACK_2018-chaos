using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMaterial : MonoBehaviour {
	public Material brown;
	public Material brownWings;
	void Start () {
		if (Random.Range (0, 2) == 1) {
			var shared = GetComponent<Renderer> ().sharedMaterials;
			shared[1] = brown;
			shared[5] = brownWings;
			GetComponent<Renderer> ().sharedMaterials = shared;
		}
	}
}