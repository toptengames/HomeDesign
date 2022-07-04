using System;
using UnityEngine;

public class ScrewdriverTool : MonoBehaviour
{
	public struct PressArguments
	{
		public bool isPressed;
	}

	public struct InitArguments
	{
		public Action<PressArguments> onPress;

		public InputHandler inputHandler;

		public CarScene carScene;
	}

	[SerializeField]
	private Transform buttonTransform;

	[NonSerialized]
	private InitArguments initArguments;

	[SerializeField]
	private GameObject drillModelPrefab;

	[NonSerialized]
	private DrillModel drillModel_;

	public bool isPressed
	{
		get
		{
			if (initArguments.inputHandler == null)
			{
				return false;
			}
			return initArguments.inputHandler.IsAnyDown;
		}
	}

	public DrillModel GetDrillModel(CarScene carScene)
	{
		if (drillModel_ == null)
		{
			drillModel_ = carScene.CreateFromPrefab<DrillModel>(drillModelPrefab);
			drillModel_.transform.localScale = Vector3.one;
		}
		return drillModel_;
	}

	public void Init(InitArguments initArguments)
	{
		this.initArguments = initArguments;
		initArguments.inputHandler.Clear();
		GGUtil.Show(this);
		GGUtil.Hide(buttonTransform);
		if (drillModel_ != null)
		{
			drillModel_.Hide();
		}
	}

	public void Hide()
	{
		GGUtil.Hide(this);
		initArguments.onPress = null;
		if (drillModel_ != null)
		{
			drillModel_.Hide();
		}
	}

	private void Update()
	{
		bool isPressed = this.isPressed;
		if (isPressed)
		{
			PressArguments obj = default(PressArguments);
			obj.isPressed = isPressed;
			initArguments.onPress?.Invoke(obj);
		}
	}
}
