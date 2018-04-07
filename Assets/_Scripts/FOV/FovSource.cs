using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FovSource : MonoBehaviour
{

    public event Action<FovTarget, FovTarget> OnOpponentVisible;

    [SerializeField] private FovTarget _me;
    [SerializeField] private float _fovAngle;
    private Collider _fovCollider;


    // Use this for initialization
    void Start()
    {
        _fovCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        var opponent = other.GetComponent<FovTarget>();
        if (opponent != null && opponent.Type != _me.Type)
        {        
            var opponentPosition = opponent.transform.position;
            var direction = (opponentPosition - transform.position).normalized;
            var distance =  (opponentPosition - transform.position).magnitude;

            float angle = Vector3.Angle(transform.forward, direction);
            if (Mathf.Abs(angle) >= _fovAngle)
            {
                return;
            }

            _gizmosP1 = transform.position;
            _gizmosP2 = direction;
            _gizmosP3 = distance;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, distance, 1))
            {
                var detectedOpponent = hit.collider.GetComponent<FovTarget>();
                if (detectedOpponent != null && detectedOpponent.Type != _me.Type)
                {
                    if (OnOpponentVisible != null)
                    {
                        OnOpponentVisible.Invoke(_me, detectedOpponent);
                    }
                }
            }
        }
    }


    private Vector3 _gizmosP1;
    private Vector3 _gizmosP2;
    private float   _gizmosP3;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position +  transform.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_gizmosP1, _gizmosP1 + _gizmosP2 * _gizmosP3);
        _gizmosP1 = Vector3.zero;
        _gizmosP2 = Vector3.zero;
    }
}
