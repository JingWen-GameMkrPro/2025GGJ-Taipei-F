using System.Collections.Generic;
using Utility;

namespace GamePlay
{
	public struct PlayerBattleInfo
	{
		public int Hp;
		public int BulletCount;
		public int KillCount;
	}
	public class BattleManager : IBattleManager
	{
		private IStateManager _stateManager;
		private ITimerManager _timerManager;
		private Timer _battleCountdown;
		private HashSet<IObserver<TimeInfo>> _countdownObservers = new HashSet<IObserver<TimeInfo>>();
		public BattleManager(IStateManager stateManager, ITimerManager timerManager)
		{
			_stateManager = stateManager;
			_timerManager = timerManager;
			_battleCountdown = timerManager.GetTimer();

			_stateManager.GetState<BattleState>().OnEnterState += OnEnterBattle;
			_battleCountdown.OnUpdate += OnCountdownUpdate;
		}

        private void OnEnterBattle()
        {
			StartBattle();
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

		public void DoDamage(int attackerIndex, int playerIndex, int damage)
		{
			throw new System.NotImplementedException();
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
        public void UseItem(int playerIndex, ItemManager.ItemInfo itemInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}