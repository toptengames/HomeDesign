using System;
using UnityEngine;

public class CopyMaterialChangeVariation : MonoBehaviour
{
	[Serializable]
	public class Config
	{
		public int variantIndex;

		public string replace;

		public string replaceWith;

		public string groupName;
	}

	[SerializeField]
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
}
