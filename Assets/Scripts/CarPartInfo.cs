using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarPartInfo
{
	public enum AnimType
	{
		ScaleBounce,
		MoveOffset
	}

	[SerializeField]
	public List<CarModelPart> hideWhenAnyActive = new List<CarModelPart>();

	[SerializeField]
	public bool isDefault;

	[SerializeField]
	public string name;

	[SerializeField]
	private string displayName_;

	[SerializeField]
	public string thingToSay;

	[SerializeField]
	public string cameraName;

	[SerializeField]
	public List<string> toSayBefore = new List<string>();

	[SerializeField]
	public int groupIndex;

	[SerializeField]
	public int explodeGroupIndex;

	[SerializeField]
	public AnimType animType;

	[SerializeField]
	public Vector3 moveOffset;

	[SerializeField]
	public bool suspendExploding;

	[SerializeField]
	public bool confirmEachSubpart;

	[SerializeField]
	public float delaySubpartAnimation;

	[SerializeField]
	public string variantGroupToShowAfterPurchase;

	[SerializeField]
	public List<string> animateChangeWithVariations = new List<string>();

	[NonSerialized]
	private CarModel carModel;

	private RoomsBackend.RoomAccessor backendAccessor_;

	private RoomsBackend.VisualObjectAccessor visualObjectModel_;

	public bool hasSomethingToSay => !string.IsNullOrWhiteSpace(thingToSay);

	public string displayName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(displayName_))
			{
				return displayName_;
			}
			return name;
		}
	}

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

	private RoomsBackend.VisualObjectAccessor visualObjectModel
	{
		get
		{
			if (visualObjectModel_ == null)
			{
				visualObjectModel_ = backendAccessor.GetVisualObject(name);
			}
			if (visualObjectModel_.needsToBeRenewed)
			{
				visualObjectModel_ = backendAccessor.GetVisualObject(name);
			}
			return visualObjectModel_;
		}
	}

	public int selectedVariation
	{
		set
		{
			visualObjectModel.visualObject.selectedVariationIndex = value;
			visualObjectModel.Save();
		}
	}

	public bool isOwned
	{
		get
		{
			if (!isDefault)
			{
				return visualObjectModel.visualObject.isOwned;
			}
			return true;
		}
		set
		{
			visualObjectModel.visualObject.isOwned = value;
			visualObjectModel.Save();
		}
	}

	public bool isUnlocked
	{
		get
		{
			if (groupIndex <= 0)
			{
				return true;
			}
			if (carModel.IsAllElementsPickedUpInGroup(groupIndex - 1))
			{
				return true;
			}
			return false;
		}
	}

	public void InitForRuntime(CarModel carModel, RoomsBackend.RoomAccessor backend)
	{
		this.carModel = carModel;
		backendAccessor_ = backend;
	}
}
