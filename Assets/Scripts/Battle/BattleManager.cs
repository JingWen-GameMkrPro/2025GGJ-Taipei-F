using System.Collections.Generic;
using Utility;

namespace GamePlay
{
	public class BattleManager : IBattleManager
	{
		private ITimerManager _timerManager;
		private Timer _battleCountdown;
		private HashSet<IObserver<TimeInfo>> _countdownObservers = new HashSet<IObserver<TimeInfo>>();
		public BattleManager(ITimerManager timerManager)
		{
			_timerManager = timerManager;
			_battleCountdown = timerManager.GetTimer();
			_battleCountdown.OnUpdate += OnCountdownUpdate;
		}
		public void StartBattle()
		{
			_battleCountdown.StartCountdown(99);
		}

		private void OnCountdownUpdate(float countdown)
		{
			foreach (var observer in _countdownObservers)
			{
				observer.Update(new TimeInfo() { Countdown = countdown });
			}
		}

		public void Register(IObserver<TimeInfo> observer)
		{
			_countdownObservers.Add(observer);
		}
		public void Deregister(IObserver<TimeInfo> observer)
		{
			_countdownObservers.Remove(observer);
		}
		public void PickUpItem(int playerIndex, ItemManager.ItemType type)
		{
			throw new System.NotImplementedException();
		}

		public void UseItem(ItemManager.ItemInfo itemInfo)
		{
			throw new System.NotImplementedException();
		}
	}
}