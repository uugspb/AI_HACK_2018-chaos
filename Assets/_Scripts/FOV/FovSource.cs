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
    [SerializeField] private GameObject _eyePoint;
    private Collider _fovCollider;

    private static readonly string _agentLayer = "Default";
    private static int _agentLayerId = -1;

    // Use this for initialization
    void Start()
    {
        _fovCollider = GetComponent<Collider>();
        if (_agentLayerId < 0)
        {
            _agentLayerId = LayerMask.NameToLayer(_agentLayer);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var opponent = other.GetComponent<FovTarget>();
        if (opponent != null && opponent.Type != _me.Type)
        {
            var opponentPosition = other.transform.position;
            var direction = (opponentPosition - _eyePoint.transform.position).normalized;
            var distance = 100f;// (opponentPosition - _eyePoint.transform.position).magnitude;

            _gizmosP1 = _eyePoint.transform.position;
            _gizmosP2 = direction;
            _gizmosP3 = distance;

            RaycastHit hit;

            if (Physics.Raycast(_eyePoint.transform.position, direction, out hit, distance, 1))
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
    private float _gizmosP3;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_gizmosP1, _gizmosP2 * _gizmosP3);
        _gizmosP1 = Vector3.zero;
        _gizmosP2 = Vector3.zero;
    }
}
