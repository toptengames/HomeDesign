using System;
using System.Collections.Generic;
using UnityEngine;

public class CarVisualItemButton : MonoBehaviour
{
	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private Transform buyButtonContanier;

	[NonSerialized]
	public CarModelPart part;

	[NonSerialized]
	private AssembleCarScreen screen;

	public void Init(CarModelPart part, AssembleCarScreen screen)
	{
		this.part = part;
		this.screen = screen;
		CarPartInfo partInfo = part.partInfo;
		GGUtil.SetActive(widgetsToHide, active: false);
		bool active = partInfo.isUnlocked && !partInfo.isOwned;
		GGUtil.SetActive(buyButtonContanier, active);
		part.SetActiveIfOwned();
		SetPositionOfBuyButton();
	}

	private void SetPositionOfBuyButton()
	{
		Vector3 localPosition = screen.TransformWorldCarPointToLocalUIPosition(part.buttonHandlePosition);
		localPosition.z = 0f;
		buyButtonContanier.localPosition = localPosition;
	}

	private void Update()
	{
		if (!(part == null))
		{
			SetPositionOfBuyButton();
		}
	}

	public void HideButton()
	{
		GGUtil.SetActive(buyButtonContanier, active: false);
	}

	public void ButtonCallback_OnBuyButton()
	{
		screen.VisualItemCallback_OnBuyItemPressed(this);
	}
}
