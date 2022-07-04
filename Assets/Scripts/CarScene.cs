using UnityEngine;

public class CarScene : MonoBehaviour
{
	[SerializeField]
	public CarModel carModel;

	[SerializeField]
	public CarCamera camera;

	public T CreateFromPrefab<T>(GameObject prefab) where T : MonoBehaviour
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
		gameObject.transform.parent = base.transform;
		GGUtil.SetLayerRecursively(gameObject, carModel.gameObject.layer);
		return gameObject.GetComponent<T>();
	}
}
