using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {

	[SerializeField]
	float currentTime = 0f;

	[SerializeField]
	float startTime = 0f;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.timeScale * Time.deltaTime;

	}
}
