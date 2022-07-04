using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation
{
	public class PartDefinition
	{
		public float startTime;

		public float endTime;

		public int sortingGroupIndex;

		public List<CarModelPart> parts = new List<CarModelPart>();
	}

	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Comparison<PartDefinition> _003C_003E9__6_0;

		internal int _003CInit_003Eb__6_0(PartDefinition a, PartDefinition b)
		{
			return b.sortingGroupIndex.CompareTo(a.sortingGroupIndex);
		}
	}

	[NonSerialized]
	private CarModel model;

	private List<PartDefinition> partDefinitions = new List<PartDefinition>();

	public bool hasParts => partDefinitions.Count > 0;

	private PartDefinition GetOrCreatePartDefinition(int sortingGroupIndex)
	{
		for (int i = 0; i < partDefinitions.Count; i++)
		{
			PartDefinition partDefinition = partDefinitions[i];
			if (partDefinition.sortingGroupIndex == sortingGroupIndex)
			{
				return partDefinition;
			}
		}
		PartDefinition partDefinition2 = new PartDefinition();
		partDefinition2.sortingGroupIndex = sortingGroupIndex;
		partDefinitions.Add(partDefinition2);
		return partDefinition2;
	}

	public void Init(CarModel model)
	{
		this.model = model;
		partDefinitions.Clear();
		List<CarModelPart> parts = model.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (!carModelPart.partInfo.suspendExploding && carModelPart.partInfo.isOwned)
			{
				GetOrCreatePartDefinition(carModelPart.partInfo.explodeGroupIndex).parts.Add(carModelPart);
			}
		}
		partDefinitions.Sort(_003C_003Ec._003C_003E9._003CInit_003Eb__6_0);
		if (partDefinitions.Count != 0)
		{
			float num = 1f / (float)partDefinitions.Count;
			for (int j = 0; j < partDefinitions.Count; j++)
			{
				PartDefinition partDefinition = partDefinitions[j];
				partDefinition.startTime = (float)j * num;
				partDefinition.endTime = (float)(j + 1) * num;
				partDefinitions[j] = partDefinition;
			}
		}
	}

	public float ClosestFullTime(float time, float changeDirection)
	{
		if (partDefinitions.Count == 0)
		{
			return 0f;
		}
		float num = 1f / (float)partDefinitions.Count;
		float num2 = Mathf.FloorToInt(time / num);
		float num3 = (time - num2 * num) / num;
		float num4 = ScriptableObjectSingleton<CarsDB>.instance.explosionSettings.minValueWhenSwitch;
		if (changeDirection < 0f)
		{
			num4 = 1f - num4;
		}
		float value = num2 * num;
		if (num3 > num4)
		{
			value = (num2 + 1f) * num;
		}
		return Mathf.Clamp01(value);
	}

	public void SetTimeTo(float time)
	{
		for (int i = 0; i < partDefinitions.Count; i++)
		{
			PartDefinition partDefinition = partDefinitions[i];
			float explodeOffset = Mathf.InverseLerp(partDefinition.startTime, partDefinition.endTime, time);
			for (int j = 0; j < partDefinition.parts.Count; j++)
			{
				partDefinition.parts[j].SetExplodeOffset(explodeOffset);
			}
		}
	}
}
