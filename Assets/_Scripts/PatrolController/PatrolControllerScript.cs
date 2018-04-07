using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolControllerScript : MonoBehaviour {

	[SerializeField] private List<FovSource> _fovSources;

	[SerializeField] private float _timeToWait = 1;

	// Use this for initialization
	void Start () {
		foreach(var source in _fovSources)
		{
			source.OnOpponentVisible += OnOpponentVisibleTest;
		}
	}

	private void OnOpponentVisibleTest(FovTarget arg1, FovTarget arg2)
	{
		Debug.LogWarning(arg1.name + " can see " + arg2.name);
		StartCoroutine (ReadyToFire(arg2));
	}

	// ждет некоторое время, после чего стреляет
	private IEnumerator ReadyToFire(FovTarget arg2)
	{
		yield return new WaitForSeconds (_timeToWait);
		OpenFire (arg2);
	}
		
	private void OpenFire (FovTarget arg2)
	{

		//arg2.gameObject.GetComponent<Fox> ().isDead = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
