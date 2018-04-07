using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : Singleton<AlarmScript>
{
    public bool isAlarm;
    public float alarmTime = 10f;
    private bool alarmActivated;

    public void AlarmActivate()
    {
        if (!alarmActivated)
        {
            StartCoroutine(AlarmCoroutine());
            alarmActivated = true;
        }
    }


    IEnumerator AlarmCoroutine()
    {
        isAlarm = true;
        yield return new WaitForSeconds(alarmTime);
        isAlarm = false;
        alarmActivated = false;

    }
}