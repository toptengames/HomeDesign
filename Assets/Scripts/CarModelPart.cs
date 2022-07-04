using GGMatch3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarModelPart : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public bool isDone;

		public AssembleCarScreen screen;

		public CarModelSubpart subpart;

		internal void _003CRemoveSubpart_003Eb__0(CarConfirmPurchase.InitArguments _003Cp0_003E)
		{
			isDone = true;
		}

		internal void _003CRemoveSubpart_003Eb__1()
		{
			isDone = true;
		}

		internal void _003CRemoveSubpart_003Eb__2()
		{
			float num = Mathf.Max(0.1f, screen.confirmPurchase.DistancePercent());
			subpart.SetOffsetPosition(1f - num);
		}
	}

	private sealed class _003CRemoveSubpart_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen screen;

		public CarModelSubpart subpart;

		private _003C_003Ec__DisplayClass21_0 _003C_003E8__1;

		private IEnumerator _003CenumAnimation_003E5__2;

		private TalkingDialog _003CtalkingDialog_003E5__3;

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
		public _003CRemoveSubpart_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			NavigationManager instance;
			List<string> toSayBeforeWork;
			CarConfirmPurchase.InitArguments initArguments;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass21_0();
				_003C_003E8__1.screen = screen;
				_003C_003E8__1.subpart = subpart;
				_003CenumAnimation_003E5__2 = _003C_003E8__1.subpart.ShowRemoveNutAnimations(_003C_003E8__1.screen);
				goto IL_0089;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0089;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0089:
				if (_003CenumAnimation_003E5__2.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				instance = NavigationManager.instance;
				_003CtalkingDialog_003E5__3 = instance.GetObject<TalkingDialog>();
				toSayBeforeWork = _003C_003E8__1.subpart.subpartInfo.toSayBeforeWork;
				if (toSayBeforeWork.Count > 0)
				{
					_003CtalkingDialog_003E5__3.ShowSingleLine(toSayBeforeWork[0]);
				}
				_003C_003E8__1.isDone = false;
				initArguments = default(CarConfirmPurchase.InitArguments);
				initArguments.screen = _003C_003E8__1.screen;
				initArguments.displayName = _003C_003E8__1.subpart.displayName;
				initArguments.carPart = null;
				initArguments.updateDirection = true;
				initArguments.useDistanceToFindIfInside = true;
				initArguments.buttonHandlePosition = _003C_003E8__1.subpart.buttonHandlePosition;
				initArguments.directionHandlePosition = _003C_003E8__1.subpart.handleTransform.TransformPoint(_003C_003E8__1.subpart.subpartInfo.offset);
				initArguments.directionHandlePosition = _003C_003E8__1.subpart.buttonHandlePosition;
				initArguments.buttonHandlePosition = _003C_003E8__1.subpart.handleTransform.TransformPoint(_003C_003E8__1.subpart.subpartInfo.offset);
				initArguments.onSuccess = _003C_003E8__1._003CRemoveSubpart_003Eb__0;
				initArguments.onCancel = _003C_003E8__1._003CRemoveSubpart_003Eb__1;
				if (_003C_003E8__1.subpart.subpartInfo.directControl)
				{
					initArguments.useMinDistanceToConfirm = true;
					initArguments.minDistance = 0.1f;
					initArguments.directionHandlePosition = _003C_003E8__1.subpart.handleTransform.TransformPoint(_003C_003E8__1.subpart.subpartInfo.offset);
					initArguments.onDrag = _003C_003E8__1._003CRemoveSubpart_003Eb__2;
				}
				_003C_003E8__1.screen.confirmPurchase.Show(initArguments);
				break;
			}
			if (!_003C_003E8__1.isDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			_003CtalkingDialog_003E5__3.Hide();
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

	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public AssembleCarScreen screen;

		public CarModelPart _003C_003E4__this;

		public CarModelInfo.VariantGroup groupToShow;

		internal void _003CAnimateIn_003Eb__3(CarVariationPanel _003Cp0_003E)
		{
			CarModelSubpart.ShowChange(_003C_003E4__this.model.AllOwnedSubpartsInVariantGroup(groupToShow));
		}
	}

	private sealed class _003C_003Ec__DisplayClass23_1
	{
		public float animationProgressPercent;

		public CarModelSubpart item;

		public _003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals1;
	}

	private sealed class _003C_003Ec__DisplayClass23_2
	{
		public bool isDone;

		public TalkingDialog talkingDialog;

		public _003C_003Ec__DisplayClass23_1 CS_0024_003C_003E8__locals2;

		internal void _003CAnimateIn_003Eb__0(CarConfirmPurchase.InitArguments _003Cp0_003E)
		{
			isDone = true;
		}

		internal void _003CAnimateIn_003Eb__1()
		{
			isDone = true;
		}

		internal void _003CAnimateIn_003Eb__2()
		{
			float num = Mathf.Max(0.1f, CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen.confirmPurchase.DistancePercent());
			CS_0024_003C_003E8__locals2.animationProgressPercent = num;
			CS_0024_003C_003E8__locals2.item.SetOffsetPosition(num);
			if (CS_0024_003C_003E8__locals2.item.subpartInfo.hideToSayWhenWorking)
			{
				talkingDialog.Hide();
			}
		}
	}

	private sealed class _003C_003Ec__DisplayClass23_3
	{
		public bool isVariantChosen;

		public _003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals3;

		internal void _003CAnimateIn_003Eb__4(CarVariationPanel _003Cp0_003E)
		{
			CarModelSubpart.ShowChange(CS_0024_003C_003E8__locals3._003C_003E4__this.model.AllOwnedSubpartsInVariantGroup(CS_0024_003C_003E8__locals3.groupToShow));
			isVariantChosen = true;
		}
	}

	private sealed class _003CAnimateIn_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen screen;

		public CarModelPart _003C_003E4__this;

		private _003C_003Ec__DisplayClass23_0 _003C_003E8__1;

		private _003C_003Ec__DisplayClass23_1 _003C_003E8__2;

		private _003C_003Ec__DisplayClass23_2 _003C_003E8__3;

		private _003C_003Ec__DisplayClass23_3 _003C_003E8__4;

		private EnumeratorsList _003CenumList_003E5__2;

		private int _003Ci_003E5__3;

		private bool _003CskipAnimation_003E5__4;

		private IEnumerator _003CremoveEnum_003E5__5;

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
		public _003CAnimateIn_003Ed__23(int _003C_003E1__state)
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
			CarModelPart carModelPart = _003C_003E4__this;
			CarVariationPanel.InitParams initParams;
			CarCamera.Settings carCamera2;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass23_0();
				_003C_003E8__1.screen = screen;
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003CenumList_003E5__2 = new EnumeratorsList();
				for (int i = 0; i < carModelPart.subparts.Count; i++)
				{
					carModelPart.subparts[i].Hide();
				}
				if (carModelPart.partInfo.confirmEachSubpart)
				{
					for (int j = 0; j < carModelPart.subparts.Count; j++)
					{
						CarModelSubpart carModelSubpart = carModelPart.subparts[j];
						if (carModelSubpart.subpartInfo.removing)
						{
							carModelSubpart.Show(force: true);
						}
					}
					_003Ci_003E5__3 = 0;
					goto IL_067f;
				}
				_003CenumList_003E5__2.Clear();
				for (int k = 0; k < carModelPart.subparts.Count; k++)
				{
					CarModelSubpart carModelSubpart2 = carModelPart.subparts[k];
					_003CenumList_003E5__2.Add(carModelSubpart2.InAnimation((float)k * carModelPart.partInfo.delaySubpartAnimation));
				}
				goto IL_0712;
			case 1:
				_003C_003E1__state = -1;
				goto IL_01f7;
			case 2:
				_003C_003E1__state = -1;
				goto IL_04b3;
			case 3:
				_003C_003E1__state = -1;
				goto IL_056f;
			case 4:
				_003C_003E1__state = -1;
				goto IL_05e1;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0659;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0712;
			case 7:
				_003C_003E1__state = -1;
				goto IL_079e;
			case 8:
				{
					_003C_003E1__state = -1;
					goto IL_0954;
				}
				IL_0659:
				if (_003CenumList_003E5__2.Update())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_0666;
				IL_0666:
				_003C_003E8__2 = null;
				goto IL_066d;
				IL_04b3:
				if (!_003C_003E8__3.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E8__3.talkingDialog.Hide();
				_003C_003E8__3 = null;
				_003C_003E8__2.item.Show();
				_003CenumList_003E5__2.Clear();
				if (_003CskipAnimation_003E5__4)
				{
					_003CenumList_003E5__2.Add(_003C_003E8__2.item.InAnimationOffset(_003C_003E8__2.animationProgressPercent));
				}
				else
				{
					_003CenumList_003E5__2.Add(_003C_003E8__2.item.InAnimation());
				}
				goto IL_056f;
				IL_079e:
				if (_003CenumList_003E5__2.Update())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 7;
					return true;
				}
				goto IL_07ab;
				IL_067f:
				if (_003Ci_003E5__3 < carModelPart.subparts.Count)
				{
					_003C_003E8__2 = new _003C_003Ec__DisplayClass23_1();
					_003C_003E8__2.CS_0024_003C_003E8__locals1 = _003C_003E8__1;
					_003C_003E8__2.item = carModelPart.subparts[_003Ci_003E5__3];
					_003CskipAnimation_003E5__4 = false;
					_003C_003E8__2.animationProgressPercent = 0f;
					CarCamera.Settings carCamera = _003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene.camera.GetCarCamera(_003C_003E8__2.item.subpartInfo.cameraName);
					if (carCamera != null)
					{
						_003C_003E8__2.CS_0024_003C_003E8__locals1.screen.scene.camera.AnimateIntoSettings(carCamera);
					}
					if (_003C_003E8__2.item.subpartInfo.removing)
					{
						_003CremoveEnum_003E5__5 = carModelPart.RemoveSubpart(_003C_003E8__2.item, _003C_003E8__2.CS_0024_003C_003E8__locals1.screen);
						goto IL_01f7;
					}
					_003C_003E8__3 = new _003C_003Ec__DisplayClass23_2();
					_003C_003E8__3.CS_0024_003C_003E8__locals2 = _003C_003E8__2;
					NavigationManager instance = NavigationManager.instance;
					_003C_003E8__3.talkingDialog = instance.GetObject<TalkingDialog>();
					List<string> toSayBefore = _003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.toSayBefore;
					if (toSayBefore.Count > 0)
					{
						_003C_003E8__3.talkingDialog.ShowSingleLine(toSayBefore[0]);
					}
					_003C_003E8__3.CS_0024_003C_003E8__locals2.item.SetOffsetPosition(0f);
					_003C_003E8__3.isDone = false;
					CarConfirmPurchase.InitArguments initArguments = default(CarConfirmPurchase.InitArguments);
					initArguments.screen = _003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen;
					initArguments.buttonHandlePosition = _003C_003E8__3.CS_0024_003C_003E8__locals2.item.buttonHandlePosition;
					initArguments.displayName = _003C_003E8__3.CS_0024_003C_003E8__locals2.item.displayName;
					initArguments.carPart = null;
					initArguments.updateDirection = true;
					initArguments.useDistanceToFindIfInside = true;
					initArguments.directionHandlePosition = _003C_003E8__3.CS_0024_003C_003E8__locals2.item.handleTransform.TransformPoint(_003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.offset);
					initArguments.onSuccess = _003C_003E8__3._003CAnimateIn_003Eb__0;
					initArguments.onCancel = _003C_003E8__3._003CAnimateIn_003Eb__1;
					if (_003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.showAtStart)
					{
						_003C_003E8__3.CS_0024_003C_003E8__locals2.item.Show(force: true);
						_003C_003E8__3.CS_0024_003C_003E8__locals2.item.SetOffsetPosition();
						GGUtil.SetActive(_003C_003E8__3.CS_0024_003C_003E8__locals2.item, active: true);
					}
					if (_003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.directControl)
					{
						initArguments.useMinDistanceToConfirm = true;
						initArguments.minDistance = 0.1f;
						initArguments.directionHandlePosition = _003C_003E8__3.CS_0024_003C_003E8__locals2.item.handleTransform.TransformPoint(_003C_003E8__3.CS_0024_003C_003E8__locals2.item.subpartInfo.offset);
						initArguments.onDrag = _003C_003E8__3._003CAnimateIn_003Eb__2;
						_003CskipAnimation_003E5__4 = true;
					}
					_003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.screen.confirmPurchase.Show(initArguments);
					goto IL_04b3;
				}
				goto IL_07d3;
				IL_0712:
				if (_003CenumList_003E5__2.Update())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 6;
					return true;
				}
				_003Ci_003E5__3 = 0;
				goto IL_07bd;
				IL_0954:
				if (!_003C_003E8__4.isVariantChosen)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 8;
					return true;
				}
				_003C_003E8__4 = null;
				break;
				IL_056f:
				if (_003CenumList_003E5__2.Update())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				if (_003C_003E8__2.item.subpartInfo.showChangeAnimAfterIn)
				{
					_003CenumList_003E5__2.Clear();
					_003CenumList_003E5__2.Add(_003C_003E8__2.item.DoShowChange(1f));
					goto IL_05e1;
				}
				goto IL_05ee;
				IL_07d3:
				_003C_003E8__1.groupToShow = null;
				_003C_003E8__1.groupToShow = carModelPart.model.modelInfo.GetVariantGroup(carModelPart.partInfo.variantGroupToShowAfterPurchase);
				if (_003C_003E8__1.groupToShow == null)
				{
					break;
				}
				_003C_003E8__4 = new _003C_003Ec__DisplayClass23_3();
				_003C_003E8__4.CS_0024_003C_003E8__locals3 = _003C_003E8__1;
				_003C_003E8__4.isVariantChosen = false;
				initParams = default(CarVariationPanel.InitParams);
				initParams.screen = _003C_003E8__4.CS_0024_003C_003E8__locals3.screen;
				initParams.variantGroup = _003C_003E8__4.CS_0024_003C_003E8__locals3.groupToShow;
				initParams.inputHandler = _003C_003E8__4.CS_0024_003C_003E8__locals3.screen.inputHandler;
				initParams.onChange = _003C_003E8__4.CS_0024_003C_003E8__locals3._003CAnimateIn_003Eb__3;
				initParams.onClosed = _003C_003E8__4._003CAnimateIn_003Eb__4;
				carCamera2 = _003C_003E8__4.CS_0024_003C_003E8__locals3.screen.scene.camera.GetCarCamera(_003C_003E8__4.CS_0024_003C_003E8__locals3.groupToShow.cameraName);
				if (carCamera2 != null)
				{
					_003C_003E8__4.CS_0024_003C_003E8__locals3.screen.scene.camera.AnimateIntoSettings(carCamera2);
				}
				_003C_003E8__4.CS_0024_003C_003E8__locals3.screen.variationPanel.Show(initParams);
				goto IL_0954;
				IL_01f7:
				if (_003CremoveEnum_003E5__5.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E8__2.item.Show();
				goto IL_066d;
				IL_066d:
				_003Ci_003E5__3++;
				goto IL_067f;
				IL_07bd:
				if (_003Ci_003E5__3 < carModelPart.subparts.Count)
				{
					_003CenumList_003E5__2.Clear();
					CarModelSubpart carModelSubpart3 = carModelPart.subparts[_003Ci_003E5__3];
					if (carModelSubpart3.HasNutAnimations)
					{
						_003CenumList_003E5__2.Clear();
						_003CenumList_003E5__2.Add(carModelSubpart3.ShowNutAnimations(_003C_003E8__1.screen));
						goto IL_079e;
					}
					goto IL_07ab;
				}
				goto IL_07d3;
				IL_05e1:
				if (_003CenumList_003E5__2.Update())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_05ee;
				IL_05ee:
				if (_003C_003E8__2.item.HasNutAnimations)
				{
					_003CenumList_003E5__2.Clear();
					_003CenumList_003E5__2.Add(_003C_003E8__2.item.ShowNutAnimations(_003C_003E8__2.CS_0024_003C_003E8__locals1.screen));
					goto IL_0659;
				}
				goto IL_0666;
				IL_07ab:
				_003Ci_003E5__3++;
				goto IL_07bd;
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
	public List<CarModelSubpart> subparts = new List<CarModelSubpart>();

	[SerializeField]
	public CarModel model;

	[SerializeField]
	public CarPartInfo partInfo = new CarPartInfo();

	[SerializeField]
	private Transform buttonHandleTransform;

	[SerializeField]
	public List<PaintTransformation> paintTransformations = new List<PaintTransformation>();

	[SerializeField]
	public Transform colliderRoot;

	private List<CarModelSubpart> subpartsWithVariantHandles_ = new List<CarModelSubpart>();

	public Vector3 buttonHandlePosition
	{
		get
		{
			if (buttonHandleTransform != null)
			{
				return buttonHandleTransform.position;
			}
			return base.transform.position;
		}
	}

	public Vector3 directionHandlePosition
	{
		get
		{
			if (subparts.Count == 0)
			{
				return buttonHandlePosition;
			}
			return buttonHandlePosition + subparts[0].subpartInfo.offset;
		}
	}

	public bool shouldShow
	{
		get
		{
			if (!partInfo.isOwned)
			{
				return false;
			}
			for (int i = 0; i < partInfo.hideWhenAnyActive.Count; i++)
			{
				if (partInfo.hideWhenAnyActive[i].partInfo.isOwned)
				{
					return false;
				}
			}
			return true;
		}
	}

	public List<CarModelSubpart> subpartsWithVariantHandles
	{
		get
		{
			subpartsWithVariantHandles_.Clear();
			for (int i = 0; i < subparts.Count; i++)
			{
				CarModelSubpart carModelSubpart = subparts[i];
				if (!(carModelSubpart.variantHandle == null))
				{
					subpartsWithVariantHandles_.Add(carModelSubpart);
				}
			}
			return subpartsWithVariantHandles_;
		}
	}

	public List<CarModelSubpart> subpartsWithInteraction
	{
		get
		{
			subpartsWithVariantHandles_.Clear();
			for (int i = 0; i < subparts.Count; i++)
			{
				CarModelSubpart carModelSubpart = subparts[i];
				if (carModelSubpart.subpartInfo.rotateSettings.enabled)
				{
					subpartsWithVariantHandles_.Add(carModelSubpart);
				}
			}
			return subpartsWithVariantHandles_;
		}
	}

	public CarModelInfo.VariantGroup firstVariantGroup
	{
		get
		{
			for (int i = 0; i < subparts.Count; i++)
			{
				CarModelInfo.VariantGroup firstVariantGroup = subparts[i].firstVariantGroup;
				if (firstVariantGroup != null)
				{
					return firstVariantGroup;
				}
			}
			return null;
		}
	}

	public void SetExplodeOffset(float nTime)
	{
		bool active = nTime <= 0f;
		GGUtil.SetActive(colliderRoot, active);
		float distanceFromCenter = ScriptableObjectSingleton<CarsDB>.instance.explosionSettings.distanceFromCenter;
		for (int i = 0; i < subparts.Count; i++)
		{
			subparts[i].SetExplodeOffset(nTime, distanceFromCenter);
		}
	}

	public void Init(CarModel model)
	{
		colliderRoot = null;
		this.model = model;
		partInfo.name = base.name;
		paintTransformations.Clear();
		buttonHandleTransform = null;
		subparts.Clear();
		foreach (Transform item in base.transform)
		{
			string text = item.name.ToLower();
			if (text.Contains("_collider"))
			{
				GGUtil.SetActive(item, active: false);
			}
			else if (text.Contains("_handle"))
			{
				buttonHandleTransform = item;
			}
			else if (!text.Contains("_ignore"))
			{
				PaintTransformation component = item.GetComponent<PaintTransformation>();
				if (component != null)
				{
					paintTransformations.Add(component);
					component.Init();
					GGUtil.Hide(component);
				}
				else
				{
					CarModelSubpart carModelSubpart = item.GetComponent<CarModelSubpart>();
					if (carModelSubpart == null)
					{
						carModelSubpart = item.gameObject.AddComponent<CarModelSubpart>();
					}
					carModelSubpart.Init(this);
					subparts.Add(carModelSubpart);
				}
			}
		}
	}

	private IEnumerator RemoveSubpart(CarModelSubpart subpart, AssembleCarScreen screen)
	{
		return new _003CRemoveSubpart_003Ed__21(0)
		{
			subpart = subpart,
			screen = screen
		};
	}

	public void ShowSubpartsIfRemoving()
	{
		for (int i = 0; i < subparts.Count; i++)
		{
			CarModelSubpart carModelSubpart = subparts[i];
			if (carModelSubpart.subpartInfo.removing)
			{
				carModelSubpart.Show(force: true);
			}
		}
	}

	public IEnumerator AnimateIn(AssembleCarScreen screen)
	{
		return new _003CAnimateIn_003Ed__23(0)
		{
			_003C_003E4__this = this,
			screen = screen
		};
	}

	public void InitForRuntime(RoomsBackend.RoomAccessor backend)
	{
		partInfo.InitForRuntime(model, backend);
	}

	public void HideSubparts()
	{
		for (int i = 0; i < subparts.Count; i++)
		{
			GGUtil.Hide(subparts[i]);
		}
	}

	public void SetActiveIfOwned()
	{
		GGUtil.SetActive(this, shouldShow);
		for (int i = 0; i < subparts.Count; i++)
		{
			subparts[i].Show();
		}
		GGUtil.SetActive(colliderRoot, shouldShow);
	}
}
