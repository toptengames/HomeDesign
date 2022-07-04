using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeVariation : MonoBehaviour
{
	[Serializable]
	public class Config
	{
		public int variantIndex;

		public Material materialToApply;

		public string groupName;
	}

	public Config config = new Config();

	public string groupName
	{
		get
		{
			if (GGUtil.HasText(config.groupName))
			{
				return config.groupName;
			}
			VariantGroupSetup componentInParent = GetComponentInParent<VariantGroupSetup>();
			if (componentInParent != null)
			{
				return componentInParent.settings.name;
			}
			return config.groupName;
		}
	}

	public void Apply()
	{
		CarModelSubpart component = GetComponent<CarModelSubpart>();
		if (component == null)
		{
			UnityEngine.Debug.Log("NEED TO BE ON THE SAME OBJECT AS SUBPART");
			return;
		}
		List<VariantModification> list = new List<VariantModification>();
		for (int i = 0; i < component.variantModifications.Count; i++)
		{
			VariantModification variantModification = component.variantModifications[i];
			if (variantModification.variantIndex == config.variantIndex)
			{
				list.Add(variantModification);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			VariantModification item = list[j];
			component.variantModifications.Remove(item);
		}
		string groupName = this.groupName;
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			for (int l = 0; l < sharedMaterials.Length; l++)
			{
				Material materialToApply = sharedMaterials[l];
				VariantModification variantModification2 = new VariantModification();
				variantModification2.groupName = groupName;
				variantModification2.materialIndex = l;
				if (config.materialToApply == null)
				{
					variantModification2.materialToApply = materialToApply;
				}
				else
				{
					variantModification2.materialToApply = config.materialToApply;
				}
				variantModification2.rendererToApplyTo = meshRenderer;
				variantModification2.variantIndex = config.variantIndex;
				component.variantModifications.Add(variantModification2);
			}
		}
	}
}
