using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CameraProjectionChange))]
public class GameCameraController : MonoBehaviour
{
    [SerializeField] private Transform _mapPoi;
    [SerializeField] private Transform _followPoi;
    [SerializeField] private float _switchTime;
    [SerializeField] private bool _isInitialMap;

    private Camera _camera;
    private CameraProjectionChange _proejctionChange;
    private float _lerpTime;
    private float _lerpValue;
    private bool _isLerpToMap;

    [ContextMenu("SwitchToMapMode")]
    public void SwitchToMapMode()
    {
        if (_camera.orthographic)
        {
            return;
        }

        _proejctionChange.SetOrthographic();
        _lerpTime = 0;
        _isLerpToMap = true;
    }

    [ContextMenu("SwitchToFoxMode")]
    [EditorButton]
    public void SwitchToFoxMode()
    {
        if (!_camera.orthographic)
        {
            return;
        }

        _proejctionChange.SetPerspective();
        _lerpTime = _switchTime;
        _isLerpToMap = false;
    }

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        _proejctionChange = GetComponent<CameraProjectionChange>();

        if(_isInitialMap)
        {
            _camera.transform.position = _mapPoi.position;
            _camera.transform.rotation = _mapPoi.rotation;
            _lerpTime = _switchTime;
            _isLerpToMap = true;
        }
        else
        {
            _camera.transform.position = _followPoi.position;
            _camera.transform.rotation = _followPoi.rotation;
            _lerpTime = 0;
            _isLerpToMap = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isLerpToMap)
        {
            _lerpTime += Time.deltaTime;
        }
        else
        {
            _lerpTime -= Time.deltaTime;
        }

        _lerpTime = Mathf.Clamp(_lerpTime, 0, _switchTime);
        _lerpValue = Mathf.SmoothStep(0, 1, _lerpTime / _switchTime);
    }

    private void LateUpdate()
    {
        var desiredPosition = Vector3.Lerp(_mapPoi.position, _followPoi.position, 1 - _lerpValue);
        var desiredRotation = Quaternion.Slerp(_mapPoi.rotation, _followPoi.rotation, 1 - _lerpValue);

        _camera.transform.position = desiredPosition;
        _camera.transform.rotation = desiredRotation;
    }
}