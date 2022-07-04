using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GGMatch3
{
	public class GoalsPanel : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> widgetsToHide = new List<Transform>();

		[SerializeField]
		private VisualStyleSet goalsDisplayStyle = new VisualStyleSet();

		[SerializeField]
		private VisualStyleSet coinsDisplayStyle = new VisualStyleSet();

		[SerializeField]
		private ComponentPool goalsPool = new ComponentPool();

		[SerializeField]
		public TextMeshProUGUI movesCountLabel;

		[SerializeField]
		private RectTransform goalsContainer;

		[SerializeField]
		public RectTransform coinsTransform;

		[SerializeField]
		private TextMeshProUGUI coinsCountLabel;

		private List<GoalsPanelGoal> uiGoalsList = new List<GoalsPanelGoal>();

		private GameScreen.StageState stageState;

		private VisibilityHelper visibilityHelper = new VisibilityHelper();

		[SerializeField]
		private TextMeshProUGUI pointsLabel;

		private int currentDisplayedScore;

		private int desiredScore;

		public void Init(GameScreen.StageState stageState)
		{
			visibilityHelper.SetActive(widgetsToHide, isVisible: false);
			goalsDisplayStyle.Apply(visibilityHelper);
			visibilityHelper.Complete();
			this.stageState = stageState;
			Vector2 prefabSizeDelta = goalsPool.prefabSizeDelta;
			List<MultiLevelGoals.Goal> allGoals = stageState.goals.allGoals;
			Vector2 sizeDeltum = goalsContainer.sizeDelta;
			Vector3 a = new Vector3(0f, prefabSizeDelta.y * ((float)allGoals.Count * 0.5f - 0.5f), 0f);
			goalsPool.Clear();
			uiGoalsList.Clear();
			for (int i = 0; i < allGoals.Count; i++)
			{
				MultiLevelGoals.Goal goal = allGoals[i];
				GoalsPanelGoal goalsPanelGoal = goalsPool.Next<GoalsPanelGoal>(activate: true);
				goalsPanelGoal.transform.localPosition = a + Vector3.down * (prefabSizeDelta.y * (float)i);
				goalsPanelGoal.Init(goal);
				uiGoalsList.Add(goalsPanelGoal);
			}
			goalsPool.HideNotUsed();
			UpdateMovesCount();
			currentDisplayedScore = (desiredScore = stageState.userScore);
			UpdateScore();
		}

		public void UpdateScore()
		{
			desiredScore = stageState.userScore;
		}

		public void ShowCoins()
		{
			visibilityHelper.SetActive(widgetsToHide, isVisible: false);
			coinsDisplayStyle.Apply(visibilityHelper);
			visibilityHelper.Complete();
		}

		public void SetCoinsCount(long coinsCount)
		{
			GGUtil.ChangeText(coinsCountLabel, coinsCount);
		}

		public GoalsPanelGoal GetGoal(Match3Goals.GoalBase goal)
		{
			if (goal == null)
			{
				return null;
			}
			for (int i = 0; i < uiGoalsList.Count; i++)
			{
				GoalsPanelGoal goalsPanelGoal = uiGoalsList[i];
				if (goalsPanelGoal.goal.IsCompatible(goal))
				{
					return goalsPanelGoal;
				}
			}
			return null;
		}

		public void UpdateMovesCount()
		{
			GGUtil.ChangeText(text: stageState.MovesRemaining.ToString(), label: movesCountLabel);
		}

		private void Update()
		{
			if (currentDisplayedScore < desiredScore)
			{
				GeneralSettings generalSettings = Match3Settings.instance.generalSettings;
				float a = (float)currentDisplayedScore + generalSettings.scoreSpeed * Time.deltaTime;
				float b = Mathf.Lerp(currentDisplayedScore, desiredScore, Time.deltaTime * generalSettings.lerpSpeed);
				float f = Mathf.Max(a, b);
				currentDisplayedScore = Mathf.Min(Mathf.RoundToInt(f), desiredScore);
				GGUtil.ChangeText(pointsLabel, currentDisplayedScore.ToString());
			}
		}
	}
}
