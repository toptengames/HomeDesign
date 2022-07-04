using GGMatch3;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPurchaseDialogPowerupPrefab : MonoBehaviour
{
	[Serializable]
	public class NamedVisualConfigs
	{
		public CurrencyPurchaseDialogPowerupVisualConfig visualConfig;

		public BoosterType type;

		public bool IsMatching(OffersDB.OfferConfig config)
		{
			return config.boosterType == type;
		}

		public void SetLabel(string text)
		{
			visualConfig.SetLabel(text);
		}

		public void SetActive(bool flag)
		{
			GGUtil.SetActive(visualConfig.gameObject, flag);
		}
	}

	[SerializeField]
	private List<NamedVisualConfigs> visualConfigs = new List<NamedVisualConfigs>();

	public void Init(OffersDB.OfferConfig config)
	{
		for (int i = 0; i < visualConfigs.Count; i++)
		{
			NamedVisualConfigs namedVisualConfigs = visualConfigs[i];
			if (namedVisualConfigs.IsMatching(config))
			{
				namedVisualConfigs.SetLabel($"x {config.count}");
				namedVisualConfigs.SetActive(flag: true);
			}
			else
			{
				namedVisualConfigs.SetActive(flag: false);
			}
		}
	}
}
