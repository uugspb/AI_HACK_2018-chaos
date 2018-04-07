using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovTest : MonoBehaviour {

    [SerializeField] private List<FovSource> _fovSources;

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
    }

    // Update is called once per frame
    void Update () {
		
	}
}
