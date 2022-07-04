using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class LivesCountDisplay : MonoBehaviour
{
	private sealed class _003CCheckLivesRegeneration_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LivesCountDisplay _003C_003E4__this;

		private float _003Ctimer_003E5__2;

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
		public _003CCheckLivesRegeneration_003Ed__12(int _003C_003E1__state)
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
			LivesCountDisplay livesCountDisplay = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0029;
			case 1:
				_003C_003E1__state = -1;
				_003Ctimer_003E5__2 += Time.deltaTime;
				break;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0029;
				}
				IL_0029:
				_003Ctimer_003E5__2 = 0f;
				break;
			}
			if (_003Ctimer_003E5__2 < livesCountDisplay.regenerationIntervalSeconds)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			livesCountDisplay.UpdateVisualy();
			_003C_003E2__current = null;
			_003C_003E1__state = 2;
			return true;
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
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet livesCountStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet infiniteEnergyCountStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet fullLivesCountStyle = new VisualStyleSet();

	[SerializeField]
	private TextMeshProUGUI livesText;

	[SerializeField]
	private TextMeshProUGUI heartsLivesText;

	[SerializeField]
	public float regenerationIntervalSeconds = 1f;

	private VisibilityHelper visibility = new VisibilityHelper();

	private IEnumerator updateEnumerator;

	private void OnEnable()
	{
		UpdateVisualy();
	}

	private void Update()
	{
		if (updateEnumerator == null)
		{
			updateEnumerator = CheckLivesRegeneration();
		}
		updateEnumerator.MoveNext();
	}

	private void UpdateVisualy()
	{
		EnergyManager instance = BehaviourSingleton<EnergyManager>.instance;
		int ownedPlayCoins = instance.ownedPlayCoins;
		visibility.Clear();
		visibility.SetActive(widgetsToHide, isVisible: false);
		if (instance.isLimitedFreeEnergyActive)
		{
			infiniteEnergyCountStyle.Apply(visibility);
			GGUtil.ChangeText(livesText, GGFormat.FormatTimeSpan(instance.limitedEnergyTimespanLeft));
		}
		else if (ownedPlayCoins <= 0)
		{
			livesCountStyle.Apply(visibility);
			TimeSpan span = TimeSpan.FromSeconds(BehaviourSingleton<EnergyManager>.instance.secToNextCoin);
			GGUtil.ChangeText(livesText, GGFormat.FormatTimeSpan(span));
		}
		else if (instance.isFullLives)
		{
			fullLivesCountStyle.Apply(visibility);
			GGUtil.ChangeText(heartsLivesText, ownedPlayCoins.ToString());
			GGUtil.ChangeText(livesText, "Full");
		}
		else
		{
			livesCountStyle.Apply(visibility);
			GGUtil.ChangeText(livesText, ownedPlayCoins.ToString());
		}
		visibility.Complete();
	}

	public IEnumerator CheckLivesRegeneration()
	{
		return new _003CCheckLivesRegeneration_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}
}
