using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGMatch3
{
	public class GameScreen : MonoBehaviour, IRemoveFromHistoryEventListener
	{
		public class GameProgress
		{
			public Match3Game game;

			public GameCompleteParams completeParams;

			public bool isDone;

			public Vector3 offset;
		}

		public class StageState
		{
			public List<GameProgress> gameProgressList = new List<GameProgress>();

			public IEnumerator runnerEnumerator;

			public MultiLevelGoals goals = new MultiLevelGoals();

			public int currentGameProgressIndex;

			public int hammersUsed;

			public int powerHammersUsed;

			public int additionalMoves
			{
				get
				{
					int num = 0;
					for (int i = 0; i < gameProgressList.Count; i++)
					{
						GameProgress gameProgress = gameProgressList[i];
						num += gameProgress.game.board.totalAdditionalMoves;
					}
					return num;
				}
			}

			public int userMovesCount
			{
				get
				{
					int num = 0;
					for (int i = 0; i < gameProgressList.Count; i++)
					{
						GameProgress gameProgress = gameProgressList[i];
						num += gameProgress.game.board.userMovesCount;
					}
					return num;
				}
			}

			public int userScore
			{
				get
				{
					int num = 0;
					for (int i = 0; i < gameProgressList.Count; i++)
					{
						GameProgress gameProgress = gameProgressList[i];
						num += gameProgress.game.board.userScore;
					}
					return num;
				}
			}

			public int MovesRemaining => goals.TotalMovesCount + additionalMoves - userMovesCount;

			public GameProgress currentGameProgress
			{
				get
				{
					if (currentGameProgressIndex >= gameProgressList.Count || currentGameProgressIndex < 0)
					{
						return null;
					}
					return gameProgressList[currentGameProgressIndex];
				}
			}

			public GameProgress GameProgressForGame(Match3Game game)
			{
				for (int i = 0; i < gameProgressList.Count; i++)
				{
					GameProgress gameProgress = gameProgressList[i];
					if (gameProgress.game == game)
					{
						return gameProgress;
					}
				}
				return null;
			}
		}

		public class Match3GameScene
		{
			public Scene scene;

			public Match3GameStarter starter;

			public Match3GameScene(Scene scene, Match3GameStarter starter)
			{
				this.scene = scene;
				this.starter = starter;
			}
		}

		[Serializable]
		public class MultiLevelAanimationSettings
		{
			public float durationPerScreen;

			public AnimationCurve moveAnimationCurve;

			public float initialDelay;
		}

		private sealed class _003CDoLoadGameScene_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public GameScreen _003C_003E4__this;

			private AsyncOperation _003CasyncOperation_003E5__2;

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
			public _003CDoLoadGameScene_003Ed__36(int _003C_003E1__state)
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
				GameScreen gameScreen = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CasyncOperation_003E5__2 = SceneManager.LoadSceneAsync(gameScreen.gameSceneName, LoadSceneMode.Additive);
					_003CasyncOperation_003E5__2.allowSceneActivation = true;
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (!_003CasyncOperation_003E5__2.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				Scene sceneByName = SceneManager.GetSceneByName(gameScreen.gameSceneName);
				Match3GameStarter starter = null;
				GameObject[] rootGameObjects = sceneByName.GetRootGameObjects();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					Match3GameStarter component = rootGameObjects[i].GetComponent<Match3GameStarter>();
					if (component != null)
					{
						starter = component;
						break;
					}
				}
				gameScreen.gameScene = new Match3GameScene(sceneByName, starter);
				GGUtil.SetActive(gameScreen.hideAll, active: false);
				gameScreen.loadedStyle.Apply();
				gameScreen.StartGame();
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

		private sealed class _003CGameRunner_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public GameScreen _003C_003E4__this;

			private BuyPowerupDialog _003Cdialog_003E5__2;

			private List<GameProgress> _003CgameProgressList_003E5__3;

			private bool _003CisBoostersPlaced_003E5__4;

			private int _003Ci_003E5__5;

			private GameProgress _003CgameProgress_003E5__6;

			private Match3Game _003Cgame_003E5__7;

			private bool _003CisFirstGame_003E5__8;

			private MultiLevelAanimationSettings _003CanimSettings_003E5__9;

			private Vector3 _003CstartOffset_003E5__10;

			private Vector3 _003CendOffset_003E5__11;

			private float _003Ctime_003E5__12;

			private float _003CtotalDuration_003E5__13;

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
			public _003CGameRunner_003Ed__41(int _003C_003E1__state)
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
				GameScreen gameScreen = _003C_003E4__this;
				Vector3 position;
				Match3Game.StartGameArguments arguments;
				LevelDefinition level;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003Cdialog_003E5__2 = BuyPowerupDialog.instance;
					_003Cdialog_003E5__2.Hide();
					_003CgameProgressList_003E5__3 = gameScreen.stageState.gameProgressList;
					RectTransform gamePlayAreaContainer = gameScreen.gamePlayAreaContainer;
					_003CisBoostersPlaced_003E5__4 = false;
					_003Ci_003E5__5 = 0;
					goto IL_0570;
				}
				case 1:
					_003C_003E1__state = -1;
					goto IL_0231;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0346;
				case 3:
					_003C_003E1__state = -1;
					goto IL_048c;
				case 4:
					{
						_003C_003E1__state = -1;
						goto IL_0526;
					}
					IL_0570:
					if (_003Ci_003E5__5 >= _003CgameProgressList_003E5__3.Count)
					{
						break;
					}
					gameScreen.stageState.currentGameProgressIndex = _003Ci_003E5__5;
					_003CgameProgress_003E5__6 = _003CgameProgressList_003E5__3[_003Ci_003E5__5];
					_003Cgame_003E5__7 = _003CgameProgress_003E5__6.game;
					_003CisFirstGame_003E5__8 = (_003Ci_003E5__5 == 0);
					if (_003CisFirstGame_003E5__8 && gameScreen.initParams.listener != null)
					{
						GameStartedParams gameStartedParams = new GameStartedParams();
						gameStartedParams.stageState = gameScreen.stageState;
						gameStartedParams.game = _003Cgame_003E5__7;
						gameScreen.initParams.listener.OnGameStarted(gameStartedParams);
						if (gameScreen.initParams.stage != null)
						{
							gameScreen.initParams.stage.OnGameStarted(gameStartedParams);
						}
					}
					if (_003CisFirstGame_003E5__8 && _003CgameProgressList_003E5__3.Count > 1)
					{
						_003CanimSettings_003E5__9 = Match3Settings.instance.multiLevelAnimationSettings;
						GameProgress gameProgress = _003CgameProgressList_003E5__3[_003CgameProgressList_003E5__3.Count - 1];
						_003CstartOffset_003E5__10 = -gameProgress.offset;
						_003CendOffset_003E5__11 = Vector3.zero;
						position = _003CstartOffset_003E5__10;
						for (int i = 0; i < _003CgameProgressList_003E5__3.Count; i++)
						{
							Match3Game game = _003CgameProgressList_003E5__3[i].game;
							if (!(game == null))
							{
								Transform transform = game.transform;
								position.z = transform.position.z;
								transform.position = position;
							}
						}
						_003Ctime_003E5__12 = 0f;
						if (_003CanimSettings_003E5__9.initialDelay > 0f)
						{
							goto IL_0231;
						}
						goto IL_0244;
					}
					goto IL_0376;
					IL_04b5:
					arguments = default(Match3Game.StartGameArguments);
					arguments.putBoosters = false;
					level = _003Cgame_003E5__7.level;
					if (level != null && !level.isPowerupPlacementSuspended && !_003CisBoostersPlaced_003E5__4)
					{
						_003CisBoostersPlaced_003E5__4 = true;
						arguments.putBoosters = true;
					}
					_003Cdialog_003E5__2.Hide();
					_003Cgame_003E5__7.StartGame(arguments);
					goto IL_0526;
					IL_0376:
					if (!_003CisFirstGame_003E5__8)
					{
						_003CtotalDuration_003E5__13 = 0f;
						GameProgress gameProgress2 = _003CgameProgressList_003E5__3[_003Ci_003E5__5 - 1];
						_003CendOffset_003E5__11 = -gameProgress2.offset;
						_003CstartOffset_003E5__10 = -_003CgameProgress_003E5__6.offset;
						_003Ctime_003E5__12 = 1f;
						goto IL_048c;
					}
					goto IL_04b5;
					IL_0244:
					_003Ctime_003E5__12 -= _003CanimSettings_003E5__9.initialDelay;
					_003CtotalDuration_003E5__13 = _003CanimSettings_003E5__9.durationPerScreen * (float)_003CgameProgressList_003E5__3.Count;
					goto IL_0346;
					IL_0346:
					if (_003Ctime_003E5__12 <= _003CtotalDuration_003E5__13)
					{
						_003Ctime_003E5__12 += Time.deltaTime;
						float time = Mathf.InverseLerp(0f, _003CtotalDuration_003E5__13, _003Ctime_003E5__12);
						time = _003CanimSettings_003E5__9.moveAnimationCurve.Evaluate(time);
						position = Vector3.LerpUnclamped(_003CstartOffset_003E5__10, _003CendOffset_003E5__11, time);
						for (int j = 0; j < _003CgameProgressList_003E5__3.Count; j++)
						{
							Match3Game game2 = _003CgameProgressList_003E5__3[j].game;
							if (!(game2 == null))
							{
								Transform transform2 = game2.transform;
								position.z = transform2.position.z;
								transform2.position = position;
							}
						}
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					_003CanimSettings_003E5__9 = null;
					_003CstartOffset_003E5__10 = default(Vector3);
					_003CendOffset_003E5__11 = default(Vector3);
					goto IL_0376;
					IL_0231:
					if (_003Ctime_003E5__12 <= _003CanimSettings_003E5__9.initialDelay)
					{
						_003Ctime_003E5__12 += Time.deltaTime;
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0244;
					IL_048c:
					if (_003CtotalDuration_003E5__13 <= _003Ctime_003E5__12)
					{
						_003CtotalDuration_003E5__13 += Time.deltaTime;
						float t = Mathf.InverseLerp(0f, _003Ctime_003E5__12, _003CtotalDuration_003E5__13);
						Vector3 position2 = Vector3.LerpUnclamped(_003CendOffset_003E5__11, _003CstartOffset_003E5__10, t);
						for (int k = 0; k < _003CgameProgressList_003E5__3.Count; k++)
						{
							Match3Game game3 = _003CgameProgressList_003E5__3[k].game;
							if (!(game3 == null))
							{
								Transform transform3 = game3.transform;
								position2.z = transform3.position.z;
								transform3.position = position2;
							}
						}
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						return true;
					}
					_003CendOffset_003E5__11 = default(Vector3);
					_003CstartOffset_003E5__10 = default(Vector3);
					goto IL_04b5;
					IL_0526:
					if (!_003CgameProgress_003E5__6.isDone)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 4;
						return true;
					}
					_003Cgame_003E5__7.SuspendGame();
					if (!_003CgameProgress_003E5__6.completeParams.isWin)
					{
						break;
					}
					_003CgameProgress_003E5__6 = null;
					_003Cgame_003E5__7 = null;
					_003Ci_003E5__5++;
					goto IL_0570;
				}
				GGUtil.Hide(gameScreen.exitButton);
				_003Cdialog_003E5__2.Hide();
				GameProgress currentGameProgress = gameScreen.stageState.currentGameProgress;
				if (gameScreen.initParams.listener != null)
				{
					gameScreen.initParams.listener.OnGameComplete(currentGameProgress.completeParams);
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
		private Transform conffettiParticle;

		[SerializeField]
		private Transform background;

		[SerializeField]
		private List<RectTransform> visibleObjects = new List<RectTransform>();

		[SerializeField]
		public string gameSceneName;

		[SerializeField]
		public GoalsPanel goalsPanel;

		[SerializeField]
		public PowerupsPanel powerupsPanel;

		[SerializeField]
		public ShuffleContainer shuffleContainer;

		public MovesContainer movesContainer;

		[SerializeField]
		public WellDoneContainer wellDoneContainer;

		[SerializeField]
		public GameTapToContinueContainer tapToContinueContainer;

		[SerializeField]
		private TextMeshProUGUI levelLabel;

		[SerializeField]
		public TutorialHandController tutorialHand;

		[SerializeField]
		public RankedBoostersStartAnimation rankedBoostersStartAnimation;

		[SerializeField]
		private List<RectTransform> hideAll = new List<RectTransform>();

		[SerializeField]
		private VisualStyleSet loadingStyle = new VisualStyleSet();

		[SerializeField]
		private VisualStyleSet loadedStyle = new VisualStyleSet();

		[SerializeField]
		public InputHandler inputHandler;

		[SerializeField]
		public PowerupPlacementHandler powerupPlacement;

		[SerializeField]
		public RectTransform gamePlayAreaContainer;

		[SerializeField]
		public RectTransform tutorialMask;

		[SerializeField]
		public RectTransform exitButton;

		private Match3GameParams initParams;

		private IEnumerator loadingGameSceneEnum;

		public StageState stageState = new StageState();

		private Match3GameScene gameScene;

		private bool isStarterLoaded
		{
			get
			{
				if (gameScene == null)
				{
					return false;
				}
				return gameScene.starter != null;
			}
		}

		private LevelDefinition CloneLevelAndApplyChanges(LevelDefinition level, Match3GameParams initParams)
		{
			level = level.Clone();
			level.ExchangeBurriedElementsForSmallOnes();
			if (!GGTest.showAdaptiveShowMatch)
			{
				return level;
			}
			Match3StagesDB.Stage stage = initParams.stage;
			if (stage != null)
			{
				if (stage.index < 16)
				{
					return level;
				}
				if (stage.difficulty != 0)
				{
					if (stage.index < 30)
					{
						return level;
					}
					if (stage.timesPlayed >= 1)
					{
						return level;
					}
				}
				if (stage.index < 30)
				{
					if (stage.timesPlayed >= 2)
					{
						return level;
					}
					if (stage.index % 3 == 0)
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
						if (level.suggestMoveType == SuggestMoveType.Normal)
						{
							level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
						}
					}
					else
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					}
				}
				else if (stage.index <= 70)
				{
					if (stage.index % 2 == 0 && stage.timesPlayed <= 2)
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
						if (level.suggestMoveType == SuggestMoveType.Normal)
						{
							level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
						}
					}
					else
					{
						level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					}
				}
				else if (stage.timesPlayed <= 2)
				{
					level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
					if (level.suggestMoveType == SuggestMoveType.Normal)
					{
						level.suggestMoveType = SuggestMoveType.GoodOnFirstAndLast2;
					}
				}
				else
				{
					level.suggestMoveSetting = ShowPotentialMatchSetting.FastMedium;
				}
			}
			return level;
		}

		public void Show(Match3GameParams initParams)
		{
			initParams.level = CloneLevelAndApplyChanges(initParams.level, initParams);
			for (int i = 0; i < initParams.levelsList.Count; i++)
			{
				LevelDefinition level = initParams.levelsList[i];
				level = CloneLevelAndApplyChanges(level, initParams);
				initParams.levelsList[i] = level;
			}
			this.initParams = initParams;
			GGSoundSystem.Play(GGSoundSystem.MusicType.GameMusic);
			NavigationManager.instance.Push(base.gameObject);
			if (initParams == null)
			{
				initParams = new Match3GameParams();
				initParams.level = ScriptableObjectSingleton<LevelDB>.instance.levels[0];
			}
			Init();
		}

		public void OnRemovedFromNavigationHistory()
		{
			DestroyCreatedGameObjects();
		}

		private void Init()
		{
			HideAll();
			loadingGameSceneEnum = null;
			GGUtil.ChangeText(levelLabel, initParams.levelIndex + 1);
			GGUtil.SetActive(background, !initParams.disableBackground);
			if (isStarterLoaded)
			{
				StartGame();
			}
			else
			{
				LoadGameScene();
			}
		}

		public void DestroyCreatedGameObjects()
		{
			if (isStarterLoaded)
			{
				gameScene.starter.DestroyCreatedGameObjects();
				stageState = new StageState();
			}
		}

		private void LoadGameScene()
		{
			GGUtil.SetActive(hideAll, active: false);
			loadingStyle.Apply();
			loadingGameSceneEnum = DoLoadGameScene();
		}

		private IEnumerator DoLoadGameScene()
		{
			return new _003CDoLoadGameScene_003Ed__36(0)
			{
				_003C_003E4__this = this
			};
		}

		private Vector2 ScreenWorldSize()
		{
			RectTransform component = GetComponent<RectTransform>();
			Vector3[] array = new Vector3[4];
			component.GetWorldCorners(array);
			return new Vector2(array[2].x - array[0].x, array[2].y - array[0].y);
		}

		public void ShowConfetti()
		{
			GGUtil.Show(conffettiParticle);
		}

		private void StartGame()
		{
			Match3GameStarter starter = gameScene.starter;
			starter.DestroyCreatedGameObjects();
			List<LevelDefinition> list = new List<LevelDefinition>();
			if (initParams.levelsList.Count > 0)
			{
				list.AddRange(initParams.levelsList);
			}
			else
			{
				list.Add(initParams.level);
			}
			GGUtil.Hide(conffettiParticle);
			tutorialHand.Hide();
			GGUtil.Hide(rankedBoostersStartAnimation);
			GGUtil.Show(exitButton);
			Vector2 vector = ScreenWorldSize();
			stageState = new StageState();
			GGUtil.Hide(powerupPlacement);
			List<GameProgress> gameProgressList = stageState.gameProgressList;
			for (int i = 0; i < list.Count; i++)
			{
				LevelDefinition level = list[i];
				Match3Game match3Game = starter.CreateGame();
				GameProgress gameProgress = new GameProgress();
				gameProgress.game = match3Game;
				gameProgressList.Add(gameProgress);
				Vector3 offset = gameProgress.offset = Vector3.right * vector.x * i;
				HideAll();
				GGUtil.SetActive(visibleObjects, active: true);
				Camera camera = NavigationManager.instance.GetCamera();
				match3Game.Init(camera, this, initParams);
				Match3Game.CreateBoardArguments arguments = default(Match3Game.CreateBoardArguments);
				arguments.level = level;
				arguments.offset = offset;
				match3Game.CreateBoard(arguments);
				if (initParams.stage != null)
				{
					match3Game.SetStageDifficulty(initParams.stage.difficulty);
				}
				stageState.goals.Add(match3Game.goals);
			}
			Match3Game game = gameProgressList[0].game;
			HideAll();
			goalsPanel.Init(stageState);
			powerupsPanel.Init(this);
			stageState.runnerEnumerator = GameRunner();
			stageState.runnerEnumerator.MoveNext();
		}

		private IEnumerator GameRunner()
		{
			return new _003CGameRunner_003Ed__41(0)
			{
				_003C_003E4__this = this
			};
		}

		public void Callback_ShowActivatePowerup(PowerupsPanelPowerup panelPowerup)
		{
			GameProgress currentGameProgress = stageState.currentGameProgress;
			if (currentGameProgress != null && !currentGameProgress.isDone)
			{
				PowerupsDB.PowerupDefinition powerup = panelPowerup.powerup;
				currentGameProgress.game.Callback_ShowActivatePowerup(powerup);
			}
		}

		public void Match3GameCallback_OnGameOutOfMoves(GameCompleteParams completeParams)
		{
			Match3Game game = completeParams.game;
			OutOfMovesDialog @object = NavigationManager.instance.GetObject<OutOfMovesDialog>();
			if (!(@object == null))
			{
				BuyMovesPricesConfig.OfferConfig offer = ScriptableObjectSingleton<BuyMovesPricesConfig>.instance.GetOffer(completeParams.stageState);
				@object.Show(offer, game, stageState.goals, OutOfMovesCallback_OnPlayOnOfferYes, OutOfMovesCallback_OnPlayOnOfferNo);
			}
		}

		private void OutOfMovesCallback_OnPlayOnOfferYes(OutOfMovesDialog dialog)
		{
			BuyMovesPricesConfig.OfferConfig offer = dialog.offer;
			WalletManager walletManager = GGPlayerSettings.instance.walletManager;
			if (!walletManager.CanBuyItemWithPrice(offer.price))
			{
				NavigationManager.instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance);
				return;
			}
			Match3Game game = dialog.game;
			walletManager.BuyItem(offer.price);
			game.ContinueGameAfterOffer(offer);
			goalsPanel.UpdateMovesCount();
			dialog.Hide();
			Analytics.MovesBoughtEvent movesBoughtEvent = new Analytics.MovesBoughtEvent();
			movesBoughtEvent.stageState = stageState;
			movesBoughtEvent.offer = offer;
			movesBoughtEvent.Send();
		}

		private void OutOfMovesCallback_OnPlayOnOfferNo(OutOfMovesDialog dialog)
		{
			dialog.Hide();
			GameProgress currentGameProgress = stageState.currentGameProgress;
			currentGameProgress.isDone = true;
			GameCompleteParams gameCompleteParams = new GameCompleteParams();
			gameCompleteParams.isWin = false;
			gameCompleteParams.stageState = stageState;
			gameCompleteParams.game = currentGameProgress.game;
			currentGameProgress.completeParams = gameCompleteParams;
		}

		public void Match3GameCallback_OnGameWon(GameCompleteParams completeParams)
		{
			Match3Game game = completeParams.game;
			GameProgress gameProgress = stageState.GameProgressForGame(game);
			if (gameProgress == null || gameProgress.game != game)
			{
				UnityEngine.Debug.LogError("GAME PROGRESS NULL isNull " + (gameProgress == null).ToString() + " listCount " + stageState.gameProgressList.Count);
				return;
			}
			gameProgress.completeParams = completeParams;
			gameProgress.isDone = true;
		}

		public void HideVisibleObjects()
		{
			GGUtil.SetActive(visibleObjects, active: false);
		}

		public void HideAll()
		{
			GGUtil.Hide(shuffleContainer);
			GGUtil.Hide(movesContainer);
			GGUtil.Hide(wellDoneContainer);
			GGUtil.Hide(tapToContinueContainer);
			GGUtil.Hide(tutorialMask);
			GGUtil.Hide(powerupPlacement);
			if (shuffleContainer != null)
			{
				shuffleContainer.Reset();
			}
		}

		private void Update()
		{
			if (stageState.runnerEnumerator != null && !stageState.runnerEnumerator.MoveNext())
			{
				stageState.runnerEnumerator = null;
			}
			if (loadingGameSceneEnum != null && !loadingGameSceneEnum.MoveNext())
			{
				loadingGameSceneEnum = null;
			}
		}

		public void ButtonCallback_OnExitButtonPressed()
		{
			NavigationManager instance = NavigationManager.instance;
			GameProgress currentGameProgress = stageState.currentGameProgress;
			if (currentGameProgress != null)
			{
				Match3Game game = currentGameProgress.game;
				if (stageState.userMovesCount > 0)
				{
					game.SuspendGame();
					ExitGameConfirmDialog.ExitGameConfirmArguments arguments = default(ExitGameConfirmDialog.ExitGameConfirmArguments);
					arguments.goals = stageState.goals;
					arguments.game = game;
					arguments.onCompleteCallback = ExitGameConfirmDialogCallback_OnExit;
					instance.GetObject<ExitGameConfirmDialog>().Show(arguments);
				}
				else
				{
					game.QuitGame();
					currentGameProgress.isDone = true;
					GameCompleteParams gameCompleteParams = new GameCompleteParams();
					gameCompleteParams.isWin = false;
					gameCompleteParams.game = game;
					gameCompleteParams.stageState = stageState;
					currentGameProgress.completeParams = gameCompleteParams;
				}
			}
		}

		private void ExitGameConfirmDialogCallback_OnExit(bool shouldExit)
		{
			GameProgress currentGameProgress = stageState.currentGameProgress;
			Match3Game game = currentGameProgress.game;
			if (currentGameProgress != null)
			{
				if (!shouldExit)
				{
					game.ResumeGame();
					return;
				}
				currentGameProgress.isDone = true;
				GameCompleteParams gameCompleteParams = new GameCompleteParams();
				gameCompleteParams.isWin = false;
				gameCompleteParams.game = game;
				gameCompleteParams.stageState = stageState;
				currentGameProgress.completeParams = gameCompleteParams;
			}
		}
	}
}
