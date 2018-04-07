using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour {

	private static AlarmScript instance;
	public static AlarmScript Instance 
	{
		get {
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<AlarmScript> ();
			}
			return AlarmScript.instance; }
	}


	[SerializeField] private bool _isAlarm;
	[SerializeField] private const float MAX_ALARM_TIME = 10f;
	[SerializeField] private float _alarmTime;

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

	public void AlarmActivate()
	{
		_alarmTime = MAX_ALARM_TIME;
		_isAlarm = !_isAlarm;
	}

	private void PanicTime()
	{
		_alarmTime -= Time.timeScale * Time.deltaTime;
	}
}
