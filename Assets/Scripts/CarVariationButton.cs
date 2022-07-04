using System;
using UnityEngine;
using UnityEngine.UI;

public class CarVariationButton : MonoBehaviour
{
	[SerializeField]
	public Image image;

	[NonSerialized]
	public int variationIndex;

	[NonSerialized]
	private CarVariationPanel variationPanel;

	public void Init(CarVariationPanel variationPanel, int variationIndex, CarModelInfo.VariantGroup.Variation variation)
	{
		this.variationIndex = variationIndex;
		this.variationPanel = variationPanel;
		GGUtil.SetColor(image, variation.uiSpriteColor);
	}

	public void OnClick()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.Flip);
		if (variationPanel != null)
		{
			variationPanel.ButtonCallback_OnChange(this);
		}
	}
}
