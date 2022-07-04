using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarModel : MonoBehaviour
{
	public struct ProgressState
	{
		public int completed;

		public int total;

		public bool isPassed => completed >= total;

		public float progress => Mathf.InverseLerp(0f, total, completed);

		public float Progress(int removeCompleted)
		{
			return Mathf.InverseLerp(0f, total, completed - removeCompleted);
		}
	}

	private sealed class _003CDoShowChange_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float scaleMult;

		public CarModel _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private float _003Cduration_003E5__3;

		private Vector3 _003CstartPosition_003E5__4;

		private AnimationCurve _003Ccurve_003E5__5;

		private Vector3 _003CendPosition_003E5__6;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CDoShowChange_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CarModel carModel = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CarModelSubpart.BlinkSettings subpartBlinkSettings = ScriptableObjectSingleton<CarsDB>.instance.subpartBlinkSettings;
				_003Ctime_003E5__2 = 0f;
				_003Cduration_003E5__3 = subpartBlinkSettings.inDuration;
				Vector3 up = Vector3.up;
				_003CstartPosition_003E5__4 = Vector3.zero + up * subpartBlinkSettings.changeOffset * scaleMult;
				_003Ccurve_003E5__5 = subpartBlinkSettings.moveCurve;
				_003CendPosition_003E5__6 = Vector3.zero;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__2 <= _003Cduration_003E5__3)
			{
				_003Ctime_003E5__2 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Cduration_003E5__3, _003Ctime_003E5__2);
				float t = _003Ccurve_003E5__5.Evaluate(time);
				Vector3.LerpUnclamped(_003CstartPosition_003E5__4, _003CendPosition_003E5__6, t);
				carModel.transform.localScale = Vector3.LerpUnclamped(Vector3.one * 1.025f, Vector3.one, t);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			carModel.transform.localScale = Vector3.one;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[SerializeField]
	public CarNutsPool nuts = new CarNutsPool();

	[SerializeField]
	public List<CarModelPart> parts = new List<CarModelPart>();

	[SerializeField]
	public CarModelInfo modelInfo = new CarModelInfo();

	[SerializeField]
	private CarModelColliders colliders;

	[SerializeField]
	private bool indexAsGroup;

	[NonSerialized]
	private IEnumerator animation;

	public ExplodeAnimation explodeAnimation = new ExplodeAnimation();

	private List<CarModelPart> availableCarModelParts_ = new List<CarModelPart>();

	private List<CarModelSubpart> subpartsHelperList_ = new List<CarModelSubpart>();

	private RoomsBackend.RoomAccessor roomBackend => SingletonInit<RoomsBackend>.instance.GetRoom(base.name);

	public bool isPassed
	{
		get
		{
			return roomBackend.isPassed;
		}
		set
		{
			roomBackend.isPassed = value;
		}
	}

	public List<CarModelPart> AvailablePartsAsTasks()
	{
		availableCarModelParts_.Clear();
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (!carModelPart.partInfo.isDefault && !carModelPart.partInfo.isOwned && carModelPart.partInfo.isUnlocked)
			{
				availableCarModelParts_.Add(carModelPart);
			}
		}
		return availableCarModelParts_;
	}

	private CarModelColliders GetOrCreateCarModelColliders()
	{
		if (colliders != null)
		{
			return colliders;
		}
		string name = "ColliderRootTransform";
		foreach (Transform item in base.transform)
		{
			CarModelColliders component = item.GetComponent<CarModelColliders>();
			if (component != null)
			{
				return component;
			}
		}
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		return gameObject.AddComponent<CarModelColliders>();
	}

	public void SetCollidersActive(bool active)
	{
		GGUtil.SetActive(colliders, active);
	}

	public void InitializeParts()
	{
		parts.Clear();
		foreach (Transform item in base.transform)
		{
			if (!item.name.ToLower().Contains("_ignore") && !item.GetComponent<CarModelColliders>())
			{
				CarModelPart carModelPart = item.GetComponent<CarModelPart>();
				if (carModelPart == null)
				{
					carModelPart = item.gameObject.AddComponent<CarModelPart>();
				}
				parts.Add(carModelPart);
				carModelPart.Init(this);
				if (indexAsGroup)
				{
					carModelPart.partInfo.groupIndex = parts.Count - 1;
				}
			}
		}
		InitPhysics();
	}

	public void InitPhysics()
	{
		colliders = GetOrCreateCarModelColliders();
		colliders.Init(this);
	}

	public bool IsAllElementsPickedUpInGroup(int index)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (carModelPart.partInfo.groupIndex == index && !carModelPart.partInfo.isOwned)
			{
				return false;
			}
		}
		return true;
	}

	public int SelectedIndexForVariantGroup(string groupName)
	{
		return modelInfo.GetVariantGroup(groupName)?.selectedVariationIndex ?? 0;
	}

	public void RefreshVariations()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].SetActiveIfOwned();
		}
	}

	public void InitForRuntime()
	{
		RoomsBackend.RoomAccessor room = SingletonInit<RoomsBackend>.instance.GetRoom(base.name);
		modelInfo.Init(room);
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].InitForRuntime(room);
		}
		for (int j = 0; j < parts.Count; j++)
		{
			parts[j].SetActiveIfOwned();
		}
		InitExplodeAnimation();
	}

	public void RefreshVisibilityOnParts()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].SetActiveIfOwned();
		}
	}

	public ProgressState GetProgressState()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (!carModelPart.partInfo.isDefault)
			{
				num2++;
				if (carModelPart.partInfo.isOwned)
				{
					num++;
				}
			}
		}
		ProgressState result = default(ProgressState);
		result.completed = num;
		result.total = num2;
		return result;
	}

	public void InitExplodeAnimation()
	{
		explodeAnimation.Init(this);
	}

	public List<CarModelSubpart> AllOwnedSubpartsInVariantGroup(CarModelInfo.VariantGroup group)
	{
		subpartsHelperList_.Clear();
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (!carModelPart.partInfo.isOwned)
			{
				continue;
			}
			bool flag = carModelPart.partInfo.animateChangeWithVariations.Contains(group.name);
			List<CarModelSubpart> subparts = carModelPart.subparts;
			for (int j = 0; j < subparts.Count; j++)
			{
				CarModelSubpart carModelSubpart = subparts[j];
				if (flag || carModelSubpart.HasVariantForGroup(group))
				{
					subpartsHelperList_.Add(carModelSubpart);
				}
			}
		}
		return subpartsHelperList_;
	}

	public void ShowChnage()
	{
		animation = DoShowChange(1f);
		animation.MoveNext();
	}

	private IEnumerator DoShowChange(float scaleMult)
	{
		return new _003CDoShowChange_003Ed__29(0)
		{
			_003C_003E4__this = this,
			scaleMult = scaleMult
		};
	}

	private void Update()
	{
		if (animation != null && !animation.MoveNext())
		{
			animation = null;
		}
	}
}
