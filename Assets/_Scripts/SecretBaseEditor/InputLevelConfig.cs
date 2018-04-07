using System.Collections.Generic;
using UnityEngine;

public class InputLevelConfig {

	public int MaxCameras { get; private set; }
	public int MaxSentinels { get; private set; }
	public List<Vector2> ObstacleCoordinates { get; private set; }

	public static InputLevelConfig DefaultConfig()
	{
		var coordinates = new List<Vector2>();
		coordinates.Add(new Vector2(2,2));
		coordinates.Add(new Vector2(3,3));
		var result = new InputLevelConfig
		{
			MaxCameras = 5,
			MaxSentinels = 5,
			ObstacleCoordinates = coordinates
		};
		return result;
	}
}
