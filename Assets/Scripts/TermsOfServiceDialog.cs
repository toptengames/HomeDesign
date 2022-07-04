using GGMatch3;
using System;
using UnityEngine;

public class TermsOfServiceDialog : MonoBehaviour
{
	private Action<bool> onComplete;

	public void Show(Action<bool> onComplete)
	{
		this.onComplete = onComplete;
		NavigationManager.instance.Push(base.gameObject, isModal: true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void ButtonCallback_OnCancel()
	{
		if (onComplete != null)
		{
			onComplete(obj: false);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void ButtonCallback_OnOK()
	{
		if (onComplete != null)
		{
			onComplete(obj: true);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnTermsOfService()
	{
		Application.OpenURL("http://www.giraffe-games.com/terms-of-use/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnPrivacyPolicy()
	{
		Application.OpenURL("http://www.giraffe-games.com/privacy-policy/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}
}
