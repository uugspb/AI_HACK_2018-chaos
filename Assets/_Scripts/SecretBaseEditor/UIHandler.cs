using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

	[SerializeField] private Button _addSentinel;
	[SerializeField] private Button _addCamera;
	[SerializeField] private Button _start;
	[SerializeField] private Text _cameraText;
	[SerializeField] private Text _sentinelText;
	[SerializeField] private LevelEditor _levelEditor;
	[SerializeField] private Text _info;
	[SerializeField] private Button _erase;
	[SerializeField] private Camera _camera;
	[SerializeField] private Button _openEditor;
	[SerializeField] private Text _title;

	private bool _isEditMode = true;
	
	private const string CameraText = "Cameras left: {0}";
	private const string SentinelText = "Sentinels left: {0}";
	private const float Speed = 5.0f;
	
	private Vector3 editorPosition = new Vector3(0, 10, 0);
	private Vector3 gamePosition = new Vector3(0, 5, -5);
	private Quaternion editorRotation = Quaternion.Euler(90, 0, 0);
	private Quaternion gameRotation = Quaternion.Euler(30, 0, 0);
	
	// Use this for initialization
	void Start ()
	{
		_levelEditor.OnConfigChanged += UpdateUI;
		_levelEditor.OnInfoChanged += UpdateInfo;
		_addSentinel.onClick.AddListener(delegate
		{
			_levelEditor.AddSentinel();
		}); 
		_addCamera.onClick.AddListener(delegate
		{
			_levelEditor.AddCamera();
		});
		_erase.onClick.AddListener(delegate
		{
			_levelEditor.Erase();
		});
		_start.onClick.AddListener(delegate
		{
			_isEditMode = false;
			_title.gameObject.active = false;
			_addSentinel.gameObject.active = false;
			_addCamera.gameObject.active = false;
			_start.gameObject.active = false;
			_cameraText.gameObject.active = false;
			_sentinelText.gameObject.active = false;
			_info.gameObject.active = false;
			_erase.gameObject.active = false;
			_openEditor.gameObject.active = true;
			StartCoroutine(ChangeCameraPos());
		});
		_openEditor.onClick.AddListener(delegate
		{
			_isEditMode = true;
			_title.gameObject.active = true;
			_addSentinel.gameObject.active = true;
			_addCamera.gameObject.active = true;
			_start.gameObject.active = true;
			_cameraText.gameObject.active = true;
			_sentinelText.gameObject.active = true;
			_info.gameObject.active = true;
			_erase.gameObject.active = true;
			_openEditor.gameObject.active = false;
			StartCoroutine(ChangeCameraPos());
		});
	}

	private void OnDestroy()
	{
		_levelEditor.OnConfigChanged -= UpdateUI;
		_levelEditor.OnInfoChanged -= UpdateInfo;
	}

	private void UpdateUI(InputLevelConfig inputConfig, OutputLevelConfig outputConfig)
	{
		_cameraText.text = string.Format(CameraText, inputConfig.MaxCameras - outputConfig.getCamerasCount());
		_sentinelText.text = string.Format(SentinelText, inputConfig.MaxSentinels - outputConfig.getSentinelCount());
		
		_addCamera.enabled = outputConfig.getCamerasCount() < inputConfig.MaxCameras;
		_addSentinel.enabled = outputConfig.getSentinelCount() < inputConfig.MaxSentinels;
	}


	private void UpdateInfo(string info)
	{
		_info.text = info;
	}
	
	// Update is called once per frame
	private IEnumerator ChangeCameraPos () {
		Vector3 target;
		Vector3 previousPosition;
		Quaternion targetRotation;
		Quaternion previousRotation;
		if (_isEditMode)
		{
			targetRotation = editorRotation;
			target = editorPosition;
		}
		else
		{
			targetRotation = gameRotation;
			target = gamePosition;
		}

		float t = 0f;
		var startingPos = _camera.transform.position;
		var startingRotation = _camera.transform.rotation;
		while (t < 1.0f)
		{
			t += Time.deltaTime * Speed;
			_camera.transform.position = Vector3.Lerp(startingPos, target, t);
			_camera.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, t);
			yield return new WaitForEndOfFrame();
		}

		yield return 0;
		//_camera.transform.rotation = Quaternion.Lerp(previousRotation, targetRotation, Time.deltaTime * Speed);
	}
}
