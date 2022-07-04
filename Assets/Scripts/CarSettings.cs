using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarSettings
{
	[SerializeField]
	public string carName;

	[SerializeField]
	private List<CarCamera.Settings> carCameraSettings = new List<CarCamera.Settings>();

	public CarCamera.Settings GetSettings(string name)
	{
		for (int i = 0; i < carCameraSettings.Count; i++)
		{
			CarCamera.Settings settings = carCameraSettings[i];
			if (settings.settingsName == name)
			{
				return settings;
			}
		}
		return null;
	}
}
