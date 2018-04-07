﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : MonoBehaviour {

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        GameController.instance.LoadPlay();
    }

}
