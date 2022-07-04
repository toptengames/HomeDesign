using GGMatch3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class CarConfirmPurchase : MonoBehaviour
{
	public struct InitArguments
	{
		public AssembleCarScreen screen;

		public string displayName;

		public Vector3 buttonHandlePosition;

		public CarModelPart carPart;

		public bool showBackground;

		public Action onCancel;

		public Action onDrag;

		public Action<InitArguments> onSuccess;

		public bool updateDirection;

		public bool useDistanceToFindIfInside;

		public bool exactPosition;

		public bool useMinDistanceToConfirm;

		public float minDistance;

		public InputHandler inputHandler;

		public Vector3 directionHandlePosition;

		public void CallOnDrag()
		{
			if (onDrag != null)
			{
				onDrag();
			}
		}

		public void CallOnCancel()
		{
			if (onCancel != null)
			{
				onCancel();
			}
		}

		public void CallOnSuccess()
		{
			if (onSuccess != null)
			{
				onSuccess(this);
			}
		}
	}

	private sealed class _003CDoInAnimation_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarConfirmPurchase _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private ConfirmPurchasePanel.Settings _003Csettings_003E5__3;

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
		public _003CDoInAnimation_003Ed__20(int _003C_003E1__state)
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
			CarConfirmPurchase carConfirmPurchase = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ctime_003E5__2 = 0f;
				_003Csettings_003E5__3 = Match3Settings.instance.confirmPurchasePanelSettings;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__2 <= _003Csettings_003E5__3.inAnimationDuration)
			{
				_003Ctime_003E5__2 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Csettings_003E5__3.inAnimationDuration, _003Ctime_003E5__2);
				float t = _003Csettings_003E5__3.inAnimationzoomAnimationCurve.Evaluate(time);
				Vector3 localScale = Vector3.LerpUnclamped(_003Csettings_003E5__3.inAnimationZoomFrom, _003Csettings_003E5__3.inAnimationZoomTo, t);
				carConfirmPurchase.scaleParent.transform.localScale = localScale;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
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
	private float distanceOfSourceFromTarget = 110f;

	[SerializeField]
	private Transform backgroundClickArea;

	[SerializeField]
	private CarDragButton dragButton;

	[SerializeField]
	public CarDragTarget dragTarget;

	[SerializeField]
	private TextMeshProUGUI dragSourceItemNameText;

	[SerializeField]
	private TextMeshProUGUI dragSourcePriceText;

	[SerializeField]
	private RectTransform arrowTransform;

	[SerializeField]
	private RectTransform constrainRect;

	[SerializeField]
	private RectTransform dragSourceRectTransform;

	[SerializeField]
	private RectTransform scaleParent;

	[SerializeField]
	private RectTransform backgroundSelected;

	[SerializeField]
	private TrailRenderer trailRenderer;

	[SerializeField]
	private RectTransform selectorTransform;

	[NonSerialized]
	private AssembleCarScreen screen;

	private IEnumerator inAnimation;

	private IEnumerator selectorAnimation;

	private InitArguments initArguments;

	private bool isDraging;

	public void Show(InitArguments initArguments)
	{
		this.initArguments = initArguments;
		screen = initArguments.screen;
		isDraging = false;
		GGUtil.SetActive(this, active: true);
		GGUtil.SetActive(backgroundClickArea, initArguments.showBackground);
		if (initArguments.inputHandler != null)
		{
			initArguments.inputHandler.onClick -= OnInputHandlerClick;
			initArguments.inputHandler.onClick += OnInputHandlerClick;
		}
		dragButton.Init(this);
		dragTarget.Init(this);
		GGUtil.ChangeText(dragSourceItemNameText, initArguments.displayName);
		dragTarget.transform.localPosition = screen.TransformWorldCarPointToLocalUIPosition(initArguments.buttonHandlePosition);
		Vector3 localPosition2 = dragTarget.transform.localPosition;
		Vector3 zero = Vector3.zero;
		zero.x = 1f;
		zero.y = 1f;
		Vector3 localPosition = zero.normalized * distanceOfSourceFromTarget;
		dragSourceRectTransform.localPosition = localPosition;
		trailRenderer.enabled = false;
		GGUtil.SetActive(backgroundSelected, active: false);
		Vector3 normalized = (dragTarget.transform.position - dragButton.transform.position).normalized;
		arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized);
		GGUtil.SetActive(arrowTransform, active: true);
		UpdatePositionAndDirection();
		inAnimation = DoInAnimation();
		inAnimation.MoveNext();
		selectorAnimation = null;
	}

	private IEnumerator DoInAnimation()
	{
		return new _003CDoInAnimation_003Ed__20(0)
		{
			_003C_003E4__this = this
		};
	}

	public void OnPurchaseConfirmed()
	{
		GGUtil.SetActive(this, active: false);
		initArguments.CallOnSuccess();
	}

	private void OnInputHandlerClick(Vector2 position)
	{
		OnBackgroundClicked();
	}

	public void OnBackgroundClicked()
	{
		GGUtil.SetActive(this, active: false);
		initArguments.CallOnCancel();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void OnDragStart()
	{
		trailRenderer.enabled = true;
		GGUtil.SetActive(arrowTransform, active: false);
		isDraging = true;
		initArguments.CallOnDrag();
	}

	public void OnDragEnd()
	{
		trailRenderer.enabled = false;
		GGUtil.SetActive(arrowTransform, active: true);
		GGUtil.SetActive(backgroundSelected, active: false);
		isDraging = false;
	}

	public bool IsTargetIn()
	{
		Vector3 position = dragButton.transform.position;
		return IsTargetIn(position);
	}

	public float DistancePercent()
	{
		Vector3 position = dragButton.transform.position;
		return DistancePercent(position);
	}

	public float DistancePercent(Vector3 dragButtonWorldPosition)
	{
		CarDragTarget carDragTarget = dragTarget;
		dragButtonWorldPosition.z = carDragTarget.transform.position.z;
		Vector3 position = carDragTarget.transform.position;
		Vector3 vector = dragSourceRectTransform.position - position;
		float magnitude = vector.magnitude;
		float value = Vector3.Dot(vector.normalized, dragButtonWorldPosition - position);
		return Mathf.InverseLerp(0f, magnitude, value);
	}

	public bool IsTargetIn(Vector3 dragButtonWorldPosition)
	{
		if (initArguments.useDistanceToFindIfInside && DistancePercent(dragButtonWorldPosition) <= 0f)
		{
			return true;
		}
		CarDragTarget carDragTarget = dragTarget;
		dragButtonWorldPosition.z = carDragTarget.transform.position.z;
		float magnitude = carDragTarget.transform.InverseTransformPoint(dragButtonWorldPosition).magnitude;
		float num = carDragTarget.GetComponent<RectTransform>().sizeDelta.x * 0.5f + dragButton.GetComponent<RectTransform>().sizeDelta.x * scaleParent.localScale.x * 0.5f;
		return magnitude <= num;
	}

	public void OnButtonClick()
	{
	}

	public void OnDrag()
	{
		isDraging = true;
		bool activeSelf = backgroundSelected.gameObject.activeSelf;
		GGUtil.SetActive(active: IsTargetIn(), trans: backgroundSelected);
		initArguments.CallOnDrag();
		if (initArguments.useMinDistanceToConfirm && DistancePercent() <= initArguments.minDistance && inAnimation == null)
		{
			OnPurchaseConfirmed();
		}
	}

	private void UpdatePositionAndDirection()
	{
		dragTarget.transform.localPosition = screen.TransformWorldCarPointToLocalUIPosition(initArguments.buttonHandlePosition);
		if (initArguments.exactPosition)
		{
			Vector3 localPosition = screen.TransformWorldCarPointToLocalUIPosition(initArguments.directionHandlePosition) - dragTarget.transform.localPosition;
			dragSourceRectTransform.localPosition = localPosition;
			Vector3 normalized = (dragTarget.transform.position - dragButton.transform.position).normalized;
			arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized);
		}
		else if (initArguments.updateDirection)
		{
			Vector3 vector = screen.TransformWorldCarPointToLocalUIPosition(initArguments.directionHandlePosition) - dragTarget.transform.localPosition;
			int num = Mathf.RoundToInt(Mathf.Sign(vector.x));
			int num2 = Mathf.RoundToInt(Mathf.Sign(vector.y));
			if (num == 0)
			{
				num = 1;
			}
			if (num2 == 0)
			{
				num2 = -1;
			}
			vector = new Vector3(num, num2, 0f).normalized;
			Vector3 localPosition2 = vector * distanceOfSourceFromTarget;
			dragSourceRectTransform.localPosition = localPosition2;
			Vector3 normalized2 = (dragTarget.transform.position - dragButton.transform.position).normalized;
			arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized2);
		}
	}

	private void OnDisable()
	{
		if (initArguments.inputHandler != null)
		{
			initArguments.inputHandler.onClick -= OnInputHandlerClick;
		}
	}

	private void Update()
	{
		if (!(dragTarget == null))
		{
			if (!isDraging)
			{
				UpdatePositionAndDirection();
			}
			if (inAnimation != null && !inAnimation.MoveNext())
			{
				inAnimation = null;
			}
			if (selectorAnimation != null && !selectorAnimation.MoveNext())
			{
				selectorAnimation = null;
			}
		}
	}
}
