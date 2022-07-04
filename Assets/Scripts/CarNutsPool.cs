using System;
using UnityEngine;

[Serializable]
public class CarNutsPool
{
	[SerializeField]
	private ComponentPool nutComponents = new ComponentPool();

	public void Clear()
	{
		nutComponents.Clear();
		nutComponents.HideNotUsed();
	}

	public CarNut NextNut()
	{
		return nutComponents.Next<CarNut>();
	}
}
