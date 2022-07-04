using GGMatch3;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CompletionDialog : UILayer
{
	public class InitArguments
	{
		public struct Task
		{
			public string name;

			public int price;

			public CarModelPart part;
		}

		public bool showModal;

		public List<Task> tasks = new List<Task>();

		public Action<Task> onComplete;

		public Action onCancel;
	}

	[SerializeField]
	private ComponentPool tasksPool = new ComponentPool();

	[SerializeField]
	private float padding = 20f;

	[NonSerialized]
	private InitArguments initArguments;

	public void Show(InitArguments init)
	{
		initArguments = init;
		NavigationManager.instance.Push(base.gameObject, init.showModal);
	}

	private void OnEnable()
	{
		Init(initArguments);
	}

	public void Init(InitArguments init)
	{
		initArguments = init;
		List<InitArguments.Task> tasks = init.tasks;
		Vector2 prefabSizeDelta = tasksPool.prefabSizeDelta;
		UnityEngine.Debug.Log("Size " + prefabSizeDelta.x);
		float num = (float)tasks.Count * prefabSizeDelta.x + (float)(tasks.Count - 1) * padding;
		Vector2 a = new Vector2((0f - num) * 0.5f, 0f);
		tasksPool.Clear();
		for (int i = 0; i < tasks.Count; i++)
		{
			InitArguments.Task task = tasks[i];
			Vector2 v = a + Vector2.right * ((float)i * (prefabSizeDelta.x + padding) + prefabSizeDelta.x * 0.5f);
			CompletionDialogTask completionDialogTask = tasksPool.Next<CompletionDialogTask>();
			completionDialogTask.transform.localPosition = v;
			completionDialogTask.Init(task, CompleteDialogTask_OnPressed);
			GGUtil.Show(completionDialogTask);
		}
		tasksPool.HideNotUsed();
	}

	private void CompleteDialogTask_OnPressed(CompletionDialogTask task)
	{
		initArguments.onComplete?.Invoke(task.task);
	}

	public override void OnGoBack(NavigationManager nav)
	{
		GGSoundSystem.Play(backSound);
		initArguments.onCancel?.Invoke();
	}
}
