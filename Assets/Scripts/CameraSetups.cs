using System.Collections.Generic;
using UnityEngine;

public class CameraSetups : MonoBehaviour
{
	[SerializeField]
	public List<CarCamera.Settings> cameraSettings = new List<CarCamera.Settings>();

	[SerializeField]
	private string roomName;

	public void HideAllCameras()
	{
		for (int i = 0; i < cameraSettings.Count; i++)
		{
			GGUtil.Hide(cameraSettings[i].originalTransform);
		}
	}

	public CarCamera.Settings GetCarCamera(string cameraName)
	{
		for (int i = 0; i < cameraSettings.Count; i++)
		{
			CarCamera.Settings settings = cameraSettings[i];
			if (settings.settingsName == cameraName)
			{
				return settings;
			}
		}
		return ScriptableObjectSingleton<CarsDB>.instance.GetCarCamera(roomName, cameraName);
	}

	public void LoadFromTransforms()
	{
		cameraSettings.Clear();
		foreach (Transform item in base.transform)
		{
			CameraSettings component = item.GetComponent<CameraSettings>();
			GGUtil.SetActive(item, active: false);
			if (!(component == null))
			{
				CarCamera.Settings settings = component.LoadSettings();
				settings.originalTransform = component.transform;
				if (settings == null)
				{
					UnityEngine.Debug.Log("CANT LOAD CAMERA " + item.name);
				}
				cameraSettings.Add(settings);
			}
		}
	}
}
