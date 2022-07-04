using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class Match3Goals
	{
		public struct ChipTypeDef
		{
			public ChipType chipType;

			public ItemColor itemColor;

			public static bool HasColor(ChipType chipType)
			{
				if (chipType != 0)
				{
					return chipType == ChipType.MonsterBlocker;
				}
				return true;
			}

			public static ChipTypeDef Create(GoalConfig goalConfig)
			{
				ChipTypeDef result = default(ChipTypeDef);
				result.chipType = goalConfig.chipType;
				if (HasColor(goalConfig.chipType))
				{
					result.itemColor = goalConfig.itemColor;
				}
				else
				{
					result.itemColor = ItemColor.Uncolored;
				}
				return result;
			}

			public static ChipTypeDef Create(Chip chip)
			{
				ChipTypeDef result = default(ChipTypeDef);
				result.chipType = chip.chipType;
				if (HasColor(chip.chipType))
				{
					result.itemColor = chip.itemColor;
				}
				else
				{
					result.itemColor = ItemColor.Uncolored;
				}
				return result;
			}

			private bool IsColorCompatible(ItemColor a, ItemColor b)
			{
				if (a == ItemColor.AnyColor || b == ItemColor.AnyColor)
				{
					return true;
				}
				return a == b;
			}

			public bool IsEqual(ChipTypeDef b)
			{
				if (chipType != b.chipType)
				{
					return false;
				}
				if (!HasColor(b.chipType))
				{
					return true;
				}
				return IsColorCompatible(itemColor, b.itemColor);
			}
		}

		public class ChipTypeCounter
		{
			public ChipTypeDef chipTypeDef;

			public int count;

			public int countAtStart;
		}

		private struct SlotData
		{
			public Slot slot;

			public int score;

			public int carpetScore;

			public int blockingLevel;

			public bool isAvailableToSelect;

			public int fallingPickupScore;

			public int chipScore;

			public int TotalScore(bool hasCarpet)
			{
				int num = score;
				if (hasCarpet)
				{
					num += carpetScore;
				}
				return num;
			}

			public void Init(Slot slot)
			{
				this.slot = slot;
				score = 0;
				isAvailableToSelect = false;
				fallingPickupScore = 0;
				blockingLevel = 0;
				carpetScore = 0;
				chipScore = 0;
			}

			public bool IsDestroyable(int additionalLevelsDown)
			{
				if (slot == null)
				{
					return false;
				}
				if (slot.isDestroySuspended)
				{
					return false;
				}
				if (slot.IsAboutToBeDestroyedLocksCount() + additionalLevelsDown > slot.maxBlockerLevel)
				{
					return false;
				}
				return true;
			}

			public SlotData(Slot slot, int score)
			{
				this.slot = slot;
				this.score = score;
				isAvailableToSelect = false;
				blockingLevel = 0;
				fallingPickupScore = 0;
				carpetScore = 0;
				chipScore = 0;
			}
		}

		private struct SlotWithScore
		{
			public Slot slot;

			public int score;

			private ActionScore actionScore;
		}

		public class GoalBase
		{
			public GoalConfig config;

			public Match3Goals goals;

			private readonly int _003CCountAtStart_003Ek__BackingField;

			private readonly int _003CRemainingCount_003Ek__BackingField;

			public virtual int CountAtStart => _003CCountAtStart_003Ek__BackingField;

			public virtual int RemainingCount => _003CRemainingCount_003Ek__BackingField;

			public virtual void Init(GoalConfig config, Match3Goals goals)
			{
				this.config = config;
				this.goals = goals;
			}

			public virtual bool IsComplete()
			{
				return false;
			}

			public virtual bool IsPartOfGoal(ChipTypeDef chipType)
			{
				return false;
			}

			public virtual int ScoreForSlot(Slot slot)
			{
				return 0;
			}
		}

		public class CollectItemsGoal : GoalBase
		{
			private ChipTypeDef chipTypeDef => ChipTypeDef.Create(config);

			public override int CountAtStart
			{
				get
				{
					ChipTypeCounter chipTypeCounter = goals.GetChipTypeCounter(chipTypeDef);
					int result = config.collectCount;
					if (config.isCollectAllPresentAtStart)
					{
						result = chipTypeCounter.countAtStart;
					}
					return result;
				}
			}

			public override int RemainingCount
			{
				get
				{
					ChipTypeCounter chipTypeCounter = goals.GetChipTypeCounter(chipTypeDef);
					int num = config.collectCount;
					if (config.isCollectAllPresentAtStart)
					{
						num = chipTypeCounter.countAtStart;
					}
					return num - chipTypeCounter.count;
				}
			}

			public override bool IsPartOfGoal(ChipTypeDef chipType)
			{
				return chipType.IsEqual(chipTypeDef);
			}

			public override bool IsComplete()
			{
				return RemainingCount <= 0;
			}

			public override int ScoreForSlot(Slot slot)
			{
				if (chipTypeDef.chipType == ChipType.FallingGingerbreadMan)
				{
					return 0;
				}
				if (IsComplete())
				{
					return 0;
				}
				int result = 0;
				if (slot.IsCompatibleWithPickupGoal(chipTypeDef))
				{
					result = ((chipTypeDef.chipType == ChipType.Chip) ? 1 : ((chipTypeDef.chipType != ChipType.GrowingElementPiece) ? 2 : ((slot.GetSlotComponent<GrowingElementChip>() != null) ? 1 : 2)));
				}
				return result;
			}
		}

		private List<ChipTypeCounter> chipTypeCounters = new List<ChipTypeCounter>();

		public List<GoalBase> goals = new List<GoalBase>();

		private GoalsDefinition goalsDefinition;

		private SlotData[] slotData;

		private List<Slot> bestSlotsForSeekingMissle = new List<Slot>();

		private List<SlotData> potentialSlotsForSeekingMissle = new List<SlotData>();

		private List<SlotData> potentialSlotsForSeekingMissleJustBlockers = new List<SlotData>();

		private List<SlotWithScore> potentialSlotsForSeekingMissleWithChip = new List<SlotWithScore>();

		private List<Slot> slotsWithFallingPickups = new List<Slot>();

		public bool isAllGoalsComplete
		{
			get
			{
				for (int i = 0; i < goals.Count; i++)
				{
					if (!goals[i].IsComplete())
					{
						return false;
					}
				}
				return true;
			}
		}

		public int TotalMovesCount => goalsDefinition.movesCount;

		public ActionScore ActionScoreForDestroyingSwitchingSlots(Slot exchangeSlot, Slot slotToSwipe, Match3Game game, bool isHavingCarpet, bool includeNeighbours)
		{
			ActionScore actionScore = default(ActionScore);
			actionScore = ActionScoreForDestroyingSwitchingSlots(exchangeSlot, slotToSwipe, isHavingCarpet);
			if (!includeNeighbours)
			{
				return actionScore;
			}
			List<Slot> neigbourSlots = exchangeSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot = neigbourSlots[i];
				if (slot != null && slot.isDestroyedByMatchingNextTo)
				{
					actionScore += ActionScoreForDestroyingSlot(slot, isHavingCarpet: false);
				}
			}
			return actionScore;
		}

		public ActionScore ActionScoreForDestroyingSlot(Slot slot, Match3Game game, bool isHavingCarpet, bool includeNeighbours)
		{
			ActionScore result = default(ActionScore);
			if (slot == null)
			{
				return result;
			}
			result = ActionScoreForDestroyingSlot(slot, isHavingCarpet);
			if (!includeNeighbours)
			{
				return result;
			}
			List<Slot> neigbourSlots = slot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot2 = neigbourSlots[i];
				if (slot2 != null && slot2.isDestroyedByMatchingNextTo)
				{
					result += ActionScoreForDestroyingSlot(slot2, isHavingCarpet: false);
				}
			}
			return result;
		}

		private ActionScore ActionScoreForDestroyingSwitchingSlots(Slot exchangeSlot, Slot slotToSwipe, bool isHavingCarpet)
		{
			ActionScore result = default(ActionScore);
			if (exchangeSlot == null || slotToSwipe == null)
			{
				return result;
			}
			if (this.slotData == null)
			{
				return result;
			}
			Match3Game game = exchangeSlot.game;
			int num = game.board.Index(exchangeSlot.position);
			SlotData slotData = this.slotData[num];
			int num2 = game.board.Index(slotToSwipe.position);
			SlotData slotData2 = this.slotData[num2];
			if (!slotData.IsDestroyable(0))
			{
				return result;
			}
			if (isHavingCarpet && slotData.carpetScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.score - slotData.chipScore + slotData2.chipScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.blockingLevel > 0)
			{
				result.obstaclesDestroyed = 1;
			}
			return result;
		}

		private ActionScore ActionScoreForDestroyingSlot(Slot slot, bool isHavingCarpet)
		{
			ActionScore result = default(ActionScore);
			if (slot == null)
			{
				return result;
			}
			if (this.slotData == null)
			{
				return result;
			}
			int num = slot.game.board.Index(slot.position);
			SlotData slotData = this.slotData[num];
			if (!slotData.IsDestroyable(0))
			{
				return result;
			}
			if (isHavingCarpet && slotData.carpetScore > 0)
			{
				result.goalsCount++;
			}
			if (slotData.isAvailableToSelect)
			{
				result.goalsCount++;
			}
			if (slotData.blockingLevel > 0)
			{
				result.obstaclesDestroyed = 1;
			}
			return result;
		}

		public ActionScore FreshActionScoreForDestroyingSlot(Slot slot)
		{
			ActionScore result = default(ActionScore);
			if (slot == null)
			{
				return result;
			}
			for (int i = 0; i < goals.Count; i++)
			{
				GoalBase goalBase = goals[i];
				if (slot.IsCompatibleWithPickupGoal(ChipTypeDef.Create(goalBase.config)))
				{
					result.goalsCount++;
				}
				result.obstaclesDestroyed = slot.totalBlockerLevel;
			}
			return result;
		}

		public bool IsDestroyingSlotCompleatingAGoal(Slot slot, Match3Game game, bool includeNeighbours)
		{
			if (slot == null)
			{
				return false;
			}
			if (IsDestroyingSlotCompleatingAGoal(slot))
			{
				return true;
			}
			if (!includeNeighbours)
			{
				return false;
			}
			List<Slot> neigbourSlots = slot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				Slot slot2 = neigbourSlots[i];
				if (slot2 != null && slot2.isDestroyedByMatchingNextTo && IsDestroyingSlotCompleatingAGoal(slot2))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsDestroyingSlotCompleatingAGoal(Slot slot)
		{
			if (slot == null)
			{
				return false;
			}
			if (this.slotData == null)
			{
				return false;
			}
			int num = slot.game.board.Index(slot.position);
			SlotData slotData = this.slotData[num];
			if (!slotData.isAvailableToSelect)
			{
				return false;
			}
			return slotData.IsDestroyable(0);
		}

		public void FillSlotData(Match3Game game)
		{
			Slot[] slots = game.board.slots;
			slotsWithFallingPickups.Clear();
			if (this.slotData == null)
			{
				this.slotData = new SlotData[slots.Length];
				for (int i = 0; i < slots.Length; i++)
				{
					Slot slot = slots[i];
					this.slotData[i] = new SlotData(slot, 0);
				}
			}
			for (int j = 0; j < slots.Length; j++)
			{
				Slot slot2 = slots[j];
				this.slotData[j].Init(slot2);
				if (slot2 == null)
				{
					continue;
				}
				Chip slotComponent = slot2.GetSlotComponent<Chip>();
				if (slotComponent != null && slotComponent.isFallingPickupElement)
				{
					slotsWithFallingPickups.Add(slot2);
				}
				int num = 0;
				int num2 = 0;
				for (int k = 0; k < goals.Count; k++)
				{
					GoalBase goalBase = goals[k];
					int num3 = goalBase.ScoreForSlot(slot2);
					num2 += num3;
					if (goalBase.config.chipType == ChipType.Chip)
					{
						num += num3;
					}
				}
				SlotData slotData = this.slotData[j];
				slotData.score = num2;
				slotData.chipScore = num;
				slotData.blockingLevel = slot2.totalBlockerLevel;
				if (game.board.carpet.IsPossibleToAddCarpet(slot2.position))
				{
					slotData.carpetScore++;
				}
				slotData.isAvailableToSelect = (num2 > 0);
				this.slotData[j] = slotData;
			}
			for (int l = 0; l < slotsWithFallingPickups.Count; l++)
			{
				Slot slot3 = slotsWithFallingPickups[l];
				Chip slotComponent2 = slot3.GetSlotComponent<Chip>();
				if (slotComponent2 == null)
				{
					continue;
				}
				ChipTypeDef chipType = ChipTypeDef.Create(slotComponent2);
				for (int m = 0; m < goals.Count; m++)
				{
					if (!goals[m].IsPartOfGoal(chipType))
					{
						continue;
					}
					int num4 = 0;
					Slot slot4 = slot3;
					while (slot4 != null && slot4 != null)
					{
						bool flag = slot4.totalBlockerLevelForFalling > 0;
						Chip slotComponent3 = slot4.GetSlotComponent<Chip>();
						bool flag2 = slotComponent3 != null && (slotComponent3.chipType == ChipType.Chip || slotComponent3.isPickupElement);
						if (!flag && !flag2)
						{
							num4++;
							slot4 = slot4.NextSlotToPushToWithoutSandflow();
							continue;
						}
						int num5 = game.board.Index(slot4.position);
						SlotData slotData2 = this.slotData[num5];
						slotData2.score++;
						int b = Mathf.Max(game.board.size.x, game.board.size.y) - num4;
						slotData2.fallingPickupScore = Mathf.Max(slotData2.fallingPickupScore, b);
						slotData2.isAvailableToSelect = true;
						this.slotData[num5] = slotData2;
						slot4 = slot4.NextSlotToPushToWithoutSandflow();
						num4++;
						if (flag)
						{
							break;
						}
					}
				}
			}
		}

		public List<Slot> BestSlotsForSeekingMissleWithChip(Match3Game game, Slot originSlot, ChipType otherChipType)
		{
			bestSlotsForSeekingMissle.Clear();
			potentialSlotsForSeekingMissleWithChip.Clear();
			potentialSlotsForSeekingMissleJustBlockers.Clear();
			FillSlotData(game);
			bool flag = originSlot.canCarpetSpreadFromHere;
			List<Slot> neigbourSlots = originSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				if (neigbourSlots[i].canCarpetSpreadFromHere)
				{
					flag = true;
					break;
				}
			}
			int b = 0;
			int b2 = 0;
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < this.slotData.Length; j++)
			{
				SlotData slotData = this.slotData[j];
				Slot slot = slotData.slot;
				bool num3 = slotData.isAvailableToSelect || (flag && slotData.carpetScore > 0);
				int blockingLevel = slotData.blockingLevel;
				if (blockingLevel > 0)
				{
					potentialSlotsForSeekingMissleJustBlockers.Add(slotData);
					num2 = Mathf.Max(blockingLevel, blockingLevel);
				}
				if (!num3 || slot == originSlot)
				{
					continue;
				}
				int additionalLevelsDown = 0;
				if (!slotData.IsDestroyable(additionalLevelsDown))
				{
					continue;
				}
				List<Slot> areaOfEffect = game.GetAreaOfEffect(otherChipType, slot.position);
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				for (int k = 0; k < areaOfEffect.Count; k++)
				{
					Slot slot2 = areaOfEffect[k];
					SlotData slotData2 = this.slotData[game.board.Index(slot2.position)];
					num4 += slotData2.TotalScore(flag);
					num5 += slotData.fallingPickupScore;
					if (slotData2.score == 0)
					{
						bool flag2 = slot2.totalBlockerLevel > 0;
						num6 += (flag2 ? 1 : 0);
					}
				}
				int goalsCount = num4 + num5;
				ActionScore actionScore = default(ActionScore);
				actionScore.goalsCount = goalsCount;
				actionScore.obstaclesDestroyed = num6;
				goalsCount = actionScore.GoalsAndObstaclesScore(2);
				b = Mathf.Max(num4, b);
				b2 = Mathf.Max(num5, b2);
				num = Mathf.Max(goalsCount, num);
				SlotWithScore item = default(SlotWithScore);
				item.slot = slotData.slot;
				item.score = goalsCount;
				potentialSlotsForSeekingMissleWithChip.Add(item);
			}
			for (int l = 0; l < potentialSlotsForSeekingMissleWithChip.Count; l++)
			{
				SlotWithScore slotWithScore = potentialSlotsForSeekingMissleWithChip[l];
				if (slotWithScore.score == num)
				{
					bestSlotsForSeekingMissle.Add(slotWithScore.slot);
				}
			}
			if (bestSlotsForSeekingMissle.Count == 0)
			{
				for (int m = 0; m < potentialSlotsForSeekingMissleJustBlockers.Count; m++)
				{
					SlotData slotData3 = potentialSlotsForSeekingMissleJustBlockers[m];
					if (slotData3.blockingLevel == num2)
					{
						bestSlotsForSeekingMissle.Add(slotData3.slot);
					}
				}
			}
			return bestSlotsForSeekingMissle;
		}

		public List<Slot> BestSlotsForSeekingMissle(Match3Game game, Slot originSlot)
		{
			bestSlotsForSeekingMissle.Clear();
			potentialSlotsForSeekingMissle.Clear();
			potentialSlotsForSeekingMissleJustBlockers.Clear();
			FillSlotData(game);
			bool flag = originSlot.canCarpetSpreadFromHere;
			List<Slot> neigbourSlots = originSlot.neigbourSlots;
			for (int i = 0; i < neigbourSlots.Count; i++)
			{
				if (neigbourSlots[i].canCarpetSpreadFromHere)
				{
					flag = true;
					break;
				}
			}
			int b = 0;
			int b2 = 0;
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < this.slotData.Length; j++)
			{
				SlotData slotData = this.slotData[j];
				Slot slot = slotData.slot;
				bool num3 = slotData.isAvailableToSelect || (flag && slotData.carpetScore > 0);
				int blockingLevel = slotData.blockingLevel;
				if (blockingLevel > 0)
				{
					potentialSlotsForSeekingMissleJustBlockers.Add(slotData);
					num2 = Mathf.Max(blockingLevel, blockingLevel);
				}
				if (num3)
				{
					int additionalLevelsDown = 0;
					if (slotData.IsDestroyable(additionalLevelsDown))
					{
						b = Mathf.Max(slotData.TotalScore(flag), b);
						b2 = Mathf.Max(slotData.fallingPickupScore, b2);
						num = Mathf.Max(slotData.TotalScore(flag) + slotData.fallingPickupScore, num);
						potentialSlotsForSeekingMissle.Add(slotData);
					}
				}
			}
			for (int k = 0; k < potentialSlotsForSeekingMissle.Count; k++)
			{
				SlotData slotData2 = potentialSlotsForSeekingMissle[k];
				if (slotData2.TotalScore(flag) + slotData2.fallingPickupScore == num)
				{
					bestSlotsForSeekingMissle.Add(slotData2.slot);
				}
			}
			if (bestSlotsForSeekingMissle.Count == 0)
			{
				for (int l = 0; l < potentialSlotsForSeekingMissleJustBlockers.Count; l++)
				{
					SlotData slotData3 = potentialSlotsForSeekingMissleJustBlockers[l];
					if (slotData3.blockingLevel == num2)
					{
						bestSlotsForSeekingMissle.Add(slotData3.slot);
					}
				}
			}
			return bestSlotsForSeekingMissle;
		}

		public GoalBase GetActiveGoal(ChipTypeDef chipTypeDef)
		{
			for (int i = 0; i < goals.Count; i++)
			{
				GoalBase goalBase = goals[i];
				if (!goalBase.IsComplete() && goalBase.IsPartOfGoal(chipTypeDef))
				{
					return goalBase;
				}
			}
			return null;
		}

		public List<GoalBase> GetActiveGoals()
		{
			List<GoalBase> list = new List<GoalBase>();
			for (int i = 0; i < goals.Count; i++)
			{
				GoalBase goalBase = goals[i];
				if (!goalBase.IsComplete())
				{
					list.Add(goalBase);
				}
			}
			return list;
		}

		public void OnPickupGoal(GoalBase goal)
		{
			if (goal != null)
			{
				GetChipTypeCounter(ChipTypeDef.Create(goal.config)).count++;
			}
		}

		public ChipTypeCounter GetChipTypeCounter(ChipTypeDef chipTypeDef)
		{
			for (int i = 0; i < chipTypeCounters.Count; i++)
			{
				ChipTypeCounter chipTypeCounter = chipTypeCounters[i];
				if (chipTypeCounter.chipTypeDef.IsEqual(chipTypeDef))
				{
					return chipTypeCounter;
				}
			}
			ChipTypeCounter chipTypeCounter2 = new ChipTypeCounter();
			chipTypeCounter2.chipTypeDef = chipTypeDef;
			chipTypeCounters.Add(chipTypeCounter2);
			return chipTypeCounter2;
		}

		public void Init(LevelDefinition levelDefinition, Match3Game game)
		{
			goalsDefinition = levelDefinition.goals;
			List<GoalConfig> list = goalsDefinition.goals;
			for (int i = 0; i < list.Count; i++)
			{
				GoalConfig goalConfig = list[i];
				GoalBase item = Create(goalConfig);
				goals.Add(item);
			}
			int count = levelDefinition.extraFallingElements.fallingElementsList.Count;
			if (count > 0)
			{
				ChipTypeDef chipTypeDef = default(ChipTypeDef);
				chipTypeDef.chipType = ChipType.FallingGingerbreadMan;
				chipTypeDef.itemColor = ItemColor.Unknown;
				GetChipTypeCounter(chipTypeDef).countAtStart += count;
			}
			Slot[] slots = game.board.slots;
			for (int j = 0; j < slots.Length; j++)
			{
				slots[j]?.AddToGoalsAtStart(this);
			}
			game.board.burriedElements.AddToGoalsAtStart(this);
			game.board.monsterElements.AddToGoalsAtStart(this);
			game.board.carpet.AddToGoalsAtStart(this);
		}

		public GoalBase Create(GoalConfig goalConfig)
		{
			GoalType goalType = goalConfig.goalType;
			GoalBase goalBase = null;
			if (goalType == GoalType.CollectItems)
			{
				goalBase = new CollectItemsGoal();
			}
			if (goalBase == null)
			{
				return goalBase;
			}
			goalBase.Init(goalConfig, this);
			return goalBase;
		}
	}
}
