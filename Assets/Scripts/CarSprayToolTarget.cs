using GGMatch3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarSprayToolTarget : MonoBehaviour
{
	public struct DragState
	{
		public bool isDragging;

		public Vector3 lastScreenPosition;
	}

	[SerializeField]
	private ParticleSystem sprayParticleSystem;

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet notPressedStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet pressedStyle = new VisualStyleSet();

	private Camera uiCamera_;

	private Action<Vector3> onDrag;

	[NonSerialized]
	public DragState dragState;

	private Camera uiCamera
	{
		get
		{
			if (uiCamera_ == null)
			{
				uiCamera_ = NavigationManager.instance.GetCamera();
			}
			return uiCamera_;
		}
	}

	public void Init(Action<Vector3> onDrag)
	{
		this.onDrag = onDrag;
		GGUtil.Hide(sprayParticleSystem);
		GGUtil.Hide(widgetsToHide);
		notPressedStyle.Apply();
		dragState = default(DragState);
	}

	public void OnPress(BaseEventData data)
	{
		OnBeginDrag(data);
	}

	public void OnRelease(BaseEventData data)
	{
		OnEndDrag(data);
	}

	public void OnBeginDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		SetPosition(position);
		GGUtil.Show(sprayParticleSystem);
		ParticleSystemHelper.SetEmmisionActiveRecursive(sprayParticleSystem, active: true);
		GGUtil.Hide(widgetsToHide);
		pressedStyle.Apply();
		dragState.isDragging = true;
	}

	public void OnEndDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		SetPosition(position);
		ParticleSystemHelper.SetEmmisionActiveRecursive(sprayParticleSystem, active: false);
		GGUtil.Hide(widgetsToHide);
		notPressedStyle.Apply();
		dragState.isDragging = false;
	}

	public void OnDrag(BaseEventData data)
	{
		PointerEventData position = data as PointerEventData;
		SetPosition(position);
	}

	private void SetPosition(PointerEventData pointerData)
	{
		if (pointerData != null)
		{
			Transform parent = base.transform.parent;
			base.transform.localPosition = parent.InverseTransformPoint(uiCamera.ScreenToWorldPoint(pointerData.position));
			dragState.lastScreenPosition = pointerData.position;
			if (onDrag != null)
			{
				onDrag(pointerData.position);
			}
		}
	}
}
