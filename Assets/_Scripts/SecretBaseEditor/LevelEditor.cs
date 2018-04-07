using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
     [SerializeField] private GameObject _cameraPrefab;
     [SerializeField] private GameObject _sentinelPrefab;
     [SerializeField] private GameObject _sentinelTargetPrefab;
    
     public delegate void LevelConfigChanged(InputLevelConfig config, int sentinelsCount, int camerasCount);

     public event LevelConfigChanged OnConfigChanged;

     public delegate void InfoChanged(string info);

     public event InfoChanged OnInfoChanged;

     private InputLevelConfig _currentInputConfig;

     private const float MinimalDiff = 1;
     private const float DeletionDiff = 0.5f;
     private const float WheelCoef = 40.0f;

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
               OnConfigChanged(_currentInputConfig, _sentinels.Count, _cameras.Count);
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
     }
     
     public void AddCamera()
     {
          _currentState = EditorState.CreateCamera;
          SetInfo("Now put camera on the map");
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
                         _currentState = EditorState.CreateTarget;
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
                         _currentState = EditorState.RotateCamera;
                         SetInfo("Configure camera rotation via mouse wheel");
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
                    var distanceFromSpawnPoint = (position - _sentinels.Last().transform.position).magnitude;
                    if (distanceFromSpawnPoint > MinimalDiff)
                    {
                         var instance = Instantiate(_sentinelTargetPrefab);
                         instance.transform.position = position;
                         _currentState = EditorState.Normal;
                         var lineRenderer = instance.AddComponent<LineRenderer>();
                         var positions = new List<Vector3>();
                         positions.Add(position);
                         positions.Add(_sentinels.Last().transform.position);
                         lineRenderer.sortingOrder = 1;
                         lineRenderer.startWidth = 0.1f;
                         lineRenderer.endWidth = 0.1f;
                         lineRenderer.material = new Material (Shader.Find ("Sprites/Default"));
                         lineRenderer.SetPositions(positions.ToArray());
                         lineRenderer.startColor = Color.yellow; 
                         lineRenderer.endColor = Color.blue;
                         SetInfo("");
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
                    _currentState = EditorState.Normal;
                    SetInfo("");
                    break;
               case EditorState.Erase:
               {
                    foreach (var camera in _cameras)
                    {
                         var diff = (position - camera.transform.position).magnitude;
                         if (diff < DeletionDiff)
                         {
                              _cameras.Remove(camera);
                              Destroy(camera);
                              break;    
                         }
                    }

                    foreach (var sentinel in _sentinels)
                    {
                         var diff = (position - sentinel.transform.position).magnitude;
                         if (diff < DeletionDiff)
                         {
                              int index = _sentinels.IndexOf(sentinel);
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
               var camera = _cameras.Last().transform;
               var previous = camera.rotation.eulerAngles;
               camera.rotation = Quaternion.Euler(previous.x, previous.y + wheelValue * WheelCoef, previous.z);
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
          foreach (var camera in _cameras)
          {
               var diffVector = position - camera.transform.position;
               if (diffVector.magnitude < MinimalDiff)
               {
                    return false;
               }
          }
          foreach (var sentinel in _sentinels)
          {
               var diffVector = position - sentinel.transform.position;
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