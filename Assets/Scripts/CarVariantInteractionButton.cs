using System;
using UnityEngine;

public class CarVariantInteractionButton : MonoBehaviour
{
	public struct InitParams
	{
		public AssembleCarScreen screen;

		public Vector3 positionToAttachTo;

		public Transform forwardTransform;

		public Vector3 forwardDirection;

		public CarModelInfo.VariantGroup variantGroup;

		public CarModelSubpart subpart;

		public Action<CarVariantInteractionButton> onClick;

		public void CallOnClick(CarVariantInteractionButton button)
		{
			onClick?.Invoke(button);
		}
	}

	[SerializeField]
	private Transform buyButtonContanier;

	[NonSerialized]
	private InitParams initParams;

	private bool shouldBeActive;

	private AssembleCarScreen screen => initParams.screen;

	private Vector3 positionToAttachTo => initParams.positionToAttachTo;

	public CarModelSubpart subpart => initParams.subpart;

	public CarModelInfo.VariantGroup variantGroup => initParams.variantGroup;

	public void Init(InitParams initParams)
	{
		shouldBeActive = true;
		this.initParams = initParams;
		GGUtil.SetActive(buyButtonContanier, active: true);
		SetPositionOfBuyButton();
	}

	private void SetPositionOfBuyButton()
	{
		Vector3 localPosition = screen.TransformWorldCarPointToLocalUIPosition(positionToAttachTo);
		localPosition.z = 0f;
		buyButtonContanier.localPosition = localPosition;
		bool flag = screen.IsFacingCamera(initParams.forwardDirection);
		GGUtil.SetActive(buyButtonContanier, flag && shouldBeActive);
	}

	private void Update()
	{
		bool flag = screen != null;
		SetPositionOfBuyButton();
	}

	public void HideButton()
	{
		shouldBeActive = false;
		GGUtil.SetActive(buyButtonContanier, active: false);
	}

	public void ButtonCallback_OnClick()
	{
		initParams.CallOnClick(this);
	}
}
