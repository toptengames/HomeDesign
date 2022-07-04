using UnityEngine;

[ExecuteInEditMode]
public class CameraSettings : MonoBehaviour
{
	[SerializeField]
	private Camera camera;

	[SerializeField]
	private Transform lookAtTransform;

	[SerializeField]
	private bool changeAnglesAtStart = true;

	public CarCamera.Settings LoadSettings()
	{
		if (camera == null || lookAtTransform == null)
		{
			return null;
		}
		CarCamera.Settings obj = new CarCamera.Settings
		{
			settingsName = base.transform.name,
			enableRotationCenter = true,
			rotationCenter = lookAtTransform.position,
			cameraDistance = 0f - Vector3.Distance(lookAtTransform.position, camera.transform.position),
			changeAnglesAtStart = changeAnglesAtStart,
			horizontalAngleSpeed = 0.2f,
			verticalAngleSpeed = 0.2f,
			verticalAngleRange = 
			{
				min = 0f,
				max = 45f
			}
		};
		float num = Vector3.Distance(lookAtTransform.position, camera.transform.position);
		float f = camera.transform.position.y - lookAtTransform.position.y;
		float startVerticalAngle = 0f;
		if (Mathf.Abs(f) > Mathf.Epsilon && num > 0f)
		{
			startVerticalAngle = Mathf.Sign(f) * Mathf.Asin(Mathf.Abs(f) / num) * 57.29578f;
		}
		obj.startVerticalAngle = startVerticalAngle;
		Vector3 to = lookAtTransform.position - camera.transform.position;
		to.y = 0f;
		obj.startHorizontalAngle = Vector3.SignedAngle(Vector3.forward, to, Vector3.up);
		obj.fov = camera.fieldOfView;
		return obj;
	}

	private void Update()
	{
		if (!(camera == null) && !(lookAtTransform == null))
		{
			camera.transform.rotation = Quaternion.LookRotation(lookAtTransform.transform.position - camera.transform.position);
		}
	}
}
