using System;
using System.Collections;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
     [SerializeField] private GameObject _plane;
     [SerializeField] private GameObject _boxPrefab;
    
     public delegate void LevelConfigChanged(InputLevelConfig config, OutputLevelConfig outputConfig);

     public event LevelConfigChanged OnConfigChanged;

     private InputLevelConfig _currentInputConfig;
     
     private OutputLevelConfig _currentOutputConfig = new OutputLevelConfig();

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
               var instance = GameObject.Instantiate(_boxPrefab, _plane.transform);
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
}