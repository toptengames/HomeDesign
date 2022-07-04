using System.Collections.Generic;

namespace GGMatch3
{
	public class MultiLevelGoals
	{
		public class Goal
		{
			public GoalConfig config;

			public List<Match3Goals.GoalBase> goals = new List<Match3Goals.GoalBase>();

			public bool isComplete => RemainingCount <= 0;

			public int RemainingCount
			{
				get
				{
					int num = 0;
					for (int i = 0; i < goals.Count; i++)
					{
						Match3Goals.GoalBase goalBase = goals[i];
						num += goalBase.RemainingCount;
					}
					return num;
				}
			}

			public bool IsCompatible(Match3Goals.GoalBase goal)
			{
				return goal?.config.IsCompatible(config) ?? false;
			}
		}

		private List<Match3Goals> goalsList = new List<Match3Goals>();

		public List<Goal> allGoals = new List<Goal>();

		private List<Goal> activeGoals = new List<Goal>();

		public int TotalMovesCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < goalsList.Count; i++)
				{
					Match3Goals match3Goals = goalsList[i];
					num += match3Goals.TotalMovesCount;
				}
				return num;
			}
		}

		public List<Goal> GetActiveGoals()
		{
			activeGoals.Clear();
			for (int i = 0; i < allGoals.Count; i++)
			{
				Goal goal = allGoals[i];
				if (!goal.isComplete)
				{
					activeGoals.Add(goal);
				}
			}
			return activeGoals;
		}

		private Goal GetOrCreateGoal(GoalConfig goalConfig)
		{
			for (int i = 0; i < allGoals.Count; i++)
			{
				Goal goal = allGoals[i];
				if (goal.config.IsCompatible(goalConfig))
				{
					return goal;
				}
			}
			Goal goal2 = new Goal();
			goal2.config = goalConfig;
			allGoals.Add(goal2);
			return goal2;
		}

		public void Add(Match3Goals goals)
		{
			goalsList.Add(goals);
			List<Match3Goals.GoalBase> goals2 = goals.goals;
			for (int i = 0; i < goals2.Count; i++)
			{
				Match3Goals.GoalBase goalBase = goals2[i];
				GetOrCreateGoal(goalBase.config).goals.Add(goalBase);
			}
		}
	}
}
