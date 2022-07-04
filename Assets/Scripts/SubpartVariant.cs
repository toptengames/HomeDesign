using System;
using UnityEngine;

public class SubpartVariant : MonoBehaviour
{
	[SerializeField]
	public CarSubPartVariantInfo info = new CarSubPartVariantInfo();

	[SerializeField]
	private CarModelSubpart subpart;

	[NonSerialized]
	private CarModelInfo.VariantGroup variantGroup_;

	public CarModelInfo.VariantGroup variantGroup
	{
		get
		{
			if (variantGroup_ == null)
			{
				variantGroup_ = subpart.part.model.modelInfo.GetVariantGroup(info.groupName);
			}
			return variantGroup_;
		}
	}

	public void ShowIfInActiveVariant()
	{
		CarModelInfo.VariantGroup variantGroup = this.variantGroup;
		int num = 0;
		if (variantGroup != null)
		{
			num = variantGroup.selectedVariationIndex;
		}
		GGUtil.SetActive(this, num == info.index);
	}

	public void Init(CarModelSubpart subpart)
	{
		this.subpart = subpart;
	}
}
