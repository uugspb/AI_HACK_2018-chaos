using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FovSource : MonoBehaviour {

    public event Action<FovTarget, FovTarget> OnOpponentVisible;

    [SerializeField] private FovTarget _me;
    [SerializeField] private GameObject _eyePoint;
    private Collider _fovCollider;

    private static readonly float _castDistance = 100;

    // Use this for initialization
    void Start () {
        _fovCollider = GetComponent<Collider>();
	}

    private void OnTriggerStay(Collider other)
    {
        var opponent = other.GetComponent<FovTarget>();
        if (opponent != null && opponent.Type != _me.Type)
        {            
            var opponentPosition = other.transform.position;
            var direction = (opponentPosition - _eyePoint.transform.position).normalized;

            RaycastHit hit;
            if(Physics.Raycast(_eyePoint.transform.position, direction,out hit, _castDistance))
            {
                var detectedOpponent = hit.collider.GetComponent<FovTarget>();
                if(detectedOpponent != null && detectedOpponent.Type != _me.Type)
                {
                    if(OnOpponentVisible != null)
                    {
                        OnOpponentVisible.Invoke(_me, detectedOpponent);
                    }                    
                }
            }
        }
    }
}
