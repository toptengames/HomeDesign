using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ExplodeSlider : MonoBehaviour
{
	[Serializable]
	public class ExplosionSettings
	{
		public float minValueWhenSwitch = 0.1f;

		public float distanceFromCenter = 3f;

		public float durationToReturn = 0.5f;

		public AnimationCurve unexplodeTimeCurve;

		public AnimationCurve sliderCurve;
	}

	private sealed class _003CSpringOnSlider_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExplodeSlider _003C_003E4__this;

		public float change;

		private float _003CcurrentTime_003E5__2;

		private ExplodeAnimation _003CexplodeAnimation_003E5__3;

		private float _003CendTime_003E5__4;

		private float _003Cduration_003E5__5;

		private float _003Ctime_003E5__6;

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
		public _003CSpringOnSlider_003Ed__18(int _003C_003E1__state)
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
			ExplodeSlider explodeSlider = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (explodeSlider.scene == null)
				{
					return false;
				}
				float num2 = _003CcurrentTime_003E5__2 = explodeSlider.slider.value;
				_003CexplodeAnimation_003E5__3 = explodeSlider.scene.carModel.explodeAnimation;
				_003CendTime_003E5__4 = _003CexplodeAnimation_003E5__3.ClosestFullTime(_003CcurrentTime_003E5__2, change);
				float num3 = 1f;
				_003Cduration_003E5__5 = Mathf.Abs(_003CendTime_003E5__4 - _003CcurrentTime_003E5__2) * num3;
				_003Ctime_003E5__6 = 0f;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__6 <= _003Cduration_003E5__5)
			{
				_003Ctime_003E5__6 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Cduration_003E5__5, _003Ctime_003E5__6);
				float t = explodeSlider.settings.sliderCurve.Evaluate(time);
				float num4 = Mathf.Lerp(_003CcurrentTime_003E5__2, _003CendTime_003E5__4, t);
				explodeSlider.slider.value = num4;
				_003CexplodeAnimation_003E5__3.SetTimeTo(num4);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			explodeSlider.UpdateButtonActive(explodeSlider.slider.value);
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

	private sealed class _003CUnexplode_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExplodeSlider _003C_003E4__this;

		private float _003CstartValue_003E5__2;

		private float _003Cduration_003E5__3;

		private float _003Ctime_003E5__4;

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
		public _003CUnexplode_003Ed__19(int _003C_003E1__state)
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
			ExplodeSlider explodeSlider = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				GGUtil.SetActive(explodeSlider.unexplodeButton, active: false);
				_003CstartValue_003E5__2 = explodeSlider.slider.value;
				_003Cduration_003E5__3 = explodeSlider.settings.durationToReturn;
				_003Ctime_003E5__4 = 0f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__4 < _003Cduration_003E5__3)
			{
				_003Ctime_003E5__4 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Cduration_003E5__3, _003Ctime_003E5__4);
				float t = explodeSlider.settings.unexplodeTimeCurve.Evaluate(time);
				float num2 = Mathf.Lerp(_003CstartValue_003E5__2, 0f, t);
				explodeSlider.slider.value = num2;
				explodeSlider.scene.carModel.explodeAnimation.SetTimeTo(num2);
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
	private Slider slider;

	[SerializeField]
	private Transform unexplodeButton;

	[NonSerialized]
	private AssembleCarScreen screen;

	[NonSerialized]
	private IEnumerator sliderAnim;

	[NonSerialized]
	private float sliderValueOnDragStart;

	public ExplosionSettings settings => ScriptableObjectSingleton<CarsDB>.instance.explosionSettings;

	public float value => slider.value;

	public bool isExploded => value > 0f;

	private CarScene scene
	{
		get
		{
			if (screen == null)
			{
				return null;
			}
			return screen.scene;
		}
	}

	public void Reset()
	{
		StopSlider();
		slider.value = 0f;
		UpdateButtonActive(0f);
	}

	public void Init(AssembleCarScreen screen)
	{
		this.screen = screen;
		scene.carModel.explodeAnimation.SetTimeTo(slider.value);
		ExplodeAnimation explodeAnimation = screen.scene.carModel.explodeAnimation;
		GGUtil.SetActive(this, explodeAnimation.hasParts);
		UpdateButtonActive(slider.value);
	}

	public void StopSlider()
	{
		sliderAnim = null;
	}

	private void UpdateButtonActive(float value)
	{
		GGUtil.SetActive(unexplodeButton, value > 0f);
	}

	private IEnumerator SpringOnSlider(float change)
	{
		return new _003CSpringOnSlider_003Ed__18(0)
		{
			_003C_003E4__this = this,
			change = change
		};
	}

	public IEnumerator Unexplode()
	{
		return new _003CUnexplode_003Ed__19(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ButtonCallbacks_OnUnexplode()
	{
		sliderAnim = Unexplode();
	}

	public void SliderCallback_OnDragStart()
	{
		sliderValueOnDragStart = slider.value;
	}

	public void SliderCallback_OnDragEnd()
	{
		float change = slider.value - sliderValueOnDragStart;
		sliderAnim = SpringOnSlider(change);
	}

	public void SliderCallback_OnValueChanged()
	{
		if (!(scene == null))
		{
			scene.carModel.explodeAnimation.SetTimeTo(slider.value);
		}
	}

	private void Update()
	{
		if (sliderAnim != null && !sliderAnim.MoveNext())
		{
			sliderAnim = null;
		}
	}
}
