using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour {

	[SerializeField] private bool _isAlarm;
	[SerializeField] private float _alarmTime = 10f;

	// Use this for initialization
	void Start () {
		_isAlarm = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isAlarm) 
		{
			PanicTime ();
		}
	}

	private void PanicTime()
	{
		_alarmTime -= Time.timeScale * Time.deltaTime;
	}
}
