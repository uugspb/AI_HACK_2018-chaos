using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneClickDetector : MonoBehaviour
{

	[SerializeField] private Camera _camera;
	[SerializeField] private LevelEditor _editor;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				if(hit.collider.CompareTag("Plane"))
					_editor.HandleMouseClick(hit.point);
			}		
		}

		var mouseWheelValue = Input.GetAxis("Mouse ScrollWheel");
		_editor.SetCameraRotation(mouseWheelValue);
	}
}
