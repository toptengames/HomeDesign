using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	[Serializable]
	public class BlendSettings
	{
		public float blendDuration;

		public AnimationCurve blendCurve;
	}

	[Serializable]
	public class Settings
	{
		public string settingsName;

		public float cameraDistance;

		public GGMath.FloatRange verticalAngleRange;

		public float fov = 33f;

		public float startVerticalAngle;

		public float startHorizontalAngle;

		public float horizontalAngleSpeed;

		public float verticalAngleSpeed;

		public bool enableRotationCenter;

		public Vector3 rotationCenter;

		public bool changeAnglesAtStart;

		public Transform originalTransform;

		public Vector3 RotationCenter(Transform cameraParentTransform)
		{
			Vector3 position = cameraParentTransform.position;
			if (enableRotationCenter)
			{
				position = rotationCenter;
			}
			return position;
		}
	}

	[Serializable]
	public class InertiaSettings
	{
		[SerializeField]
		public float maxVelocity = 8f;

		[SerializeField]
		public float dragSpeed = 1f;

		[SerializeField]
		public float minInertia = 1f;

		[SerializeField]
		public float affinityToNew = 0.5f;

		public Vector2 GetInertia(Vector2 displace)
		{
			Vector2 result = default(Vector2);
			result.x = Mathf.Sign(displace.x) * Mathf.Min(Mathf.Abs(displace.x), maxVelocity);
			result.y = Mathf.Sign(displace.y) * Mathf.Min(Mathf.Abs(displace.y), maxVelocity);
			return result;
		}
	}

	public struct State
	{
		public float horizontalAngle;

		public float verticalAngle;
	}

	private struct InputState
	{
		private Vector3 previousPosition;

		public bool isDown;

		public int touchId;

		public Vector2 lastPosition;

		public Vector2 inertia;
	}

	public struct AnimateIntoSettingsArguments
	{
		public Settings newSettings;

		public bool updateAngle;
	}

	private sealed class _003CDoAnimateIntoSettings_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AnimateIntoSettingsArguments arguments;

		public CarCamera _003C_003E4__this;

		private Settings _003CnewSettings_003E5__2;

		private float _003Cduration_003E5__3;

		private AnimationCurve _003Ccurve_003E5__4;

		private Vector3 _003CrotationCenterStart_003E5__5;

		private Vector3 _003CrotationCenterEnd_003E5__6;

		private float _003ChorizontalAngleStart_003E5__7;

		private float _003CverticalAngleStart_003E5__8;

		private float _003ChorizontalAngleEnd_003E5__9;

		private float _003CverticalAngleEnd_003E5__10;

		private float _003CstartDistance_003E5__11;

		private float _003CendDistance_003E5__12;

		private Transform _003CcameraTransform_003E5__13;

		private float _003CstartFOV_003E5__14;

		private float _003CendFOV_003E5__15;

		private float _003Ctime_003E5__16;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CDoAnimateIntoSettings_003Ed__30(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CarCamera carCamera = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003CnewSettings_003E5__2 = arguments.newSettings;
				Settings usedSettings = carCamera.usedSettings;
				_003Cduration_003E5__3 = carCamera.blendSettings.blendDuration;
				_003Ccurve_003E5__4 = carCamera.blendSettings.blendCurve;
				_003CrotationCenterStart_003E5__5 = usedSettings.RotationCenter(carCamera.transform);
				_003CrotationCenterEnd_003E5__6 = _003CnewSettings_003E5__2.RotationCenter(carCamera.transform);
				_003ChorizontalAngleStart_003E5__7 = carCamera.state.horizontalAngle;
				_003CverticalAngleStart_003E5__8 = carCamera.state.verticalAngle;
				_003ChorizontalAngleEnd_003E5__9 = _003CnewSettings_003E5__2.startHorizontalAngle;
				_003CverticalAngleEnd_003E5__10 = _003CnewSettings_003E5__2.startVerticalAngle;
				_003CstartDistance_003E5__11 = usedSettings.cameraDistance;
				_003CendDistance_003E5__12 = _003CnewSettings_003E5__2.cameraDistance;
				_003CcameraTransform_003E5__13 = carCamera.camera.transform;
				_003CstartFOV_003E5__14 = carCamera.camera.fieldOfView;
				_003CendFOV_003E5__15 = _003CnewSettings_003E5__2.fov;
				_003Ctime_003E5__16 = 0f;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__16 <= _003Cduration_003E5__3)
			{
				_003Ctime_003E5__16 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Cduration_003E5__3, _003Ctime_003E5__16);
				float t = _003Ccurve_003E5__4.Evaluate(time);
				Vector3 vector = Vector3.Lerp(_003CrotationCenterStart_003E5__5, _003CrotationCenterEnd_003E5__6, t);
				float horizontalAngle = Mathf.LerpAngle(_003ChorizontalAngleStart_003E5__7, _003ChorizontalAngleEnd_003E5__9, t);
				float verticalAngle = Mathf.LerpAngle(_003CverticalAngleStart_003E5__8, _003CverticalAngleEnd_003E5__10, t);
				float fieldOfView = Mathf.Lerp(_003CstartFOV_003E5__14, _003CendFOV_003E5__15, t);
				float d = Mathf.Lerp(_003CstartDistance_003E5__11, _003CendDistance_003E5__12, t);
				if (arguments.updateAngle)
				{
					carCamera.state.horizontalAngle = horizontalAngle;
					carCamera.state.verticalAngle = verticalAngle;
				}
				Vector3 a = vector;
				Quaternion rotation = Quaternion.AngleAxis(carCamera.state.horizontalAngle, Vector3.up) * Quaternion.AngleAxis(carCamera.state.verticalAngle, Vector3.right);
				_003CcameraTransform_003E5__13.position = vector + rotation * (Vector3.forward * d);
				_003CcameraTransform_003E5__13.rotation = Quaternion.LookRotation(a - _003CcameraTransform_003E5__13.position);
				carCamera.camera.fieldOfView = fieldOfView;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			carCamera.usedSettings = _003CnewSettings_003E5__2;
			carCamera.UpdateCameraPosition();
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[SerializeField]
	private Camera camera;

	[SerializeField]
	private bool useSettingsStartAngles;

	[SerializeField]
	public CameraSetups cameraSetups;

	[SerializeField]
	private Settings settings = new Settings();

	[NonSerialized]
	private Settings usedSettings;

	private IEnumerator animation;

	[SerializeField]
	public InertiaSettings inertiaSettings;

	[NonSerialized]
	private InputState inputState;

	[NonSerialized]
	private InputHandler inputHandler;

	private State state;

	public Vector3 cameraForward => camera.transform.forward;

	public Vector3 cameraPosition => camera.transform.position;

	public BlendSettings blendSettings => ScriptableObjectSingleton<CarsDB>.instance.blendSettings;

	public Vector3 ScreenToViewPortPoint(Vector3 screenPoint)
	{
		return camera.ScreenToViewportPoint(screenPoint);
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		return camera.WorldToScreenPoint(position);
	}

	public Ray ScreenPointToRay(Vector3 position)
	{
		return camera.ScreenPointToRay(position);
	}

	public Settings GetCarCamera(string name)
	{
		if (cameraSetups == null)
		{
			return null;
		}
		return cameraSetups.GetCarCamera(name);
	}

	public void Init(InputHandler inputHandler)
	{
		this.inputHandler = inputHandler;
		usedSettings = settings;
		state.horizontalAngle = settings.startHorizontalAngle;
		state.verticalAngle = settings.startVerticalAngle;
		UpdateCameraPosition();
	}

	public void SetStandardSettings()
	{
		AnimateIntoSettingsArguments arguments = default(AnimateIntoSettingsArguments);
		arguments.newSettings = settings;
		arguments.updateAngle = false;
		AnimateIntoSettings(arguments);
	}

	public void AnimateIntoSettings(Settings newSettings)
	{
		AnimateIntoSettingsArguments arguments = default(AnimateIntoSettingsArguments);
		arguments.newSettings = newSettings;
		arguments.updateAngle = newSettings.changeAnglesAtStart;
		AnimateIntoSettings(arguments);
	}

	public void AnimateIntoSettings(AnimateIntoSettingsArguments arguments)
	{
		inputState.inertia = Vector3.zero;
		animation = DoAnimateIntoSettings(arguments);
		animation.MoveNext();
	}

	private IEnumerator DoAnimateIntoSettings(AnimateIntoSettingsArguments arguments)
	{
		return new _003CDoAnimateIntoSettings_003Ed__30(0)
		{
			_003C_003E4__this = this,
			arguments = arguments
		};
	}

	public void Move(Vector2 distance)
	{
		Settings settings = usedSettings;
		state.horizontalAngle += distance.x * settings.horizontalAngleSpeed;
		state.verticalAngle = settings.verticalAngleRange.Clamp((0f - distance.y) * settings.verticalAngleSpeed + state.verticalAngle);
		UpdateCameraPosition();
	}

	private void UpdateCameraPosition()
	{
		if (!(camera == null) && usedSettings != null)
		{
			Settings settings = usedSettings;
			Transform transform = camera.transform;
			Vector3 vector = base.transform.position;
			if (settings.enableRotationCenter)
			{
				vector = settings.rotationCenter;
			}
			Vector3 a = vector;
			Quaternion rotation = Quaternion.AngleAxis(state.horizontalAngle, Vector3.up) * Quaternion.AngleAxis(state.verticalAngle, Vector3.right);
			transform.position = vector + rotation * (Vector3.forward * settings.cameraDistance);
			transform.rotation = Quaternion.LookRotation(a - transform.position);
			if (camera.fieldOfView != settings.fov)
			{
				camera.fieldOfView = settings.fov;
			}
		}
	}

	private void UpdateInertia()
	{
		if (!(Mathf.Max(inputState.inertia.x, inputState.inertia.y) < inertiaSettings.minInertia))
		{
			inputState.inertia = Vector3.Lerp(inputState.inertia, Vector3.zero, Time.unscaledDeltaTime * inertiaSettings.dragSpeed);
			Move(inputState.inertia);
		}
	}

	private void UpdateInputHandler()
	{
		if (!(inputHandler == null))
		{
			InputHandler.PointerData pointerData = inputHandler.FirstDownPointer();
			if (!inputState.isDown && pointerData != null)
			{
				inputState = default(InputState);
				inputState.isDown = pointerData.isDown;
				inputState.touchId = pointerData.pointerId;
				inputState.lastPosition = pointerData.position;
			}
			else if (!inputState.isDown)
			{
				UpdateInertia();
			}
			else if (!inputHandler.IsDown(inputState.touchId))
			{
				inputState.isDown = false;
			}
			else
			{
				Vector2 lastPosition = inputState.lastPosition;
				Vector2 vector = inputHandler.Position(inputState.touchId);
				Vector2 vector2 = vector - lastPosition;
				inputState.inertia = Vector3.Lerp(inputState.inertia, inertiaSettings.GetInertia(vector2), inertiaSettings.affinityToNew);
				inputState.lastPosition = vector;
				Move(vector2);
			}
		}
	}

	private void Update()
	{
		if (animation != null)
		{
			if (!animation.MoveNext())
			{
				animation = null;
			}
			return;
		}
		UpdateInputHandler();
		if (Application.isEditor)
		{
			if (useSettingsStartAngles && usedSettings != null)
			{
				state.verticalAngle = usedSettings.startVerticalAngle;
				state.horizontalAngle = usedSettings.startHorizontalAngle;
			}
			UpdateCameraPosition();
		}
	}
}
