using UnityEngine;

public class VariantGroupSetup : MonoBehaviour
{
	[SerializeField]
	public CarModelInfo.VariantGroup settings = new CarModelInfo.VariantGroup();

	public void Apply()
	{
		CarModel componentInParent = base.transform.GetComponentInParent<CarModel>();
		if (string.IsNullOrWhiteSpace(settings.name))
		{
			UnityEngine.Debug.Log("VARIANT GROUP NAME EMPTY " + base.transform.name);
			return;
		}
		componentInParent.modelInfo.RemoveVariantGroup(settings.name);
		componentInParent.modelInfo.AddGroup(settings);
	}
}
