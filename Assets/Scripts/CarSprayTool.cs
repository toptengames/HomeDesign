using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSprayTool : MonoBehaviour
{
	public struct InitArguments
	{
		public AssembleCarScreen screen;

		public PaintTransformation paintTransformation;

		public Action onDone;
	}

	[SerializeField]
	private Image fillImage;

	[SerializeField]
	private TextMeshProUGUI fillText;

	[SerializeField]
	private CarSprayToolTarget sprayTool;

	[SerializeField]
	private float viewportBoundsX = 0.5f;

	[SerializeField]
	private float viewportBoundsY = 0.9f;

	[SerializeField]
	private Vector3 viewportSpeed = Vector3.one;

	private float fillTreshold = 0.8f;

	private float spraySize = 0.5f;

	private InitArguments initArguments;

	private float currentFillPercentage;

	private AssembleCarScreen screen => initArguments.screen;

	public PaintTransformation paintTransformation => initArguments.paintTransformation;

	public void Init(InitArguments initArguments)
	{
		this.initArguments = initArguments;
		currentFillPercentage = 0f;
		GGUtil.Show(this);
		sprayTool.Init(OnDrag);
		sprayTool.transform.localPosition = Vector3.zero;
		UpdateFill(currentFillPercentage);
	}

	private void UpdateFill(float percentage)
	{
		float num = Mathf.InverseLerp(0f, fillTreshold, percentage);
		GGUtil.SetFill(fillImage, num);
		string text = $"{Mathf.FloorToInt(num * 100f)}%";
		GGUtil.ChangeText(fillText, text);
	}

	private void OnDrag(Vector3 screenPosition)
	{
		Ray ray = screen.scene.camera.ScreenPointToRay(screenPosition);
		if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{
			GameObject gameObject = hitInfo.collider.gameObject;
			Vector3 point = ray.GetPoint(hitInfo.distance);
			HandleHitPoint(point);
		}
	}

	private void Update()
	{
		if (sprayTool.dragState.isDragging)
		{
			HandleCamera(sprayTool.dragState.lastScreenPosition);
		}
		else if (currentFillPercentage >= fillTreshold)
		{
			ButtonCallback_OnFinish();
		}
	}

	private void HandleCamera(Vector3 screenPosition)
	{
		CarCamera camera = screen.scene.camera;
		Vector3 a = camera.ScreenToViewPortPoint(screenPosition);
		a = a * 2f - Vector3.one;
		float num = Mathf.InverseLerp(viewportBoundsX, 1f, Mathf.Abs(a.x));
		float num2 = Mathf.InverseLerp(viewportBoundsX, 1f, Mathf.Abs(a.y));
		if (num > 0f || num2 > 0f)
		{
			num *= viewportSpeed.x * Time.deltaTime;
			num2 *= viewportSpeed.y * Time.deltaTime;
			Vector2 distance = new Vector2(num * Mathf.Sign(a.x), num2 * Mathf.Sign(a.y));
			camera.Move(distance);
		}
	}

	private void HandleHitPoint(Vector3 hitPointWorldPosition)
	{
		if (!(paintTransformation == null))
		{
			GGPSphereCommand.Params sphereParams = default(GGPSphereCommand.Params);
			sphereParams.brushColor = Color.white;
			sphereParams.brushHardness = 0.01f;
			sphereParams.brushSize = spraySize;
			sphereParams.worldPosition = hitPointWorldPosition;
			paintTransformation.RenderSphere(sphereParams);
			currentFillPercentage = paintTransformation.FillPercent();
			UpdateFill(currentFillPercentage);
		}
	}

	public void ButtonCallback_OnFinish()
	{
		if (initArguments.onDone != null)
		{
			initArguments.onDone();
		}
	}
}
