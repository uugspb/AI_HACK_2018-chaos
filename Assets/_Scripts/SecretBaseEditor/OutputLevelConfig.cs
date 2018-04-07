using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputLevelConfig
{
     public List<Vector2> CameraCoordinates = new List<Vector2>();
     public List<Vector2> SentinelCoordinates = new List<Vector2>();
     public List<Vector2> TargetCoordinates = new List<Vector2>();
     public List<Vector3> CameraRotations = new List<Vector3>();
     
     public int getCamerasCount()
     {
          return CameraCoordinates.Count;
     }

     public int getSentinelCount()
     {
          return SentinelCoordinates.Count;
     }
}