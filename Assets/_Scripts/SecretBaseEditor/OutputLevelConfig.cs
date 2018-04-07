using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputLevelConfig
{
     public class SentinelInfo
     {
          public string Id;
          public Vector2 Coordinate;
          public Vector2 Target;
     }
     
     public class CameraInfo
     {
          public string Id;
          public Vector2 Coordinate;
          public Vector3 Rotation;
     }
     public List<SentinelInfo> SentinelInfos = new List<SentinelInfo>();
     public List<CameraInfo> CameraInfos = new List<CameraInfo>();
     
     public int getCamerasCount()
     {
          return CameraInfos.Count;
     }

     public int getSentinelCount()
     {
          return SentinelInfos.Count;
     }
}