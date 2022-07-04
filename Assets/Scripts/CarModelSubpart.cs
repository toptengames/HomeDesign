using GGMatch3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarModelSubpart : MonoBehaviour
{
	[Serializable]
	public class Settings
	{
		public float inDuration = 1f;

		public float fromScale;

		public float toScale = 1f;

		public AnimationCurve scaleCurve;

		public float moveDuration;

		public AnimationCurve moveCurve;

		public float openDuration = 1f;
	}

	[Serializable]
	public class BlinkSettings
	{
		public float inDuration = 1f;

		public float fromScale;

		public float toScale = 1f;

		public AnimationCurve scaleCurve;

		public float moveDuration;

		public float fromScaleChange = 0.99f;

		public float changeOffset = 0.1f;

		public AnimationCurve moveCurve;
	}

	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public int variant;

		internal bool _003CRemoveAllModificationsOfVariant_003Eb__0(VariantModification mod)
		{
			return mod.variantIndex == variant;
		}
	}

	private sealed class _003CInAnimationScaleBounce_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		private Settings _003Csettings_003E5__2;

		private float _003Ctime_003E5__3;

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
		public _003CInAnimationScaleBounce_003Ed__30(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Csettings_003E5__2 = carModelSubpart.settings;
				_003Ctime_003E5__3 = 0f;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__3 < _003Csettings_003E5__2.inDuration)
			{
				_003Ctime_003E5__3 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, _003Csettings_003E5__2.inDuration, _003Ctime_003E5__3);
				float t = _003Csettings_003E5__2.scaleCurve.Evaluate(time);
				float d = Mathf.LerpUnclamped(_003Csettings_003E5__2.fromScale, _003Csettings_003E5__2.toScale, t);
				Vector3 localScale = carModelSubpart.scaleAtStart * d;
				carModelSubpart.transform.localScale = localScale;
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

	private sealed class _003CDoChangeRotation_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private float _003Cduration_003E5__3;

		private Quaternion _003CstartRotation_003E5__4;

		private Quaternion _003CendRotation_003E5__5;

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
		public _003CDoChangeRotation_003Ed__41(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003Ctime_003E5__2 = 0f;
				CarSubPartInfo.RotateSettings rotateSettings = carModelSubpart.subpartInfo.rotateSettings;
				_003Cduration_003E5__3 = carModelSubpart.settings.openDuration;
				_003CstartRotation_003E5__4 = carModelSubpart.transform.localRotation;
				float angle = carModelSubpart.isOpen ? rotateSettings.initialAngle : rotateSettings.outAngle;
				carModelSubpart.isOpen = !carModelSubpart.isOpen;
				_003CendRotation_003E5__5 = Quaternion.AngleAxis(angle, rotateSettings.axis);
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Ctime_003E5__2 < _003Cduration_003E5__3)
			{
				_003Ctime_003E5__2 += Time.deltaTime;
				float t = Mathf.InverseLerp(0f, _003Cduration_003E5__3, _003Ctime_003E5__2);
				carModelSubpart.transform.localRotation = Quaternion.Lerp(_003CstartRotation_003E5__4, _003CendRotation_003E5__5, t);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			carModelSubpart.transform.localRotation = _003CendRotation_003E5__5;
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

	private sealed class _003CDoShowChange_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		public float scaleMult;

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
		public _003CDoShowChange_003Ed__45(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				BlinkSettings blinkSettings = carModelSubpart.blinkSettings;
				_003Ctime_003E5__2 = 0f;
				_003Cduration_003E5__3 = blinkSettings.inDuration;
				Vector3 normalized = carModelSubpart.subpartInfo.offset.normalized;
				if (carModelSubpart.subpartInfo.overrideChangeAnimOffset)
				{
					normalized = carModelSubpart.subpartInfo.changeAnimOffset.normalized;
				}
				_003CstartPosition_003E5__4 = carModelSubpart.localPositionAtStart + normalized * blinkSettings.changeOffset * scaleMult;
				_003Ccurve_003E5__5 = blinkSettings.moveCurve;
				_003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
				goto IL_012a;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_012a;
			case 2:
				{
					_003C_003E1__state = -1;
					BlinkSettings blinkSettings = default(BlinkSettings);
					if (_003Ctime_003E5__2 < blinkSettings.inDuration)
					{
						_003Ctime_003E5__2 += Time.deltaTime;
						float time = Mathf.InverseLerp(0f, blinkSettings.inDuration, _003Ctime_003E5__2);
						float t = blinkSettings.scaleCurve.Evaluate(time);
						float d = Mathf.LerpUnclamped(blinkSettings.fromScaleChange, blinkSettings.toScale, t);
						Vector3 localScale = carModelSubpart.scaleAtStart * d;
						carModelSubpart.transform.localScale = localScale;
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				IL_012a:
				if (_003Ctime_003E5__2 <= _003Cduration_003E5__3)
				{
					_003Ctime_003E5__2 += Time.deltaTime;
					float time2 = Mathf.InverseLerp(0f, _003Cduration_003E5__3, _003Ctime_003E5__2);
					float t2 = _003Ccurve_003E5__5.Evaluate(time2);
					Vector3 localPosition = Vector3.LerpUnclamped(_003CstartPosition_003E5__4, _003CendPosition_003E5__6, t2);
					carModelSubpart.transform.localPosition = localPosition;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003CstartPosition_003E5__4 = default(Vector3);
				_003Ccurve_003E5__5 = null;
				_003CendPosition_003E5__6 = default(Vector3);
				break;
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

	private sealed class _003CShowNutAnimations_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		private CarNutsPool _003Cnuts_003E5__2;

		private Vector3 _003CoffsetDirection_003E5__3;

		private int _003Ci_003E5__4;

		private IEnumerator _003CrotationAnimation_003E5__5;

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
		public _003CShowNutAnimations_003Ed__46(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0145;
			}
			_003C_003E1__state = -1;
			_003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
			_003Cnuts_003E5__2.Clear();
			new List<CarNut>();
			_003CoffsetDirection_003E5__3 = carModelSubpart.subpartInfo.offset.normalized;
			_003Ci_003E5__4 = 0;
			goto IL_016b;
			IL_0145:
			if (_003CrotationAnimation_003E5__5.MoveNext())
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003CrotationAnimation_003E5__5 = null;
			_003Ci_003E5__4++;
			goto IL_016b;
			IL_016b:
			if (_003Ci_003E5__4 < carModelSubpart.nutTransforms.Count)
			{
				Transform transform = carModelSubpart.nutTransforms[_003Ci_003E5__4];
				CarNut carNut = _003Cnuts_003E5__2.NextNut();
				Vector3 position2 = transform.position;
				UnityEngine.Debug.Log("OFFSET DIRECTION " + _003CoffsetDirection_003E5__3);
				Quaternion rotation = Quaternion.LookRotation(_003CoffsetDirection_003E5__3);
				carNut.Init();
				carNut.transform.position = transform.position + _003CoffsetDirection_003E5__3 * carNut.nutSize;
				carNut.SetRotation(rotation);
				GGUtil.Show(carNut);
				Vector3 fromPosition = transform.position + _003CoffsetDirection_003E5__3 * carNut.nutSize;
				Vector3 position = transform.position;
				_003CrotationAnimation_003E5__5 = carNut.DoRotateIn(fromPosition, position, 0.5f);
				goto IL_0145;
			}
			_003Cnuts_003E5__2.Clear();
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

	private sealed class _003C_003Ec__DisplayClass47_0
	{
		public AssembleCarScreen screen;

		public CarModelSubpart _003C_003E4__this;

		public TalkingDialog talkingDialog;
	}

	private sealed class _003C_003Ec__DisplayClass47_1
	{
		public float time;

		public _003C_003Ec__DisplayClass47_0 CS_0024_003C_003E8__locals1;

		internal void _003CShowRemoveNutAnimations_003Eb__0(ScrewdriverTool.PressArguments _003Cp0_003E)
		{
			time += Time.deltaTime;
			CS_0024_003C_003E8__locals1.screen.tutorialHand.Hide();
			if (CS_0024_003C_003E8__locals1._003C_003E4__this.subpartInfo.hideToSayWhenWorking)
			{
				CS_0024_003C_003E8__locals1.talkingDialog.Hide();
			}
		}
	}

	private sealed class _003CShowRemoveNutAnimations_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen screen;

		public CarModelSubpart _003C_003E4__this;

		private _003C_003Ec__DisplayClass47_0 _003C_003E8__1;

		private _003C_003Ec__DisplayClass47_1 _003C_003E8__2;

		private CarNutsPool _003Cnuts_003E5__2;

		private List<CarNut> _003CnutsList_003E5__3;

		private Vector3 _003CoffsetDirection_003E5__4;

		private int _003Ci_003E5__5;

		private CarNut _003Cnut_003E5__6;

		private ScrewdriverTool _003CscrewdriverTool_003E5__7;

		private float _003Cduration_003E5__8;

		private DrillModel _003Cdrill_003E5__9;

		private Vector3 _003CtoPosition_003E5__10;

		private Vector3 _003CfromPosition_003E5__11;

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
		public _003CShowRemoveNutAnimations_003Ed__47(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_03c0;
			}
			_003C_003E1__state = -1;
			_003C_003E8__1 = new _003C_003Ec__DisplayClass47_0();
			_003C_003E8__1.screen = screen;
			_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
			_003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
			_003Cnuts_003E5__2.Clear();
			_003CnutsList_003E5__3 = new List<CarNut>();
			_003CoffsetDirection_003E5__4 = carModelSubpart.subpartInfo.offset.normalized;
			NavigationManager instance = NavigationManager.instance;
			_003C_003E8__1.talkingDialog = instance.GetObject<TalkingDialog>();
			List<string> toSayBefore = carModelSubpart.subpartInfo.toSayBefore;
			if (toSayBefore.Count > 0)
			{
				_003C_003E8__1.talkingDialog.ShowSingleLine(toSayBefore[0]);
			}
			for (int i = 0; i < carModelSubpart.nutTransforms.Count; i++)
			{
				Transform transform = carModelSubpart.nutTransforms[i];
				CarNut carNut = _003Cnuts_003E5__2.NextNut();
				Vector3 position = transform.position;
				Quaternion rotation = Quaternion.LookRotation(_003CoffsetDirection_003E5__4);
				carNut.Init();
				carNut.transform.position = transform.position;
				carNut.SetRotation(rotation);
				GGUtil.Show(carNut);
				_003CnutsList_003E5__3.Add(carNut);
			}
			_003Ci_003E5__5 = 0;
			goto IL_047d;
			IL_047d:
			if (_003Ci_003E5__5 < _003CnutsList_003E5__3.Count)
			{
				_003C_003E8__2 = new _003C_003Ec__DisplayClass47_1();
				_003C_003E8__2.CS_0024_003C_003E8__locals1 = _003C_003E8__1;
				Transform transform2 = carModelSubpart.nutTransforms[_003Ci_003E5__5];
				_003Cnut_003E5__6 = _003CnutsList_003E5__3[_003Ci_003E5__5];
				if (_003Ci_003E5__5 == 0 && carModelSubpart.subpartInfo.showNutTutorialHand)
				{
					TutorialHandController.InitArguments initArguments = default(TutorialHandController.InitArguments);
					Transform transform3 = _003C_003E8__2.CS_0024_003C_003E8__locals1.screen.transform;
					initArguments.endLocalPosition = Vector3.zero;
					initArguments.startLocalPosition = Vector3.zero;
					initArguments.settings = Match3Settings.instance.tutorialHandSettings;
					initArguments.repeat = true;
					_003C_003E8__2.CS_0024_003C_003E8__locals1.screen.tutorialHand.Show(initArguments);
				}
				_003CscrewdriverTool_003E5__7 = _003C_003E8__2.CS_0024_003C_003E8__locals1.screen.screwdriverTool;
				ScrewdriverTool.InitArguments initArguments2 = default(ScrewdriverTool.InitArguments);
				initArguments2.inputHandler = _003C_003E8__2.CS_0024_003C_003E8__locals1.screen.inputHandler;
				_003C_003E8__2.time = 0f;
				_003Cduration_003E5__8 = 1f;
				ref Action<ScrewdriverTool.PressArguments> onPress = ref initArguments2.onPress;
				onPress = (Action<ScrewdriverTool.PressArguments>)Delegate.Combine(onPress, new Action<ScrewdriverTool.PressArguments>(_003C_003E8__2._003CShowRemoveNutAnimations_003Eb__0));
				_003CscrewdriverTool_003E5__7.Init(initArguments2);
				_003Cdrill_003E5__9 = _003CscrewdriverTool_003E5__7.GetDrillModel(_003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene);
				_003CtoPosition_003E5__10 = transform2.position + _003CoffsetDirection_003E5__4 * _003Cnut_003E5__6.nutSize;
				_003CfromPosition_003E5__11 = transform2.position;
				_003Cdrill_003E5__9.Show(_003CfromPosition_003E5__11, Quaternion.LookRotation(_003CfromPosition_003E5__11 - _003CtoPosition_003E5__10));
				goto IL_03c0;
			}
			_003Cnuts_003E5__2.Clear();
			return false;
			IL_03c0:
			if (_003C_003E8__2.time < _003Cduration_003E5__8)
			{
				float num2 = Mathf.InverseLerp(0f, _003Cduration_003E5__8, _003C_003E8__2.time);
				_003Cnut_003E5__6.SetRotateIn(_003CfromPosition_003E5__11, _003CtoPosition_003E5__10, num2);
				_003Cdrill_003E5__9.transform.position = Vector3.Lerp(_003CfromPosition_003E5__11, _003CtoPosition_003E5__10, num2);
				_003Cdrill_003E5__9.SetActive(_003CscrewdriverTool_003E5__7.isPressed);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003Cnut_003E5__6.SetRotateIn(_003CfromPosition_003E5__11, _003CtoPosition_003E5__10, 1f);
			_003CscrewdriverTool_003E5__7.Hide();
			_003C_003E8__2.CS_0024_003C_003E8__locals1.screen.tutorialHand.Hide();
			GGUtil.Hide(_003Cnut_003E5__6);
			_003C_003E8__2.CS_0024_003C_003E8__locals1.talkingDialog.Hide();
			_003C_003E8__2 = null;
			_003Cnut_003E5__6 = null;
			_003CscrewdriverTool_003E5__7 = null;
			_003Cdrill_003E5__9 = null;
			_003CtoPosition_003E5__10 = default(Vector3);
			_003CfromPosition_003E5__11 = default(Vector3);
			_003Ci_003E5__5++;
			goto IL_047d;
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

	private sealed class _003C_003Ec__DisplayClass48_0
	{
		public float time;

		internal void _003CShowNutAnimations_003Eb__0(ScrewdriverTool.PressArguments _003Cp0_003E)
		{
			time += Time.deltaTime;
		}
	}

	private sealed class _003CShowNutAnimations_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		public AssembleCarScreen screen;

		private _003C_003Ec__DisplayClass48_0 _003C_003E8__1;

		private CarNutsPool _003Cnuts_003E5__2;

		private Vector3 _003CoffsetDirection_003E5__3;

		private int _003Ci_003E5__4;

		private CarNut _003Cnut_003E5__5;

		private ScrewdriverTool _003CscrewdriverTool_003E5__6;

		private DrillModel _003Cdrill_003E5__7;

		private float _003Cduration_003E5__8;

		private Vector3 _003CfromPosition_003E5__9;

		private Vector3 _003CtoPosition_003E5__10;

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
		public _003CShowNutAnimations_003Ed__48(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0277;
			}
			_003C_003E1__state = -1;
			_003Cnuts_003E5__2 = carModelSubpart.part.model.nuts;
			_003Cnuts_003E5__2.Clear();
			new List<CarNut>();
			_003CoffsetDirection_003E5__3 = carModelSubpart.subpartInfo.offset.normalized;
			_003Ci_003E5__4 = 0;
			goto IL_02fa;
			IL_0277:
			if (_003C_003E8__1.time < _003Cduration_003E5__8)
			{
				float num2 = Mathf.InverseLerp(0f, _003Cduration_003E5__8, _003C_003E8__1.time);
				_003Cnut_003E5__5.SetRotateIn(_003CfromPosition_003E5__9, _003CtoPosition_003E5__10, num2);
				_003Cdrill_003E5__7.transform.position = Vector3.Lerp(_003CfromPosition_003E5__9, _003CtoPosition_003E5__10, num2);
				_003Cdrill_003E5__7.SetActive(_003CscrewdriverTool_003E5__6.isPressed);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003Cnut_003E5__5.SetRotateIn(_003CfromPosition_003E5__9, _003CtoPosition_003E5__10, 1f);
			_003CscrewdriverTool_003E5__6.Hide();
			_003C_003E8__1 = null;
			_003Cnut_003E5__5 = null;
			_003CscrewdriverTool_003E5__6 = null;
			_003Cdrill_003E5__7 = null;
			_003CfromPosition_003E5__9 = default(Vector3);
			_003CtoPosition_003E5__10 = default(Vector3);
			_003Ci_003E5__4++;
			goto IL_02fa;
			IL_02fa:
			if (_003Ci_003E5__4 < carModelSubpart.nutTransforms.Count)
			{
				_003C_003E8__1 = new _003C_003Ec__DisplayClass48_0();
				Transform transform = carModelSubpart.nutTransforms[_003Ci_003E5__4];
				_003Cnut_003E5__5 = _003Cnuts_003E5__2.NextNut();
				Vector3 position = transform.position;
				Quaternion rotation = Quaternion.LookRotation(_003CoffsetDirection_003E5__3);
				_003Cnut_003E5__5.Init();
				_003Cnut_003E5__5.transform.position = transform.position + _003CoffsetDirection_003E5__3 * _003Cnut_003E5__5.nutSize;
				_003Cnut_003E5__5.SetRotation(rotation);
				GGUtil.Show(_003Cnut_003E5__5);
				_003CscrewdriverTool_003E5__6 = screen.screwdriverTool;
				ScrewdriverTool.InitArguments initArguments = default(ScrewdriverTool.InitArguments);
				initArguments.inputHandler = screen.inputHandler;
				_003Cdrill_003E5__7 = _003CscrewdriverTool_003E5__6.GetDrillModel(screen.scene);
				_003C_003E8__1.time = 0f;
				_003Cduration_003E5__8 = 1f;
				ref Action<ScrewdriverTool.PressArguments> onPress = ref initArguments.onPress;
				onPress = (Action<ScrewdriverTool.PressArguments>)Delegate.Combine(onPress, new Action<ScrewdriverTool.PressArguments>(_003C_003E8__1._003CShowNutAnimations_003Eb__0));
				_003CscrewdriverTool_003E5__6.Init(initArguments);
				_003CfromPosition_003E5__9 = transform.position + _003CoffsetDirection_003E5__3 * _003Cnut_003E5__5.nutSize;
				_003CtoPosition_003E5__10 = transform.position;
				_003Cdrill_003E5__7.Show(_003CfromPosition_003E5__9, Quaternion.LookRotation(_003CtoPosition_003E5__10 - _003CfromPosition_003E5__9));
				goto IL_0277;
			}
			_003Cnuts_003E5__2.Clear();
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

	private sealed class _003CInAnimationOffset_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		public float percent;

		private float _003Ctime_003E5__2;

		private float _003Cduration_003E5__3;

		private AnimationCurve _003Ccurve_003E5__4;

		private Vector3 _003CstartPosition_003E5__5;

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
		public _003CInAnimationOffset_003Ed__49(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Settings settings = carModelSubpart.settings;
				_003Ctime_003E5__2 = 0f;
				_003Cduration_003E5__3 = settings.moveDuration;
				_003Ccurve_003E5__4 = settings.moveCurve;
				_003CstartPosition_003E5__5 = carModelSubpart.localPositionAtStart + Vector3.Lerp(Vector3.zero, carModelSubpart.subpartInfo.offset, percent);
				_003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
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
				float t = _003Ccurve_003E5__4.Evaluate(time);
				Vector3 localPosition = Vector3.LerpUnclamped(_003CstartPosition_003E5__5, _003CendPosition_003E5__6, t);
				carModelSubpart.transform.localPosition = localPosition;
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

	private sealed class _003CInAnimationScaleOffset_003Ed__50 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelSubpart _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private float _003Cduration_003E5__3;

		private AnimationCurve _003Ccurve_003E5__4;

		private Vector3 _003CstartPosition_003E5__5;

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
		public _003CInAnimationScaleOffset_003Ed__50(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Settings settings = carModelSubpart.settings;
				_003Ctime_003E5__2 = 0f;
				_003Cduration_003E5__3 = settings.moveDuration;
				_003Ccurve_003E5__4 = settings.moveCurve;
				_003CstartPosition_003E5__5 = carModelSubpart.localPositionAtStart + carModelSubpart.subpartInfo.offset;
				_003CendPosition_003E5__6 = carModelSubpart.localPositionAtStart;
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
				float t = _003Ccurve_003E5__4.Evaluate(time);
				Vector3 localPosition = Vector3.LerpUnclamped(_003CstartPosition_003E5__5, _003CendPosition_003E5__6, t);
				carModelSubpart.transform.localPosition = localPosition;
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

	private sealed class _003CInAnimation_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delay;

		public CarModelSubpart _003C_003E4__this;

		private IEnumerator _003CanimationEnum_003E5__2;

		private float _003Ctime_003E5__3;

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
		public _003CInAnimation_003Ed__51(int _003C_003E1__state)
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
			CarModelSubpart carModelSubpart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (delay > 0f)
				{
					carModelSubpart.Hide();
					_003Ctime_003E5__3 = 0f;
					goto IL_0072;
				}
				goto IL_0080;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0072;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0080:
				carModelSubpart.Show();
				_003CanimationEnum_003E5__2 = null;
				if (carModelSubpart.part.partInfo.animType == CarPartInfo.AnimType.ScaleBounce)
				{
					_003CanimationEnum_003E5__2 = carModelSubpart.InAnimationScaleBounce();
				}
				else
				{
					_003CanimationEnum_003E5__2 = carModelSubpart.InAnimationScaleOffset();
				}
				break;
				IL_0072:
				if (_003Ctime_003E5__3 <= delay)
				{
					_003Ctime_003E5__3 += Time.deltaTime;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0080;
			}
			if (_003CanimationEnum_003E5__2.MoveNext())
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
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
	private Vector3 scaleAtStart;

	[SerializeField]
	private Vector3 localPositionAtStart;

	[SerializeField]
	public CarModelPart part;

	[SerializeField]
	private Transform handleTransform_;

	[SerializeField]
	private List<SubpartVariant> variants = new List<SubpartVariant>();

	[SerializeField]
	public List<VariantModification> variantModifications = new List<VariantModification>();

	[SerializeField]
	private List<Transform> nutTransforms = new List<Transform>();

	[SerializeField]
	public string defaultVariantGroupName;

	private IEnumerator activeAnimation;

	private IEnumerator openAnimation;

	[SerializeField]
	public Transform variantHandle;

	[SerializeField]
	public string variantGroupName;

	[SerializeField]
	public CarSubPartInfo subpartInfo = new CarSubPartInfo();

	private bool isOpen;

	public CarModelInfo.VariantGroup firstVariantGroup
	{
		get
		{
			for (int i = 0; i < variants.Count; i++)
			{
				CarModelInfo.VariantGroup variantGroup = variants[i].variantGroup;
				if (variantGroup != null)
				{
					return variantGroup;
				}
			}
			for (int j = 0; j < variantModifications.Count; j++)
			{
				VariantModification variantModification = variantModifications[j];
				CarModelInfo.VariantGroup variantGroup2 = part.model.modelInfo.GetVariantGroup(variantModification.groupName);
				if (variantGroup2 != null)
				{
					return variantGroup2;
				}
			}
			return null;
		}
	}

	public Vector3 buttonHandlePosition
	{
		get
		{
			if (handleTransform_ != null)
			{
				return handleTransform_.position;
			}
			return base.transform.position;
		}
	}

	public Transform handleTransform
	{
		get
		{
			if (handleTransform_ != null)
			{
				return handleTransform_;
			}
			return base.transform;
		}
	}

	public string displayName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(subpartInfo.displayName))
			{
				return subpartInfo.displayName;
			}
			return base.transform.name;
		}
	}

	private BlinkSettings blinkSettings => ScriptableObjectSingleton<CarsDB>.instance.subpartBlinkSettings;

	private Settings settings => ScriptableObjectSingleton<CarsDB>.instance.subpartInSettings;

	public bool HasNutAnimations => nutTransforms.Count > 0;

	public void RemoveAllModificationsOfVariant(int variant)
	{
		_003C_003Ec__DisplayClass7_0 _003C_003Ec__DisplayClass7_ = new _003C_003Ec__DisplayClass7_0();
		_003C_003Ec__DisplayClass7_.variant = variant;
		variantModifications.RemoveAll(_003C_003Ec__DisplayClass7_._003CRemoveAllModificationsOfVariant_003Eb__0);
	}

	public bool HasVariantForGroup(CarModelInfo.VariantGroup group)
	{
		for (int i = 0; i < variants.Count; i++)
		{
			if (variants[i].info.groupName == group.name)
			{
				return true;
			}
		}
		for (int j = 0; j < variantModifications.Count; j++)
		{
			if (variantModifications[j].groupName == group.name)
			{
				return true;
			}
		}
		return false;
	}

	public void Init(CarModelPart part)
	{
		scaleAtStart = base.transform.localScale;
		localPositionAtStart = base.transform.localPosition;
		this.part = part;
		handleTransform_ = null;
		variantHandle = null;
		variants.Clear();
		nutTransforms.Clear();
		if (subpartInfo.rotateSettings.enabled)
		{
			subpartInfo.rotateSettings.forwardDirection = base.transform.forward;
		}
		foreach (Transform item in base.transform)
		{
			string text = item.name.ToLower();
			if (text.Contains("_nut"))
			{
				nutTransforms.Add(item);
			}
			else if (text.Contains("_collider"))
			{
				GGUtil.Hide(item);
			}
			else if (text.Contains("_variant_handle"))
			{
				variantHandle = item;
			}
			else if (text.Contains("_handle"))
			{
				handleTransform_ = item;
			}
			else if (text.Contains("_variant"))
			{
				SubpartVariant subpartVariant = item.GetComponent<SubpartVariant>();
				if (subpartVariant == null)
				{
					subpartVariant = item.gameObject.AddComponent<SubpartVariant>();
				}
				subpartVariant.Init(this);
				subpartVariant.info.index = variants.Count;
				variants.Add(subpartVariant);
			}
			else if (text.Contains("_rotate"))
			{
				subpartInfo.rotateSettings.forwardDirection = item.forward;
			}
		}
	}

	public IEnumerator InAnimationScaleBounce()
	{
		return new _003CInAnimationScaleBounce_003Ed__30(0)
		{
			_003C_003E4__this = this
		};
	}

	public void Hide()
	{
		GGUtil.SetActive(this, active: false);
	}

	public void Show(bool force = false)
	{
		bool active = !subpartInfo.removing | force;
		GGUtil.SetActive(this, active);
		ShowActiveVariant();
		for (int i = 0; i < variantModifications.Count; i++)
		{
			VariantModification variantModification = variantModifications[i];
			CarModelInfo.VariantGroup variantGroup = part.model.modelInfo.GetVariantGroup(variantModification.groupName);
			int index = 0;
			if (variantGroup != null)
			{
				index = variantGroup.selectedVariationIndex;
			}
			if (variantModification.IsApplicable(index))
			{
				variantModification.Apply(useSharedMaterial: false);
			}
		}
	}

	public void ApplyVariantModification(int selectedVariationIndex, bool useSharedMaterial)
	{
		for (int i = 0; i < variantModifications.Count; i++)
		{
			VariantModification variantModification = variantModifications[i];
			if (variantModification.IsApplicable(selectedVariationIndex))
			{
				variantModification.Apply(useSharedMaterial);
			}
		}
	}

	private void ShowActiveVariant()
	{
		for (int i = 0; i < variants.Count; i++)
		{
			variants[i].ShowIfInActiveVariant();
		}
	}

	public void SetOffsetPosition()
	{
		base.transform.localPosition = localPositionAtStart + subpartInfo.offset;
	}

	public void SetOffsetPosition(float p)
	{
		base.transform.localPosition = Vector3.Lerp(localPositionAtStart, localPositionAtStart + subpartInfo.offset, p);
	}

	public void SetExplodeOffset(float nTime, float distance)
	{
		float magnitude = subpartInfo.offset.magnitude;
		Vector3 a = Vector3.up;
		if (magnitude > Mathf.Epsilon)
		{
			a = subpartInfo.offset / magnitude;
		}
		base.transform.localPosition = Vector3.Lerp(localPositionAtStart, localPositionAtStart + a * distance, nTime);
	}

	public static void ShowChange(List<CarModelSubpart> subparts, float scale = 1f)
	{
		for (int i = 0; i < subparts.Count; i++)
		{
			subparts[i].ShowChange(scale);
		}
	}

	public void ChangeRotation()
	{
		openAnimation = DoChangeRotation();
		openAnimation.MoveNext();
	}

	private IEnumerator DoChangeRotation()
	{
		return new _003CDoChangeRotation_003Ed__41(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ShowChange(float scale)
	{
		activeAnimation = DoShowChange(scale);
		activeAnimation.MoveNext();
	}

	public IEnumerator DoShowChange(float scaleMult)
	{
		return new _003CDoShowChange_003Ed__45(0)
		{
			_003C_003E4__this = this,
			scaleMult = scaleMult
		};
	}

	public IEnumerator ShowNutAnimations()
	{
		return new _003CShowNutAnimations_003Ed__46(0)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerator ShowRemoveNutAnimations(AssembleCarScreen screen)
	{
		return new _003CShowRemoveNutAnimations_003Ed__47(0)
		{
			_003C_003E4__this = this,
			screen = screen
		};
	}

	public IEnumerator ShowNutAnimations(AssembleCarScreen screen)
	{
		return new _003CShowNutAnimations_003Ed__48(0)
		{
			_003C_003E4__this = this,
			screen = screen
		};
	}

	public IEnumerator InAnimationOffset(float percent)
	{
		return new _003CInAnimationOffset_003Ed__49(0)
		{
			_003C_003E4__this = this,
			percent = percent
		};
	}

	public IEnumerator InAnimationScaleOffset()
	{
		return new _003CInAnimationScaleOffset_003Ed__50(0)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerator InAnimation(float delay = 0f)
	{
		return new _003CInAnimation_003Ed__51(0)
		{
			_003C_003E4__this = this,
			delay = delay
		};
	}

	private void Update()
	{
		if (activeAnimation != null)
		{
			activeAnimation.MoveNext();
		}
		if (openAnimation != null)
		{
			openAnimation.MoveNext();
		}
	}
}
