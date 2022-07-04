using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarModelInfo
{
	[Serializable]
	public class VariantGroup
	{
		[Serializable]
		public class Variation
		{
			public Color uiSpriteColor;
		}

		[SerializeField]
		public string name;

		[SerializeField]
		public List<Variation> variations = new List<Variation>();

		[SerializeField]
		public string cameraName;

		private RoomsBackend.RoomAccessor backendAccessor_;

		private RoomsBackend.VariantGroupAccessor variantGroupModel_;

		private RoomsBackend.RoomAccessor backendAccessor
		{
			get
			{
				if (backendAccessor_.needsToBeRenewed)
				{
					backendAccessor_ = backendAccessor_.CreateRenewedAccessor();
				}
				return backendAccessor_;
			}
		}

		private RoomsBackend.VariantGroupAccessor variantGroupModel
		{
			get
			{
				if (variantGroupModel_ == null)
				{
					variantGroupModel_ = backendAccessor.GetVariantGroup(name);
				}
				if (variantGroupModel_.needsToBeRenewed)
				{
					variantGroupModel_ = backendAccessor.GetVariantGroup(name);
				}
				return variantGroupModel_;
			}
		}

		public int selectedVariationIndex
		{
			get
			{
				return variantGroupModel.variantGroup.selectedVariationIndex;
			}
			set
			{
				variantGroupModel.variantGroup.selectedVariationIndex = value;
				variantGroupModel.Save();
			}
		}

		public void Init(RoomsBackend.RoomAccessor backendAccessor)
		{
			backendAccessor_ = backendAccessor;
		}
	}

	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public string name;

		internal bool _003CRemoveVariantGroup_003Eb__0(VariantGroup group)
		{
			return group.name == name;
		}
	}

	[SerializeField]
	private List<VariantGroup> variantGroups = new List<VariantGroup>();

	public VariantGroup GetVariantGroup(string name)
	{
		for (int i = 0; i < variantGroups.Count; i++)
		{
			VariantGroup variantGroup = variantGroups[i];
			if (variantGroup.name == name)
			{
				return variantGroup;
			}
		}
		return null;
	}

	public void RemoveVariantGroup(string name)
	{
		_003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = new _003C_003Ec__DisplayClass3_0();
		_003C_003Ec__DisplayClass3_.name = name;
		variantGroups.RemoveAll(_003C_003Ec__DisplayClass3_._003CRemoveVariantGroup_003Eb__0);
	}

	public void AddGroup(VariantGroup group)
	{
		variantGroups.Add(group);
	}

	public void Init(RoomsBackend.RoomAccessor roomAccessor)
	{
		for (int i = 0; i < variantGroups.Count; i++)
		{
			variantGroups[i].Init(roomAccessor);
		}
	}
}
