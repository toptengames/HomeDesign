using System.Collections.Generic;
using UnityEngine;

public class CarModelColliders : MonoBehaviour
{
	private class State
	{
		public CarModelPart part;
	}

	private class OutState
	{
		public bool hasCollider;

		public bool isInColliderSubtree;

		public CarModelSubpart subpart;
	}

	public void Init(CarModel model)
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		foreach (Transform item in transform)
		{
			list.Add(item);
		}
		for (int i = 0; i < list.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(list[i].gameObject);
		}
		List<CarModelPart> parts = model.parts;
		for (int j = 0; j < parts.Count; j++)
		{
			CarModelPart carModelPart = parts[j];
			GameObject gameObject = new GameObject(carModelPart.gameObject.name);
			gameObject.transform.parent = base.transform;
			State state = new State();
			state.part = carModelPart;
			carModelPart.colliderRoot = gameObject.transform;
			CopyRecursively(gameObject.transform, carModelPart.transform, state, new OutState());
		}
	}

	private void CopyRecursively(Transform localParent, Transform searchParent, State state, OutState outState)
	{
		GGUtil.CopyWorldTransform(searchParent, localParent);
		foreach (Transform item in searchParent)
		{
			OutState outState2 = new OutState();
			outState2.isInColliderSubtree = outState.isInColliderSubtree;
			outState2.subpart = outState.subpart;
			CarModelSubpart component = item.GetComponent<CarModelSubpart>();
			if (component != null)
			{
				outState2.subpart = component;
			}
			MeshFilter component2 = item.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				Mesh sharedMesh = component2.sharedMesh;
			}
			Collider component3 = item.GetComponent<Collider>();
			bool num = component3 != null;
			if (item.name.ToLower().Contains("_collider"))
			{
				outState2.isInColliderSubtree = true;
			}
			GameObject gameObject = null;
			if (num)
			{
				outState.hasCollider = true;
				gameObject = UnityEngine.Object.Instantiate(item.gameObject, localParent);
				StripObject(gameObject);
				if (!outState2.isInColliderSubtree)
				{
					component3.enabled = false;
				}
				gameObject.GetComponent<Collider>().enabled = true;
				PartCollider partCollider = gameObject.AddComponent<PartCollider>();
				partCollider.part = state.part;
				partCollider.subpart = outState2.subpart;
			}
			else
			{
				gameObject = new GameObject(item.name);
			}
			bool num2 = item.GetComponent<PaintTransformation>() != null;
			gameObject.name = item.name;
			gameObject.transform.parent = localParent;
			GGUtil.SetActive(gameObject, active: true);
			if (!num2)
			{
				CopyRecursively(gameObject.transform, item, state, outState2);
				if (outState2.hasCollider)
				{
					outState.hasCollider = true;
				}
			}
			if (gameObject.GetComponent<Collider>() == null && !outState2.hasCollider)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
	}

	private void StripObject(GameObject objectToStrip)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in objectToStrip.transform)
		{
			list.Add(item);
		}
		Component[] components = objectToStrip.GetComponents(typeof(Component));
		foreach (Component component in components)
		{
			if (!(component == null) && !(component is Transform) && !(component is Collider))
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(list[j].gameObject);
		}
	}
}
