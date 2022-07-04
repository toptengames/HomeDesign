using System;
using System.Collections.Generic;
using UnityEngine;

public class CarVariationPanel : MonoBehaviour
{
	public struct InitParams
	{
		public bool isPurchased;

		public AssembleCarScreen screen;

		public CarModelInfo.VariantGroup variantGroup;

		public Action<CarVariationPanel> onClosed;

		public Action<CarVariationPanel> onChange;

		public bool showBackground;

		public InputHandler inputHandler;

		public void CallOnClosed(CarVariationPanel panel)
		{
			if (onClosed != null)
			{
				onClosed(panel);
			}
		}

		public void CallOnChange(CarVariationPanel panel)
		{
			onChange?.Invoke(panel);
		}
	}

	[SerializeField]
	private ComponentPool buttonPool = new ComponentPool();

	[SerializeField]
	private float buttonPadding;

	[SerializeField]
	private Transform backgroundClickArea;

	[NonSerialized]
	public InitParams initParams;

	private int _003CvariationIndexAtStart_003Ek__BackingField;

	private int _003CselectedVariation_003Ek__BackingField;

	private bool _003CisVariationChanged_003Ek__BackingField;

	public CarModelInfo.VariantGroup variantGroup => initParams.variantGroup;

	public int variationIndexAtStart
	{
		get
		{
			return _003CvariationIndexAtStart_003Ek__BackingField;
		}
		protected set
		{
			_003CvariationIndexAtStart_003Ek__BackingField = value;
		}
	}

	public int selectedVariation
	{
		get
		{
			return _003CselectedVariation_003Ek__BackingField;
		}
		set
		{
			_003CselectedVariation_003Ek__BackingField = value;
		}
	}

	public bool isVariationChanged
	{
		get
		{
			return _003CisVariationChanged_003Ek__BackingField;
		}
		protected set
		{
			_003CisVariationChanged_003Ek__BackingField = value;
		}
	}

	public void Show(InitParams initParams)
	{
		isVariationChanged = false;
		this.initParams = initParams;
		CarModelInfo.VariantGroup variantGroup = initParams.variantGroup;
		variationIndexAtStart = variantGroup.selectedVariationIndex;
		selectedVariation = variationIndexAtStart;
		this.initParams = initParams;
		GGUtil.SetActive(backgroundClickArea, initParams.showBackground);
		if (initParams.inputHandler != null)
		{
			initParams.inputHandler.onClick -= OnInputHandlerClick;
			initParams.inputHandler.onClick += OnInputHandlerClick;
		}
		buttonPool.Clear();
		List<CarModelInfo.VariantGroup.Variation> variations = variantGroup.variations;
		float x = buttonPool.prefabSizeDelta.x;
		float num = (float)variations.Count * x + (float)(variations.Count - 1) * buttonPadding;
		for (int i = 0; i < variations.Count; i++)
		{
			CarModelInfo.VariantGroup.Variation variation = variations[i];
			CarVariationButton carVariationButton = buttonPool.Next<CarVariationButton>();
			Vector3 zero = Vector3.zero;
			zero.x = (0f - num) * 0.5f + ((float)i + 0.5f) * x + (float)i * buttonPadding;
			GGUtil.SetActive(carVariationButton, active: true);
			carVariationButton.transform.localPosition = zero;
			carVariationButton.Init(this, i, variation);
		}
		buttonPool.HideNotUsed();
		GGUtil.SetActive(this, active: true);
	}

	private void OnInputHandlerClick(Vector2 position)
	{
		OnBackgroundClick();
	}

	public void OnBackgroundClick()
	{
		GGUtil.SetActive(this, active: false);
		initParams.CallOnClosed(this);
	}

	public void ButtonCallback_OnChange(CarVariationButton button)
	{
		selectedVariation = button.variationIndex;
		initParams.variantGroup.selectedVariationIndex = selectedVariation;
		initParams.screen.scene.carModel.RefreshVariations();
		initParams.CallOnChange(this);
		isVariationChanged = true;
	}

	private void OnDisable()
	{
		if (initParams.inputHandler != null)
		{
			initParams.inputHandler.onClick -= OnInputHandlerClick;
		}
	}
}
