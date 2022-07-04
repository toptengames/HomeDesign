using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarNut : MonoBehaviour
{
	private sealed class _003CDoRotateIn_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarNut _003C_003E4__this;

		public Vector3 fromPosition;

		public Vector3 toPosition;

		public float duration;

		private float _003Ctime_003E5__2;

		private float _003Crotations_003E5__3;

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
		public _003CDoRotateIn_003Ed__9(int _003C_003E1__state)
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
			CarNut carNut = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				carNut.screwInTransform.localRotation = Quaternion.identity;
				carNut.transform.position = fromPosition;
				_003Ctime_003E5__2 = 0f;
				_003Crotations_003E5__3 = Vector3.Distance(fromPosition, toPosition) / carNut.nutSize * carNut.rotationsTillLength;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__2 <= duration)
			{
				_003Ctime_003E5__2 += Time.deltaTime;
				float t = Mathf.InverseLerp(0f, duration, _003Ctime_003E5__2);
				Vector3 position = Vector3.Lerp(fromPosition, toPosition, t);
				float angle = Mathf.Lerp(0f, _003Crotations_003E5__3 * 360f, t);
				carNut.screwInTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
				carNut.transform.position = position;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
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
	private Transform screwInTransform;

	[SerializeField]
	private Transform tailTransfrom;

	[SerializeField]
	private Transform headTransfrom;

	[SerializeField]
	private float rotationsTillLength = 20f;

	public float nutSize => Vector3.Distance(headTransfrom.position, tailTransfrom.position);

	public void Init()
	{
		screwInTransform.localRotation = Quaternion.identity;
	}

	public void SetRotation(Quaternion rotation)
	{
		base.transform.rotation = rotation * Quaternion.Euler(0f, 90f, 90f);
	}

	public void SetRotateIn(Vector3 fromPosition, Vector3 toPosition, float n)
	{
		screwInTransform.localRotation = Quaternion.identity;
		base.transform.position = fromPosition;
		float num = Vector3.Distance(fromPosition, toPosition) / nutSize * rotationsTillLength;
		Vector3 position = Vector3.Lerp(fromPosition, toPosition, n);
		float angle = Mathf.Lerp(0f, num * 360f, n);
		screwInTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
		base.transform.position = position;
	}

	public IEnumerator DoRotateIn(Vector3 fromPosition, Vector3 toPosition, float duration)
	{
		return new _003CDoRotateIn_003Ed__9(0)
		{
			_003C_003E4__this = this,
			fromPosition = fromPosition,
			toPosition = toPosition,
			duration = duration
		};
	}
}
