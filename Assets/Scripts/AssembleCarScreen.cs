using GGMatch3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssembleCarScreen : MonoBehaviour, Match3GameListener
{
	public class LoadedAsset
	{
		public string name;

		public CarsDB.Car loadedCar;

		public Scene scene;

		public CarScene sceneBehaviour;

		public LoadedAsset(string name, CarScene sceneBehaviour)
		{
			this.name = name;
			this.sceneBehaviour = sceneBehaviour;
		}
	}

	private sealed class _003CDoLoadScene_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen _003C_003E4__this;

		public CarsDB.Car car;

		private CarsDB.LoadCarRequest _003CroomRequest_003E5__2;

		private IEnumerator _003CupdateEnum_003E5__3;

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
		public _003CDoLoadScene_003Ed__40(int _003C_003E1__state)
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
			AssembleCarScreen assembleCarScreen = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				GGUtil.SetActive(assembleCarScreen.widgetsToHide, active: false);
				assembleCarScreen.loadingSceneStyle.Apply();
				_003CroomRequest_003E5__2 = new CarsDB.LoadCarRequest(car);
				CarsDB instance = ScriptableObjectSingleton<CarsDB>.instance;
				_003CupdateEnum_003E5__3 = instance.Load(_003CroomRequest_003E5__2);
				GGUtil.SetFill(assembleCarScreen.progressBarSprite, 0f);
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CupdateEnum_003E5__3.MoveNext())
			{
				float progress = _003CroomRequest_003E5__2.progress;
				GGUtil.SetFill(assembleCarScreen.progressBarSprite, progress);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (car.rootTransform == null)
			{
				GGUtil.SetActive(assembleCarScreen.widgetsToHide, active: false);
				assembleCarScreen.retryLoadingStyle.Apply();
				return false;
			}
			GGUtil.SetActive(assembleCarScreen.widgetsToHide, active: false);
			assembleCarScreen.roomLoadedStyle.Apply();
			CarScene sceneBehaviour = car.sceneBehaviour;
			assembleCarScreen.activeRoom = new LoadedAsset(car.name, car.sceneBehaviour);
			assembleCarScreen.activeRoom.loadedCar = car;
			GGUtil.SetActive(sceneBehaviour, active: true);
			sceneBehaviour.carModel.InitForRuntime();
			sceneBehaviour.camera.Init(assembleCarScreen.inputHandler);
			GGUtil.SetActive(assembleCarScreen.widgetsToHide, active: false);
			assembleCarScreen.roomLoadedStyle.Apply();
			assembleCarScreen.InitScene(car.sceneBehaviour, isFirstTime: true);
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

	private sealed class _003C_003Ec__DisplayClass52_0
	{
		public bool isSprayingDone;

		internal void _003CShowCarSpray_003Eb__0()
		{
			isSprayingDone = true;
		}
	}

	private sealed class _003CShowCarSpray_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarModelPart part;

		public AssembleCarScreen _003C_003E4__this;

		private _003C_003Ec__DisplayClass52_0 _003C_003E8__1;

		private List<string> _003CtoSayBeforeOpen_003E5__2;

		private int _003Ci_003E5__3;

		private PaintTransformation _003CpaintTransformation_003E5__4;

		private IEnumerator _003CbeforeDialog_003E5__5;

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
		public _003CShowCarSpray_003Ed__52(int _003C_003E1__state)
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
			AssembleCarScreen assembleCarScreen = _003C_003E4__this;
			CarSprayTool.InitArguments initArguments;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				part.HideSubparts();
				for (int i = 0; i < part.paintTransformations.Count; i++)
				{
					GGUtil.Hide(part.paintTransformations[i]);
				}
				_003CtoSayBeforeOpen_003E5__2 = part.partInfo.toSayBefore;
				_003Ci_003E5__3 = 0;
				goto IL_020c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0144;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_01be;
				}
				IL_01be:
				if (!_003C_003E8__1.isSprayingDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				GGUtil.Hide(_003CpaintTransformation_003E5__4);
				_003CpaintTransformation_003E5__4.ReleaseAll();
				GGUtil.Hide(assembleCarScreen.sprayTool);
				_003C_003E8__1 = null;
				_003CpaintTransformation_003E5__4 = null;
				_003Ci_003E5__3++;
				goto IL_020c;
				IL_0158:
				_003C_003E8__1.isSprayingDone = false;
				initArguments = default(CarSprayTool.InitArguments);
				initArguments.screen = assembleCarScreen;
				initArguments.paintTransformation = _003CpaintTransformation_003E5__4;
				initArguments.onDone = _003C_003E8__1._003CShowCarSpray_003Eb__0;
				assembleCarScreen.sprayTool.Init(initArguments);
				goto IL_01be;
				IL_020c:
				if (_003Ci_003E5__3 < part.paintTransformations.Count)
				{
					_003C_003E8__1 = new _003C_003Ec__DisplayClass52_0();
					_003CpaintTransformation_003E5__4 = part.paintTransformations[_003Ci_003E5__3];
					GGUtil.Show(_003CpaintTransformation_003E5__4);
					_003CpaintTransformation_003E5__4.Init();
					_003CpaintTransformation_003E5__4.ClearTexturesToColor(Color.black);
					if (_003Ci_003E5__3 == 0 && _003CtoSayBeforeOpen_003E5__2.Count > 0)
					{
						TalkingDialog @object = NavigationManager.instance.GetObject<TalkingDialog>();
						TalkingDialog.ShowArguments showArguments = new TalkingDialog.ShowArguments();
						showArguments.inputHandler = assembleCarScreen.inputHandler;
						showArguments.thingsToSay.AddRange(_003CtoSayBeforeOpen_003E5__2);
						_003CbeforeDialog_003E5__5 = @object.DoShow(showArguments);
						goto IL_0144;
					}
					goto IL_0158;
				}
				for (int j = 0; j < part.paintTransformations.Count; j++)
				{
					PaintTransformation paintTransformation = part.paintTransformations[j];
					GGUtil.Hide(paintTransformation);
					paintTransformation.ReleaseAll();
				}
				part.SetActiveIfOwned();
				return false;
				IL_0144:
				if (_003CbeforeDialog_003E5__5.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003CbeforeDialog_003E5__5 = null;
				goto IL_0158;
			}
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

	private sealed class _003CShowNewPart_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen _003C_003E4__this;

		public CarModelPart part;

		private bool _003CisRoomComplete_003E5__2;

		private IEnumerator _003CenumeratorToAnimate_003E5__3;

		private IEnumerator _003CslideBackEnum_003E5__4;

		private IEnumerator _003CbeforeDialog_003E5__5;

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
		public _003CShowNewPart_003Ed__53(int _003C_003E1__state)
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
			AssembleCarScreen assembleCarScreen = _003C_003E4__this;
			List<string> toSayBefore;
			bool isPassed;
			ProgressMessageList obj;
			CarModel.ProgressState progressState;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				GGUtil.Hide(assembleCarScreen.controlWidgets);
				assembleCarScreen.slider.StopSlider();
				if (assembleCarScreen.slider.isExploded)
				{
					_003CslideBackEnum_003E5__4 = assembleCarScreen.slider.Unexplode();
					goto IL_0082;
				}
				goto IL_0096;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0082;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0100;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0196;
			case 4:
				_003C_003E1__state = -1;
				goto IL_01d5;
			case 5:
				{
					_003C_003E1__state = -1;
					goto IL_0358;
				}
				IL_0082:
				if (_003CslideBackEnum_003E5__4.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003CslideBackEnum_003E5__4 = null;
				goto IL_0096;
				IL_01aa:
				_003CslideBackEnum_003E5__4 = part.AnimateIn(assembleCarScreen);
				goto IL_01d5;
				IL_0196:
				if (_003CbeforeDialog_003E5__5.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003CbeforeDialog_003E5__5 = null;
				goto IL_01aa;
				IL_0096:
				part.model.RefreshVisibilityOnParts();
				part.SetActiveIfOwned();
				part.partInfo.selectedVariation = 0;
				if (part.paintTransformations.Count > 0)
				{
					_003CslideBackEnum_003E5__4 = assembleCarScreen.ShowCarSpray(part);
					goto IL_0100;
				}
				part.ShowSubpartsIfRemoving();
				toSayBefore = part.partInfo.toSayBefore;
				if (toSayBefore.Count > 0)
				{
					TalkingDialog @object = NavigationManager.instance.GetObject<TalkingDialog>();
					TalkingDialog.ShowArguments showArguments = new TalkingDialog.ShowArguments();
					showArguments.inputHandler = assembleCarScreen.inputHandler;
					showArguments.thingsToSay.AddRange(toSayBefore);
					_003CbeforeDialog_003E5__5 = @object.DoShow(showArguments);
					goto IL_0196;
				}
				goto IL_01aa;
				IL_01e9:
				assembleCarScreen.toSayAfterOpen.Clear();
				if (part.partInfo.hasSomethingToSay)
				{
					assembleCarScreen.toSayAfterOpen.Add(part.partInfo.thingToSay);
				}
				if (assembleCarScreen.toSayAfterOpen.Count == 0)
				{
					assembleCarScreen.toSayAfterOpen.Add("Great job!");
				}
				isPassed = assembleCarScreen.scene.carModel.isPassed;
				obj = new ProgressMessageList
				{
					messageList = assembleCarScreen.toSayAfterOpen
				};
				progressState = assembleCarScreen.scene.carModel.GetProgressState();
				_003CisRoomComplete_003E5__2 = false;
				if (progressState.isPassed && !isPassed)
				{
					assembleCarScreen.scene.carModel.isPassed = true;
					_003CisRoomComplete_003E5__2 = true;
				}
				obj.progressChange = new ProgressMessageList.ProgressChange();
				obj.progressChange.fromProgress = progressState.Progress(1);
				obj.progressChange.toProgress = progressState.progress;
				_003CenumeratorToAnimate_003E5__3 = null;
				if (assembleCarScreen.toSayAfterOpen.Count > 0)
				{
					assembleCarScreen.ShowConfettiParticle();
					TalkingDialog object2 = NavigationManager.instance.GetObject<TalkingDialog>();
					TalkingDialog.ShowArguments showArguments2 = new TalkingDialog.ShowArguments();
					showArguments2.inputHandler = assembleCarScreen.inputHandler;
					showArguments2.thingsToSay.AddRange(assembleCarScreen.toSayAfterOpen);
					_003CenumeratorToAnimate_003E5__3 = object2.DoShow(showArguments2);
					assembleCarScreen.scene.camera.SetStandardSettings();
				}
				if (_003CenumeratorToAnimate_003E5__3 == null)
				{
					break;
				}
				goto IL_0358;
				IL_0100:
				if (_003CslideBackEnum_003E5__4.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003CslideBackEnum_003E5__4 = null;
				goto IL_01e9;
				IL_01d5:
				if (_003CslideBackEnum_003E5__4.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				_003CslideBackEnum_003E5__4 = null;
				goto IL_01e9;
				IL_0358:
				if (_003CenumeratorToAnimate_003E5__3.MoveNext())
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 5;
					return true;
				}
				assembleCarScreen.scene.carModel.ShowChnage();
				break;
			}
			if (_003CisRoomComplete_003E5__2)
			{
				assembleCarScreen.OnCompleteRoom();
			}
			else
			{
				assembleCarScreen.InitScene(assembleCarScreen.scene, isFirstTime: false);
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

	private sealed class _003C_003Ec__DisplayClass54_0
	{
		public NavigationManager nav;

		public CarsDB.Car loadedRoom;

		public GiftBoxScreen giftBoxScreen;

		public GiftBoxScreen.ShowArguments arguments;

		internal void _003COnCompleteRoom_003Eb__0()
		{
			nav.Pop(activateNextScreen: false);
			ScrollableSelectCarScreen.ChangeCarArguments changeCarArguments = new ScrollableSelectCarScreen.ChangeCarArguments();
			changeCarArguments.passedCar = loadedRoom;
			changeCarArguments.unlockedCar = ScriptableObjectSingleton<CarsDB>.instance.NextCar(loadedRoom);
			nav.GetObject<ScrollableSelectCarScreen>().Show(changeCarArguments);
		}

		internal void _003COnCompleteRoom_003Eb__1()
		{
			nav.Pop();
			giftBoxScreen.Show(arguments);
		}
	}

	private sealed class _003C_003Ec__DisplayClass56_0
	{
		public bool canContiue;

		internal void _003CDoShowMessageEnumerator_003Eb__0()
		{
			canContiue = true;
		}
	}

	private sealed class _003CDoShowMessageEnumerator_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AssembleCarScreen _003C_003E4__this;

		public TextDialog.MessageArguments messageArguments;

		private _003C_003Ec__DisplayClass56_0 _003C_003E8__1;

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
		public _003CDoShowMessageEnumerator_003Ed__56(int _003C_003E1__state)
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
			AssembleCarScreen assembleCarScreen = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass56_0();
				assembleCarScreen.visibilityHelper.Clear();
				assembleCarScreen.visibilityHelper.SaveIsVisible(assembleCarScreen.widgetsToHide);
				GGUtil.SetActive(assembleCarScreen.widgetsToHide, active: false);
				messageArguments.message = messageArguments.message.Replace("\\n", "\n");
				TextDialog @object = NavigationManager.instance.GetObject<TextDialog>();
				_003C_003E8__1.canContiue = false;
				@object.ShowOk(messageArguments, _003C_003E8__1._003CDoShowMessageEnumerator_003Eb__0);
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003C_003E8__1.canContiue)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			assembleCarScreen.visibilityHelper.Complete();
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

	private sealed class _003CDoShowMessageEnumerator_003Ed__57 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ProgressMessageList progressMessages;

		public AssembleCarScreen _003C_003E4__this;

		public Action onComplete;

		private List<string> _003Cmessages_003E5__2;

		private IEnumerator _003Cenumerator_003E5__3;

		private TextDialog.MessageArguments _003CmessageArguments_003E5__4;

		private int _003Ci_003E5__5;

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
		public _003CDoShowMessageEnumerator_003Ed__57(int _003C_003E1__state)
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
			AssembleCarScreen assembleCarScreen = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0118;
			}
			_003C_003E1__state = -1;
			_003Cmessages_003E5__2 = progressMessages.messageList;
			_003Cenumerator_003E5__3 = null;
			_003CmessageArguments_003E5__4 = default(TextDialog.MessageArguments);
			if (progressMessages.progressChange != null)
			{
				_003CmessageArguments_003E5__4.showProgress = true;
				_003CmessageArguments_003E5__4.fromProgress = progressMessages.progressChange.fromProgress;
				_003CmessageArguments_003E5__4.toProgress = progressMessages.progressChange.toProgress;
			}
			if (_003Cmessages_003E5__2 != null)
			{
				_003Ci_003E5__5 = 0;
				goto IL_0135;
			}
			goto IL_014b;
			IL_014b:
			if (onComplete != null)
			{
				onComplete();
			}
			return false;
			IL_0135:
			if (_003Ci_003E5__5 < _003Cmessages_003E5__2.Count)
			{
				string message = _003Cmessages_003E5__2[_003Ci_003E5__5];
				if (_003Ci_003E5__5 == _003Cmessages_003E5__2.Count - 1)
				{
					_003CmessageArguments_003E5__4.message = message;
					_003Cenumerator_003E5__3 = assembleCarScreen.DoShowMessageEnumerator(_003CmessageArguments_003E5__4);
				}
				else
				{
					_003Cenumerator_003E5__3 = assembleCarScreen.DoShowMessageEnumerator(message);
				}
				goto IL_0118;
			}
			goto IL_014b;
			IL_0118:
			if (_003Cenumerator_003E5__3.MoveNext())
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003Ci_003E5__5++;
			goto IL_0135;
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

	private sealed class _003C_003Ec__DisplayClass72_0
	{
		public WinScreen.InitArguments winScreenArguments;

		public WinScreen winScreen;

		internal void _003COnGameComplete_003Eb__0()
		{
			GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.coins, (int)winScreenArguments.coinsWon);
			winScreen.Show(winScreenArguments);
			GGSoundSystem.Play(GGSoundSystem.MusicType.MainMenuMusic);
		}
	}

	[SerializeField]
	private TasksButton tasksButton;

	[SerializeField]
	private CarSprayTool sprayTool;

	[SerializeField]
	public ScrewdriverTool screwdriverTool;

	[SerializeField]
	public CarConfirmPurchase confirmPurchase;

	[SerializeField]
	public InputHandler inputHandler;

	[SerializeField]
	private Image progressBarSprite;

	[SerializeField]
	private GameObject confettiParticle;

	[SerializeField]
	private TextMeshProUGUI levelCountLabel;

	[SerializeField]
	private ComponentPool visualItemsPool = new ComponentPool();

	[SerializeField]
	private ComponentPool variationInteractionItemsPool = new ComponentPool();

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private List<RectTransform> controlWidgets = new List<RectTransform>();

	[SerializeField]
	private VisualStyleSet loadingSceneStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet retryLoadingStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet roomLoadedStyle = new VisualStyleSet();

	[SerializeField]
	public RectTransform coinRect;

	[SerializeField]
	public RectTransform starRect;

	[SerializeField]
	public CurrencyPanel currencyPanel;

	[SerializeField]
	public CarVariationPanel variationPanel;

	[SerializeField]
	public TutorialHandController tutorialHand;

	[SerializeField]
	private ExplodeSlider slider;

	private VisibilityHelper visibilityHelper = new VisibilityHelper();

	private List<CarVisualItemButton> uiButtons = new List<CarVisualItemButton>();

	private List<CarVariantInteractionButton> interactionButtons = new List<CarVariantInteractionButton>();

	private IEnumerator updateEnumerator;

	private IEnumerator animationEnumerator;

	public LoadedAsset activeRoom;

	private List<string> toSayAfterOpen = new List<string>();

	private int wonCoinsCount;

	public CarScene scene
	{
		get
		{
			if (activeRoom == null)
			{
				return null;
			}
			return activeRoom.sceneBehaviour;
		}
	}

	private bool isRoomLoaded
	{
		get
		{
			if (activeRoom != null && activeRoom.sceneBehaviour != null)
			{
				return activeRoom.loadedCar == ScriptableObjectSingleton<CarsDB>.instance.Active;
			}
			return false;
		}
	}

	private void ShowConfettiParticle()
	{
		GGUtil.SetActive(UnityEngine.Object.Instantiate(confettiParticle, base.transform), active: true);
	}

	private void OnEnable()
	{
		Init();
	}

	private void OnDisable()
	{
		if (isRoomLoaded)
		{
			GGUtil.SetActive(scene, active: false);
		}
	}

	public void Init()
	{
		Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
		GGUtil.ChangeText(levelCountLabel, currentStage.index + 1);
		if (isRoomLoaded)
		{
			GGUtil.SetActive(scene, active: true);
			GGUtil.SetActive(widgetsToHide, active: false);
			roomLoadedStyle.Apply();
			InitScene(scene, isFirstTime: true);
		}
		else
		{
			CarsDB.Car active = ScriptableObjectSingleton<CarsDB>.instance.Active;
			LoadScene(active);
		}
	}

	private void StopReactingToClick()
	{
		inputHandler.onClick -= OnInputHandlerClick;
	}

	private void StartReactingToClick()
	{
		inputHandler.onClick -= OnInputHandlerClick;
		inputHandler.onClick += OnInputHandlerClick;
	}

	private void OnInputHandlerClick(Vector2 position)
	{
		if (scene == null)
		{
			return;
		}
		CarCamera camera = scene.camera;
		if (camera == null || !Physics.Raycast(camera.ScreenPointToRay(position), out RaycastHit hitInfo))
		{
			return;
		}
		PartCollider component = hitInfo.collider.gameObject.GetComponent<PartCollider>();
		if (!(component == null))
		{
			CarModelInfo.VariantGroup variantGroup = null;
			if (component.subpart != null)
			{
				variantGroup = component.subpart.firstVariantGroup;
			}
			CarModelPart part = component.part;
			if (part != null && variantGroup == null)
			{
				variantGroup = part.firstVariantGroup;
			}
			if (variantGroup != null)
			{
				OnChangeVariant(variantGroup);
			}
		}
	}

	private void LoadScene(CarsDB.Car car)
	{
		GGUtil.SetActive(widgetsToHide, active: false);
		loadingSceneStyle.Apply();
		updateEnumerator = DoLoadScene(car);
	}

	private IEnumerator DoLoadScene(CarsDB.Car car)
	{
		return new _003CDoLoadScene_003Ed__40(0)
		{
			_003C_003E4__this = this,
			car = car
		};
	}

	private void InitScene(CarScene carScene, bool isFirstTime)
	{
		GGUtil.SetActive(controlWidgets, active: true);
		StartReactingToClick();
		carScene.carModel.SetCollidersActive(active: true);
		carScene.carModel.InitExplodeAnimation();
		tasksButton.Show(carScene);
		visualItemsPool.Clear();
		uiButtons.Clear();
		interactionButtons.Clear();
		List<CarModelPart> parts = carScene.carModel.parts;
		scene.camera.SetStandardSettings();
		variationInteractionItemsPool.Clear();
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (carModelPart.partInfo.isOwned)
			{
				List<CarModelSubpart> subpartsWithInteraction = carModelPart.subpartsWithInteraction;
				for (int j = 0; j < subpartsWithInteraction.Count; j++)
				{
					CarModelSubpart carModelSubpart = subpartsWithInteraction[j];
					CarVariantInteractionButton carVariantInteractionButton = variationInteractionItemsPool.Next<CarVariantInteractionButton>(activate: true);
					CarVariantInteractionButton.InitParams initParams = default(CarVariantInteractionButton.InitParams);
					initParams.positionToAttachTo = carModelSubpart.transform.position;
					initParams.screen = this;
					initParams.variantGroup = null;
					initParams.subpart = carModelSubpart;
					initParams.forwardTransform = carModelSubpart.transform;
					initParams.forwardDirection = carModelSubpart.subpartInfo.rotateSettings.forwardDirection;
					initParams.onClick = OnRotateSubpart;
					carVariantInteractionButton.Init(initParams);
					interactionButtons.Add(carVariantInteractionButton);
				}
			}
		}
		variationInteractionItemsPool.HideNotUsed();
		for (int k = 0; k < parts.Count; k++)
		{
			parts[k].SetActiveIfOwned();
		}
		visualItemsPool.HideNotUsed();
		slider.Init(this);
		GGUtil.SetActive(slider, active: false);
	}

	private void OnChangeVariant(CarVariantInteractionButton button)
	{
		OnChangeVariant(button.variantGroup);
	}

	private void OnRotateSubpart(CarVariantInteractionButton button)
	{
		if (!(button == null) && !(button.subpart == null))
		{
			button.subpart.ChangeRotation();
		}
	}

	private void OnChangeVariant(CarModelInfo.VariantGroup variantGroup)
	{
		HideAllButtons();
		StopReactingToClick();
		scene.carModel.SetCollidersActive(active: false);
		CarVariationPanel.InitParams initParams = default(CarVariationPanel.InitParams);
		initParams.screen = this;
		initParams.variantGroup = variantGroup;
		initParams.onChange = OnVariantPanelChanged;
		initParams.onClosed = OnVariantPanelClosed;
		initParams.inputHandler = inputHandler;
		initParams.showBackground = false;
		variationPanel.Show(initParams);
		CarCamera.Settings carCamera = scene.camera.GetCarCamera(variantGroup.cameraName);
		if (carCamera != null)
		{
			scene.camera.AnimateIntoSettings(carCamera);
		}
		CarModelSubpart.ShowChange(scene.carModel.AllOwnedSubpartsInVariantGroup(variantGroup));
	}

	private void OnVariantPanelChanged(CarVariationPanel panel)
	{
		CarModelSubpart.ShowChange(scene.carModel.AllOwnedSubpartsInVariantGroup(panel.variantGroup));
	}

	private void OnVariantPanelClosed(CarVariationPanel panel)
	{
		CarModelSubpart.ShowChange(scene.carModel.AllOwnedSubpartsInVariantGroup(panel.variantGroup), 0.4f);
		InitScene(scene, isFirstTime: false);
	}

	private void HideAllButtons()
	{
		GGUtil.SetActive(slider, active: false);
		for (int i = 0; i < uiButtons.Count; i++)
		{
			uiButtons[i].HideButton();
		}
		for (int j = 0; j < interactionButtons.Count; j++)
		{
			interactionButtons[j].HideButton();
		}
	}

	public void VisualItemCallback_OnBuyItemPressed(CarVisualItemButton button)
	{
		HideAllButtons();
		StopReactingToClick();
		CarConfirmPurchase.InitArguments initArguments = default(CarConfirmPurchase.InitArguments);
		initArguments.screen = this;
		initArguments.buttonHandlePosition = button.part.buttonHandlePosition;
		initArguments.displayName = button.part.partInfo.displayName;
		initArguments.carPart = button.part;
		initArguments.onSuccess = ConfirmPurchasePanelCallback_OnConfirm;
		initArguments.onCancel = ConfirmPurchasePanelCallback_OnClosed;
		initArguments.updateDirection = true;
		initArguments.directionHandlePosition = button.part.directionHandlePosition;
		initArguments.showBackground = false;
		initArguments.inputHandler = inputHandler;
		confirmPurchase.Show(initArguments);
		CarCamera.Settings carCamera = scene.camera.GetCarCamera(button.part.partInfo.cameraName);
		if (carCamera != null)
		{
			scene.camera.AnimateIntoSettings(carCamera);
		}
	}

	public void ConfirmPurchasePanelCallback_OnConfirm(CarConfirmPurchase.InitArguments initArguments)
	{
		CarModelPart carPart = initArguments.carPart;
		SingleCurrencyPrice price = new SingleCurrencyPrice(1, CurrencyType.diamonds);
		WalletManager walletManager = GGPlayerSettings.instance.walletManager;
		if (!walletManager.CanBuyItemWithPrice(price))
		{
			ButtonCallback_PlayButtonClick();
			return;
		}
		walletManager.BuyItem(price);
		currencyPanel.SetLabels();
		carPart.partInfo.isOwned = true;
		StopReactingToClick();
		animationEnumerator = ShowNewPart(carPart);
		animationEnumerator.MoveNext();
	}

	public void ConfirmPurchasePanelCallback_OnClosed()
	{
		InitScene(scene, isFirstTime: false);
	}

	private IEnumerator ShowCarSpray(CarModelPart part)
	{
		return new _003CShowCarSpray_003Ed__52(0)
		{
			_003C_003E4__this = this,
			part = part
		};
	}

	private IEnumerator ShowNewPart(CarModelPart part)
	{
		return new _003CShowNewPart_003Ed__53(0)
		{
			_003C_003E4__this = this,
			part = part
		};
	}

	private void OnCompleteRoom()
	{
		_003C_003Ec__DisplayClass54_0 _003C_003Ec__DisplayClass54_ = new _003C_003Ec__DisplayClass54_0();
		_003C_003Ec__DisplayClass54_.nav = NavigationManager.instance;
		_003C_003Ec__DisplayClass54_.giftBoxScreen = _003C_003Ec__DisplayClass54_.nav.GetObject<GiftBoxScreen>();
		_003C_003Ec__DisplayClass54_.loadedRoom = activeRoom.loadedCar;
		_003C_003Ec__DisplayClass54_.arguments = default(GiftBoxScreen.ShowArguments);
		_003C_003Ec__DisplayClass54_.arguments.title = "Car Complete";
		_003C_003Ec__DisplayClass54_.arguments.giftsDefinition = _003C_003Ec__DisplayClass54_.loadedRoom.giftDefinition;
		_003C_003Ec__DisplayClass54_.arguments.onComplete = _003C_003Ec__DisplayClass54_._003COnCompleteRoom_003Eb__0;
		_003C_003Ec__DisplayClass54_.nav.GetObject<WellDoneScreen>().Show(new WellDoneScreen.InitArguments
		{
			mainText = "Car Complete",
			onComplete = _003C_003Ec__DisplayClass54_._003COnCompleteRoom_003Eb__1
		});
	}

	private IEnumerator DoShowMessageEnumerator(string message)
	{
		TextDialog.MessageArguments messageArguments = default(TextDialog.MessageArguments);
		messageArguments.message = message;
		return DoShowMessageEnumerator(messageArguments);
	}

	private IEnumerator DoShowMessageEnumerator(TextDialog.MessageArguments messageArguments)
	{
		return new _003CDoShowMessageEnumerator_003Ed__56(0)
		{
			_003C_003E4__this = this,
			messageArguments = messageArguments
		};
	}

	private IEnumerator DoShowMessageEnumerator(ProgressMessageList progressMessages, Action onComplete = null)
	{
		return new _003CDoShowMessageEnumerator_003Ed__57(0)
		{
			_003C_003E4__this = this,
			progressMessages = progressMessages,
			onComplete = onComplete
		};
	}

	public void ButtonCallback_Tasks()
	{
		CompletionDialog @object = NavigationManager.instance.GetObject<CompletionDialog>();
		List<CarModelPart> list = scene.carModel.AvailablePartsAsTasks();
		tasksButton.HideAnimation();
		CompletionDialog.InitArguments initArguments = new CompletionDialog.InitArguments();
		initArguments.onComplete = OnTaskSelected;
		initArguments.onCancel = OnTaskDialogClosed;
		initArguments.showModal = true;
		for (int i = 0; i < list.Count; i++)
		{
			CarModelPart carModelPart = list[i];
			CompletionDialog.InitArguments.Task item = default(CompletionDialog.InitArguments.Task);
			item.name = carModelPart.partInfo.displayName;
			item.price = 1;
			item.part = carModelPart;
			initArguments.tasks.Add(item);
		}
		@object.Show(initArguments);
	}

	private void OnTaskDialogClosed()
	{
		NavigationManager.instance.Pop();
		InitScene(scene, isFirstTime: false);
	}

	private void OnTaskSelected(CompletionDialog.InitArguments.Task task)
	{
		NavigationManager.instance.Pop();
		CarModelPart part = task.part;
		if (part == null)
		{
			return;
		}
		WalletManager walletManager = GGPlayerSettings.instance.walletManager;
		SingleCurrencyPrice price = new SingleCurrencyPrice(task.price, CurrencyType.diamonds);
		if (!walletManager.CanBuyItemWithPrice(price))
		{
			ButtonCallback_PlayButtonClick();
			return;
		}
		HideAllButtons();
		StopReactingToClick();
		CarCamera.Settings carCamera = scene.camera.GetCarCamera(part.partInfo.cameraName);
		if (carCamera != null)
		{
			scene.camera.AnimateIntoSettings(carCamera);
		}
		walletManager.BuyItem(price);
		currencyPanel.SetLabels();
		part.partInfo.isOwned = true;
		StopReactingToClick();
		animationEnumerator = ShowNewPart(part);
		animationEnumerator.MoveNext();
	}

	public void ButtonCallback_OnRetry()
	{
		Init();
	}

	public Vector3 TransformWorldCarPointToLocalUIPosition(Vector3 worldCarPoint)
	{
		Camera camera = NavigationManager.instance.GetCamera();
		if (scene == null || scene.camera == null || camera == null)
		{
			return Vector3.zero;
		}
		Vector3 position = scene.camera.WorldToScreenPoint(worldCarPoint);
		Vector3 position2 = camera.ScreenToWorldPoint(position);
		return base.transform.InverseTransformPoint(position2);
	}

	public bool IsFacingCamera(Vector3 forward)
	{
		Camera camera = NavigationManager.instance.GetCamera();
		if (scene == null || scene.camera == null || camera == null)
		{
			return true;
		}
		return Vector3.Dot(scene.camera.cameraForward, forward) < 0f;
	}

	private void Update()
	{
		if (updateEnumerator != null && !updateEnumerator.MoveNext())
		{
			updateEnumerator = null;
		}
		if (animationEnumerator != null && !animationEnumerator.MoveNext())
		{
			animationEnumerator = null;
		}
		if (!(scene != null) || animationEnumerator != null)
		{
			return;
		}
		if (Application.isEditor && UnityEngine.Input.GetKeyDown(KeyCode.N))
		{
			CarModelPart carModelPart = null;
			List<CarModelPart> parts = scene.carModel.parts;
			for (int i = 0; i < parts.Count; i++)
			{
				CarModelPart carModelPart2 = parts[i];
				if (!carModelPart2.partInfo.isOwned && (carModelPart == null || carModelPart.partInfo.groupIndex > carModelPart2.partInfo.groupIndex))
				{
					carModelPart = carModelPart2;
				}
			}
			if (carModelPart != null)
			{
				carModelPart.partInfo.isOwned = true;
			}
			InitScene(scene, isFirstTime: false);
		}
		if (!Application.isEditor || !Input.GetKeyDown(KeyCode.P))
		{
			return;
		}
		CarModelPart carModelPart3 = null;
		List<CarModelPart> parts2 = scene.carModel.parts;
		for (int j = 0; j < parts2.Count; j++)
		{
			CarModelPart carModelPart4 = parts2[j];
			if (carModelPart4.partInfo.isOwned && (carModelPart3 == null || carModelPart3.partInfo.groupIndex < carModelPart4.partInfo.groupIndex))
			{
				carModelPart3 = carModelPart4;
			}
		}
		if (carModelPart3 != null)
		{
			carModelPart3.partInfo.isOwned = false;
		}
		InitScene(scene, isFirstTime: false);
	}

	public void StartGame(Match3GameParams initParams)
	{
		if (!BehaviourSingleton<EnergyManager>.instance.HasEnergyForOneLife())
		{
			OutOfLivesDialog @object = NavigationManager.instance.GetObject<OutOfLivesDialog>();
			LivesPriceConfig.PriceConfig priceForLevelOrDefault = ScriptableObjectSingleton<LivesPriceConfig>.instance.GetPriceForLevelOrDefault(initParams.levelIndex);
			@object.Show(priceForLevelOrDefault, OnAllLifesRefilled, OnFirstLifeRefilled, Init);
			return;
		}
		GameScreen object2 = NavigationManager.instance.GetObject<GameScreen>();
		Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
		initParams.level = currentStage.levelReference.level;
		if (currentStage.multiLevelReference.Count > 0)
		{
			List<Match3StagesDB.LevelReference> multiLevelReference = currentStage.multiLevelReference;
			for (int i = 0; i < multiLevelReference.Count; i++)
			{
				LevelDefinition level = multiLevelReference[i].level;
				if (level != null)
				{
					initParams.levelsList.Add(level);
				}
			}
		}
		initParams.stage = currentStage;
		initParams.levelIndex = currentStage.index;
		initParams.listener = this;
		initParams.disableBackground = true;
		GiftsDefinitionDB.BuildupBooster.BoosterGift boosterGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.buildupBooster.GetBoosterGift();
		if (boosterGift != null)
		{
			initParams.giftBoosterLevel = boosterGift.level;
			List<BoosterConfig> boosterConfig = boosterGift.boosterConfig;
			for (int j = 0; j < boosterConfig.Count; j++)
			{
				BoosterConfig item = boosterConfig[j];
				initParams.activeBoosters.Add(item);
			}
		}
		object2.Show(initParams);
		if (scene != null)
		{
			GGUtil.SetActive(scene, active: true);
		}
		GGSoundSystem.Play(GGSoundSystem.MusicType.GameMusic);
	}

	public void OnAllLifesRefilled()
	{
		NavigationManager.instance.GetObject<OutOfLivesDialog>().Hide();
		ShowPregameDialog();
	}

	public void OnFirstLifeRefilled()
	{
		NavigationManager.instance.GetObject<OutOfLivesDialog>().Hide();
		ShowPregameDialog();
	}

	public void ButtonCallback_PlayButtonClick()
	{
		ShowPregameDialog();
	}

	public void ShowPregameDialog()
	{
		PreGameDialog @object = NavigationManager.instance.GetObject<PreGameDialog>();
		StopReactingToClick();
		HideAllButtons();
		GGUtil.SetActive(widgetsToHide, active: false);
		Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
		@object.Show(currentStage, null, Init, StartGame);
	}

	public void OnGameStarted(GameStartedParams startedParams)
	{
		BehaviourSingleton<EnergyManager>.instance.SpendLifeIfNotFreeEnergy();
		List<BoosterConfig> activeBoosters = startedParams.game.initParams.activeBoosters;
		PlayerInventory instance = PlayerInventory.instance;
		for (int i = 0; i < activeBoosters.Count; i++)
		{
			BoosterConfig boosterConfig = activeBoosters[i];
			instance.SetOwned(boosterConfig.boosterType, instance.OwnedCount(boosterConfig.boosterType) - 1);
			instance.SetUsedCount(boosterConfig.boosterType, instance.UsedCount(boosterConfig.boosterType) + 1);
		}
	}

	public void OnGameComplete(GameCompleteParams completeParams)
	{
		Match3Game game = completeParams.game;
		Match3StagesDB.Stage stage = game.initParams.stage;
		bool flag = false;
		if (stage != null)
		{
			flag = stage.isPassed;
		}
		stage?.OnGameComplete(completeParams);
		if (completeParams.isWin)
		{
			_003C_003Ec__DisplayClass72_0 _003C_003Ec__DisplayClass72_ = new _003C_003Ec__DisplayClass72_0();
			NavigationManager instance = NavigationManager.instance;
			_003C_003Ec__DisplayClass72_.winScreen = instance.GetObject<WinScreen>();
			WalletManager walletManager = GGPlayerSettings.instance.walletManager;
			_003C_003Ec__DisplayClass72_.winScreenArguments = new WinScreen.InitArguments();
			_003C_003Ec__DisplayClass72_.winScreenArguments.previousCoins = walletManager.CurrencyCount(CurrencyType.coins);
			_003C_003Ec__DisplayClass72_.winScreenArguments.baseStageWonCoins = 50L;
			_003C_003Ec__DisplayClass72_.winScreenArguments.previousStars = walletManager.CurrencyCount(CurrencyType.diamonds);
			_003C_003Ec__DisplayClass72_.winScreenArguments.currentStars = _003C_003Ec__DisplayClass72_.winScreenArguments.previousStars + 1;
			_003C_003Ec__DisplayClass72_.winScreenArguments.onMiddle = OnWinScreenAnimationMiddle;
			_003C_003Ec__DisplayClass72_.winScreenArguments.game = game;
			_003C_003Ec__DisplayClass72_.winScreenArguments.decorateRoomScreen = null;
			_003C_003Ec__DisplayClass72_.winScreenArguments.starRect = starRect;
			_003C_003Ec__DisplayClass72_.winScreenArguments.coinRect = coinRect;
			_003C_003Ec__DisplayClass72_.winScreenArguments.currencyPanel = currencyPanel;
			game.StartWinAnimation(_003C_003Ec__DisplayClass72_.winScreenArguments, _003C_003Ec__DisplayClass72_._003COnGameComplete_003Eb__0);
			wonCoinsCount = stage.coinsCount;
			if (!flag)
			{
				GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, Match3Settings.instance.moneyPickupAnimationSettings.numberOfStars);
			}

			if (PlayerPrefs.GetInt("Logged", 0) == 1)
			{
				BehaviourSingleton<MessageUtility>.instance.AutoSave();
			}
			BehaviourSingleton<EnergyManager>.instance.AddLifeIfNotFreeEnergy();
		}
		else
		{
			GGSoundSystem.Play(GGSoundSystem.MusicType.MainMenuMusic);
			if (!game.hasPlayedAnyMoves)
			{
				BehaviourSingleton<EnergyManager>.instance.AddLifeIfNotFreeEnergy();
			}
			GameScreen gameScreen = game.gameScreen;
			NavigationManager.instance.Pop();
			gameScreen.DestroyCreatedGameObjects();
			if (game.hasPlayedAnyMoves)
			{
				ShowPregameDialog();
			}
		}
	}

	private void OnWinScreenAnimationMiddle()
	{
		NavigationManager instance = NavigationManager.instance;
		instance.GetObject<GameScreen>().DestroyCreatedGameObjects();
		GiftsDefinitionDB.GiftDefinition currentGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.currentGift;
		if (currentGift?.isAvailableToCollect ?? false)
		{
			instance.Pop(activateNextScreen: false);
			GiftBoxScreen @object = instance.GetObject<GiftBoxScreen>();
			GiftBoxScreen.ShowArguments showArguments = new GiftBoxScreen.ShowArguments
			{
				giftsDefinition = currentGift.gifts,
				title = ""
			};
			currentGift.ClaimGifts();
			@object.Show(showArguments);
		}
		else
		{
			instance.Pop();
			currencyPanel.SetLabels();
		}
	}

	private void OnWinScreenAnimationComplete()
	{
		GameScreen @object = NavigationManager.instance.GetObject<GameScreen>();
		NavigationManager.instance.Pop();
		@object.DestroyCreatedGameObjects();
		currencyPanel.SetLabels();
	}

	private void OnCoinsGiven()
	{
		currencyPanel.SetLabels();
	}

	public void ButtonCallback_OnLivesClicked()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		if (!BehaviourSingleton<EnergyManager>.instance.isFreeEnergy && BehaviourSingleton<EnergyManager>.instance.ownedPlayCoins != EnergyControlConfig.instance.totalCoin)
		{
			GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
			OutOfLivesDialog @object = NavigationManager.instance.GetObject<OutOfLivesDialog>();
			int index = Match3StagesDB.instance.currentStage.index;
			LivesPriceConfig.PriceConfig priceForLevelOrDefault = ScriptableObjectSingleton<LivesPriceConfig>.instance.GetPriceForLevelOrDefault(index);
			@object.Show(priceForLevelOrDefault, null, null, Init);
		}
	}

	public void ButtonCallback_OnSettingsButtonPress()
	{
		NavigationManager instance = NavigationManager.instance;
		if (ConfigBase.instance.debug)
		{
			SettingsScreen @object = instance.GetObject<SettingsScreen>();
			instance.Push(@object);
		}
		else
		{
			InGameSettingsScreen object2 = instance.GetObject<InGameSettingsScreen>();
			instance.Push(object2);
		}
	}
}
