using System;
using TMPro;
using UnityEngine;

public class CompletionDialogTask : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	[SerializeField]
	private TextMeshProUGUI priceLabel;

	private Action<CompletionDialogTask> onComplete;

	[NonSerialized]
	public CompletionDialog.InitArguments.Task task;

	public void Init(CompletionDialog.InitArguments.Task task, Action<CompletionDialogTask> onComplete)
	{
		this.task = task;
		GGUtil.ChangeText(headerLabel, task.name);
		GGUtil.ChangeText(priceLabel, task.price);
		this.onComplete = onComplete;
	}

	public void ButtonCallback_OnPress()
	{
		onComplete?.Invoke(this);
	}
}
