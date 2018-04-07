using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovTarget : MonoBehaviour {

    [SerializeField] private TargetType _targetType;

    public TargetType Type { get { return _targetType; } }

    public enum TargetType
    {
        FOX,
        DOG
    }
}
