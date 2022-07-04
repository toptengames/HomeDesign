using GGMatch3;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableSelectCarScreenButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI titleLabel;

	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private RectTransform lockedStyle;

	[SerializeField]
	private RectTransform completeStyle;

	[SerializeField]
	private RectTransform unlockAnimation;

	[SerializeField]
	private RectTransform passAnimation;

	[SerializeField]
	public float unlockAnimationDuration;

	[SerializeField]
	public float passAnimationDuration;

	[SerializeField]
	private Image mainImage;

	[NonSerialized]
	public CarsDB.Car car;

	public void Init(CarsDB.Car car)
	{
		this.car = car;
		GGUtil.SetActive(widgetsToHide, active: false);
		if (car.isPassed)
		{
			GGUtil.Show(completeStyle);
		}
		else if (car.isLocked)
		{
			GGUtil.Show(lockedStyle);
		}
		GGUtil.ChangeText(titleLabel, car.displayName);
		GGUtil.ChangeText(descriptionLabel, car.description);
		GGUtil.SetSprite(mainImage, car.cardSprite);
	}

	public void ShowPassedAnimation()
	{
		GGUtil.SetActive(widgetsToHide, active: false);
		GGUtil.Show(passAnimation);
	}

	public void ShowUnlockAnimation()
	{
		GGUtil.SetActive(widgetsToHide, active: false);
		GGUtil.Show(unlockAnimation);
	}

	public void ShowOpenNotPassed()
	{
		GGUtil.SetActive(widgetsToHide, active: false);
	}

	public void ShowLocked()
	{
		GGUtil.SetActive(widgetsToHide, active: false);
		GGUtil.Show(lockedStyle);
	}

	public void Callback_OnClick()
	{
		UnityEngine.Debug.Log("OPEN ROOM " + car.name);
		if (!Application.isEditor && !car.isOpen)
		{
			GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
			return;
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
		int num2 = SingletonInit<RoomsBackend>.instance.selectedRoomIndex = ScriptableObjectSingleton<CarsDB>.instance.IndexOf(car);
		NavigationManager.instance.Pop();
	}
}
