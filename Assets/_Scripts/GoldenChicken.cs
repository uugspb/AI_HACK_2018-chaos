using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenChicken : MonoBehaviour {
	public float rotationSpeed;
	// Update is called once per frame
	void Update () {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}
}