using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

	[SerializeField] private Button _addSentinel;
	[SerializeField] private Button _addCamera;
	[SerializeField] private Button _save;
	[SerializeField] private Text _cameraText;
	[SerializeField] private Text _sentinelText;
	[SerializeField] private LevelEditor _levelEditor;
	[SerializeField] private Text _info;
	
	private const string CameraText = "Cameras left: {0}";
	private const string SentinelText = "Sentinels left: {0}";
	
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
	void Update () {
		
	}
}
