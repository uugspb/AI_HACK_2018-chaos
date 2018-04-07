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
				var isEditorAffected = false;
				if (_editor.GetCurrentState() == LevelEditor.EditorState.Erase)
				{
					if (hit.collider.CompareTag("Camera") ||
					    hit.collider.CompareTag("Sentinel") ||
					    hit.collider.CompareTag("Target") ||
					    hit.collider.CompareTag("Plane"))
					{
						isEditorAffected = true;
					}
				} else if (_editor.GetCurrentState() == LevelEditor.EditorState.RotateCamera)
				{
					if (hit.collider.CompareTag("Camera") ||
					    hit.collider.CompareTag("Plane"))
					{
						isEditorAffected = true;
					}
				}
				else
				{
					if (hit.collider.CompareTag("Plane"))
					{
						isEditorAffected = true;
					}
				}

				if (isEditorAffected)
				{
					_editor.HandleMouseClick(hit.point);
				}
			}		
		}

		var mouseWheelValue = Input.GetAxis("Mouse ScrollWheel");
		_editor.SetCameraRotation(mouseWheelValue);
	}
}
