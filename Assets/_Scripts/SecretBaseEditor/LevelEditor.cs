using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
     [SerializeField] private GameObject _cameraPrefab;
     [SerializeField] private GameObject _sentinelPrefab;
     [SerializeField] private GameObject _sentinelTargetPrefab;
    
     public delegate void LevelConfigChanged(InputLevelConfig config, OutputLevelConfig outputConfig);

     public event LevelConfigChanged OnConfigChanged;

     public delegate void InfoChanged(string info);

     public event InfoChanged OnInfoChanged;

     private InputLevelConfig _currentInputConfig;
     
     private OutputLevelConfig _currentOutputConfig = new OutputLevelConfig();

     private const float MinimalDiff = 1;
     private const float DeletionDiff = 0.5f;
     private const float WheelCoef = 40.0f;

     private Vector3 _lastAddedSentinelCoordinate;
     private string _lastAddedSentinelId;
     private string _lastAddedCameraId;
     private GameObject _lastAddedCamera;
     private List<GameObject> _cameras = new List<GameObject>();
     private List<GameObject> _targets = new List<GameObject>();
     private List<GameObject> _sentinels = new List<GameObject>();

     public enum EditorState
     {
          Normal,
          CreateSentinel,
          CreateCamera,
          CreateTarget,
          RotateCamera,
          Erase
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

     public void HandleMouseClick(Vector3 coordinate)
     {
          var position = new Vector3(coordinate.x, 0, coordinate.z);
          switch (_currentState)
          {
               case EditorState.Normal:
                    break;
               case EditorState.CreateSentinel:
               {
                    if (CheckCoordinates(position))
                    {
                         var instance = Instantiate(_sentinelPrefab);
                         instance.transform.position = position;
                         var sentinel = new OutputLevelConfig.SentinelInfo();
                         sentinel.Id = System.Guid.NewGuid().ToString();
                         sentinel.Coordinate = new Vector2(coordinate.x, coordinate.z);
                         _lastAddedSentinelId = sentinel.Id;
                         
                         _currentOutputConfig.SentinelInfos.Add(sentinel);
                         _currentState = EditorState.CreateTarget;
                         _lastAddedSentinelCoordinate = position;
                         _sentinels.Add(instance);
                         SetInfo("Now add sentinel's target");
                         Refresh();    
                    }
                    else
                    {
                         SetInfo("Too close to other object");
                    }
               }
                   
                    break;
               case EditorState.CreateCamera:
               {
                    if (CheckCoordinates(position))
                    {
                         var instance = Instantiate(_cameraPrefab);
                         instance.transform.position = position;
                         var camera = new OutputLevelConfig.CameraInfo();
                         camera.Id = System.Guid.NewGuid().ToString();
                         _lastAddedCameraId = camera.Id;
                         camera.Coordinate = new Vector2(coordinate.x, coordinate.z);
                         _currentOutputConfig.CameraInfos.Add(camera);
                         _currentState = EditorState.RotateCamera;
                         SetInfo("Configure camera rotation via mouse wheel");
                         _lastAddedCamera = instance;
                         _cameras.Add(instance);
                         Refresh();    
                    }
                    else
                    {
                         SetInfo("Too close to other object");
                    }
               }
                    break;
               case EditorState.CreateTarget:
               {
                    var distanceFromSpawnPoint = (position - _lastAddedSentinelCoordinate).magnitude;
                    if (distanceFromSpawnPoint > MinimalDiff)
                    {
                         var instance = Instantiate(_sentinelTargetPrefab);
                         instance.transform.position = position;
                         _currentState = EditorState.Normal;
                         var lineRenderer = instance.AddComponent<LineRenderer>();
                         var positions = new List<Vector3>();
                         positions.Add(position);
                         positions.Add(_lastAddedSentinelCoordinate);
                         lineRenderer.sortingOrder = 1;
                         lineRenderer.startWidth = 0.1f;
                         lineRenderer.endWidth = 0.1f;
                         lineRenderer.material = new Material (Shader.Find ("Sprites/Default"));
                         lineRenderer.SetPositions(positions.ToArray());
                         lineRenderer.startColor = Color.yellow; 
                         lineRenderer.endColor = Color.blue;
                         SetInfo("");
                         foreach (var sentinelInfo in _currentOutputConfig.SentinelInfos)
                         {
                              if (sentinelInfo.Id == _lastAddedSentinelId)
                              {
                                   sentinelInfo.Target = new Vector2(coordinate.x, coordinate.z);
                              }
                         }
                         _targets.Add(instance);
                         Refresh();    
                    }
                    else
                    {
                         SetInfo("Target is too close to spawn point");
                    }
               }
                    break;
               case EditorState.RotateCamera:
                    foreach (var cameraInfo in _currentOutputConfig.CameraInfos)
                    {
                         if (cameraInfo.Id == _lastAddedCameraId)
                         {
                              cameraInfo.Rotation = _lastAddedCamera.transform.rotation.eulerAngles;
                         }
                    }
                    _currentState = EditorState.Normal;
                    SetInfo("");
                    break;
               case EditorState.Erase:
               {
                    foreach (var cameraInfo in _currentOutputConfig.CameraInfos)
                    {
                         var diff = (new Vector2(coordinate.x, coordinate.z) - cameraInfo.Coordinate).magnitude;
                         if (diff < DeletionDiff)
                         {
                              int index = _currentOutputConfig.CameraInfos.IndexOf(cameraInfo);
                              _currentOutputConfig.CameraInfos.RemoveAt(index);
                              GameObject camera = _cameras[index];
                              if(camera != null)
                                   _cameras.Remove(camera);
                              UnityEngine.Object.Destroy(camera);     
                              break;
                         }
                    }
                    foreach (var sentinelInfo in _currentOutputConfig.SentinelInfos)
                    {
                         var diff = (new Vector2(coordinate.x, coordinate.z) - sentinelInfo.Coordinate).magnitude;
                         if (diff < DeletionDiff)
                         {
                              int index = _currentOutputConfig.SentinelInfos.IndexOf(sentinelInfo);
                              _currentOutputConfig.SentinelInfos.RemoveAt(index);
                              DestroyByIndex(index);     
                              break;
                         }
                    }
        
                    this._currentState = LevelEditor.EditorState.Normal;
                    this.SetInfo("");
               }
                    break;
               default:
                    throw new ArgumentOutOfRangeException();
          }
     }

     private void DestroyByIndex(int index)
     {
          GameObject sentinel = _sentinels[index];
          if (sentinel != null)
          {
               _sentinels.Remove(sentinel);
               UnityEngine.Object.Destroy((UnityEngine.Object) sentinel); 
          }
          GameObject target = _targets[index];
          if (target != null)
          {
               _targets.Remove(target);
               UnityEngine.Object.Destroy((UnityEngine.Object) target); 
          }
     }

     public void SetCameraRotation(float wheelValue)
     {
          if (_currentState == EditorState.RotateCamera)
          {
               var previous = _lastAddedCamera.transform.rotation.eulerAngles;
               _lastAddedCamera.transform.rotation = Quaternion.Euler(previous.x, previous.y + wheelValue * WheelCoef, previous.z);
          }
     }

     private void SetInfo(string message)
     {
          if (OnInfoChanged != null)
          {
               OnInfoChanged(message);
          }
     }

     private bool CheckCoordinates(Vector3 position)
     {
          foreach (var cameraInfo in _currentOutputConfig.CameraInfos)
          {
               var diffVector = new Vector2(position.x, position.y) - cameraInfo.Coordinate;
               if (diffVector.magnitude < MinimalDiff)
               {
                    return false;
               }
          }
          foreach (var sentiInfo in _currentOutputConfig.SentinelInfos)
          {
               var diffVector = new Vector2(position.x, position.y) - sentiInfo.Coordinate;
               if (diffVector.magnitude < MinimalDiff)
               {
                    return false;
               }
          }

          return true;
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

     public void Erase()
     {
          _currentState = EditorState.Erase;
          SetInfo("Erase placed object");
     }

     public EditorState GetCurrentState()
     {
          return _currentState;
     }
}