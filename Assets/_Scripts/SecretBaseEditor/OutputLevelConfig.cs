using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputLevelConfig
{
     public List<Vector2> CameraCoordinates = new List<Vector2>();
     public List<Vector2> SentinelCoordinates = new List<Vector2>();
     
     public int getCamerasCount()
     {
          return CameraCoordinates.Count;
     }

     public int getSentinelCount()
     {
          return SentinelCoordinates.Count;
     }

     public void addCamera(Vector2 coordinates)
     {
          CameraCoordinates.Add(coordinates);
     }

     public void addSentinel(Vector2 coordinates)
     {
          SentinelCoordinates.Add(coordinates);
     }
}