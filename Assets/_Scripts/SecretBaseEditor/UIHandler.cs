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
	
	private const string CameraText = "Cameras left: {0}";
	private const string SentinelText = "Sentinels left: {0}";
	
	// Use this for initialization
	void Start ()
	{
		_levelEditor.OnConfigChanged += UpdateUI;
	}

	private void OnDestroy()
	{
		_levelEditor.OnConfigChanged -= UpdateUI;
	}

	private void UpdateUI(InputLevelConfig inputConfig, OutputLevelConfig outputConfig)
	{
		_cameraText.text = string.Format(CameraText, inputConfig.MaxCameras - outputConfig.getCamerasCount());
		_sentinelText.text = string.Format(SentinelText, inputConfig.MaxSentinels - outputConfig.getSentinelCount());
	}

	// Update is called once per frame
	void Update () {
		
	}
}
