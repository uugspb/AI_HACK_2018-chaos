using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {

	[SerializeField]
	float currentTime = 0f;

	[SerializeField]
	float startTime = 0f;

	[SerializeField]
	float timeScale = 1;

	public float TimeScale
	{
		get { return timeScale; }
		set { timeScale = value; }
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += timeScale * Time.deltaTime;
	}
}
