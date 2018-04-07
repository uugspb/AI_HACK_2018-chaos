using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputLevelConfig
{
     public class SentinelInfo
     {
          public string Id;
          public Vector3 Coordinate;
          public Vector3 Target;
     }
     
     public class CameraInfo
     {
          public Vector3 Coordinate;
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