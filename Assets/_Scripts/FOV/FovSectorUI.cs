using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FovSectorUI : MonoBehaviour {

    [SerializeField] private Image _sectorImage;

    private RectTransform _imageRect;

    public float SectorDegrees;


	// Use this for initialization
	void Start ()
    {
        _imageRect = _sectorImage.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        var angles = _imageRect.localEulerAngles;
        angles.z = SectorDegrees;
        _imageRect.localEulerAngles = angles;

        _sectorImage.fillAmount = SectorDegrees * 2 / 360;
	}
}
