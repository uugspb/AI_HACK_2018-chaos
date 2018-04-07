using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
     [SerializeField] private GameObject _plane;
     [SerializeField] private GameObject _boxPrefab;
     [SerializeField] private GameObject _cameraPrefab;
     [SerializeField] private GameObject _sentinelPrefab;
    
     public delegate void LevelConfigChanged(InputLevelConfig config, OutputLevelConfig outputConfig);

     public event LevelConfigChanged OnConfigChanged;

     public delegate void InfoChanged(string info);

     public event InfoChanged OnInfoChanged;

     private InputLevelConfig _currentInputConfig;
     
     private OutputLevelConfig _currentOutputConfig = new OutputLevelConfig();

     private const float MinimalDiff = 1;

     private const float PlaneSize = 4.0f;
     
     private enum EditorState
     {
          Normal,
          CreateSentinel,
          CreateCamera,
          CreateTarget,
          RotateCamera
     }

     private EditorState _currentState = EditorState.Normal;

     private void Refresh()
     {
          if (OnConfigChanged != null)
          {
               OnConfigChanged(_currentInputConfig, _currentOutputConfig);
          }
     }

     private void Awake()
     {
          _currentInputConfig = InputLevelConfig.DefaultConfig();
     }

     private void Start()
     {
          StartCoroutine(WaitForSubscribers());
          foreach (var coordinate in _currentInputConfig.ObstacleCoordinates)
          {
               var instance = Instantiate(_boxPrefab, _plane.transform);
               instance.transform.localPosition = new Vector3(coordinate.x, 0, coordinate.y);
          }
     }

     private IEnumerator WaitForSubscribers()
     {
          var updated = false;
          while (!updated)
          {
               Refresh();
               if (OnConfigChanged != null)
                    updated = true;
               yield return new WaitForSeconds(0.1f);
          }
     }

     public void AddSentinel()
     {
          _currentState = EditorState.CreateSentinel;
          SetInfo("Now put sentinel on the map");
          //CreateMapObject(_sentinelPrefab, _currentOutputConfig.SentinelCoordinates);
     }
     
     public void AddCamera()
     {
          _currentState = EditorState.CreateCamera;
          SetInfo("Now put camera on the map");
          //CreateMapObject(_cameraPrefab, _currentOutputConfig.CameraCoordinates);
     }

     private void SetInfo(string message)
     {
          if (OnInfoChanged != null)
          {
               OnInfoChanged(message);
          }
     }

     private void CreateMapObject(GameObject prefab, List<Vector2> coordinates2change)
     {
          var isCorrectPosition = false;
          while (!isCorrectPosition)
          {
               var xCoord = UnityEngine.Random.Range(-PlaneSize, PlaneSize);
               var zCoord = UnityEngine.Random.Range(-PlaneSize, PlaneSize);
               var position = new Vector3(xCoord, 0, zCoord);
               if (CheckCoordinateByExisting(position, _currentInputConfig.ObstacleCoordinates) &&
                   CheckCoordinateByExisting(position, _currentOutputConfig.CameraCoordinates) &&
                   CheckCoordinateByExisting(position, _currentOutputConfig.SentinelCoordinates))
               {
                    isCorrectPosition = true;
                    var instance = Instantiate(prefab, _plane.transform);
                    instance.transform.localPosition = position;
                    coordinates2change.Add(new Vector2(position.x, position.z));
               }
          }
     }

     private bool CheckCoordinateByExisting(Vector3 coordinateToCheck, List<Vector2> existingCoordinates)
     {
          foreach (var coordinate in existingCoordinates)
          {
               var diffVector = new Vector3(coordinate.x, 0, coordinate.y) - coordinateToCheck;
               if (diffVector.magnitude < MinimalDiff)
               {
                    return false;
               }
          }
          return true;
     }
}