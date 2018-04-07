using System.Collections.Generic;
using UnityEngine;

public class InputLevelConfig {

	public int MaxCameras { get; private set; }
	public int MaxSentinels { get; private set; }
	
	public static InputLevelConfig DefaultConfig()
	{
		var result = new InputLevelConfig
		{
			MaxCameras = 5,
			MaxSentinels = 5
		};
		return result;
	}
}
