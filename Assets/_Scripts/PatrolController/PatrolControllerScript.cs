using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolControllerScript : MonoBehaviour {

	[SerializeField] private List<FovSource> _fovSources;

	[SerializeField] private float _timeToWaitBeforeKill = 1f;

	// Use this for initialization
	void Start () {
		
	}

	private void OnOpponentVisibleTest(FovTarget arg1, FovTarget arg2)
	{
		Debug.LogWarning(arg1.name + " can see " + arg2.name);
		if (arg2.gameObject.GetComponent<Fox> () != null)
			StartCoroutine (ReadyToFire (arg2));
	}

	// ждет некоторое время, после чего стреляет
	private IEnumerator ReadyToFire(FovTarget arg2)
	{
		yield return new WaitForSeconds (_timeToWaitBeforeKill);
		OpenFire (arg2);
	}
		
	private void OpenFire (FovTarget arg2)
	{
		arg2.gameObject.GetComponent<Fox> ().Kill();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(var source in _fovSources)
		{
			source.OnOpponentVisible += OnOpponentVisibleTest;
		}
	}
}
