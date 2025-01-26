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
		private Dictionary<int, PlayerData> _playerDict;
		private Dictionary<int, PlayerBattleInfo> _playerBattleInfos = new Dictionary<int, PlayerBattleInfo>();
		private Dictionary<int, Timer> _playerRespawnTimers = new Dictionary<int, Timer>();
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
			//StartBattle();
		}


		public void StartBattle(Dictionary<int, PlayerData> playerTable)
		{
			_playerDict = playerTable;
			_playerBattleInfos.Clear();
			_playerRespawnTimers.Clear();

			foreach (var playerKeyValue in playerTable)
			{
				var playerID = playerKeyValue.Key;
				var playerData = playerKeyValue.Value;
				playerData.matchStatus = MatchStatus.Battle;

				_playerBattleInfos[playerID] = new PlayerBattleInfo()
				{
					//TODO: magic number
					Hp = playerData.hp,
					BulletCount = 0,
					KillCount = 0
				};
				//_playerRespawnTimers[playerID] = _timerManager.GetTimer();
				//這裡會有問題，需要解註冊
				//_playerRespawnTimers[playerID].OnUpdate += (daltaTime) => { PlayerRespawnCountdown(playerID, daltaTime); };
			}
			
			_battleCountdown.StartCountdown(99);
		}

		private void PlayerRespawnCountdown(int playerID, float countdown)
		{
			if (countdown <= 0)
			{

			}
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
			var playerData = _playerDict[playerIndex];
			playerData.ModifyHP(damage);
			if (playerData.IsDied())
			{
				//TODO Add Kill Count
				//TODO respawn
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
		public void UseItem(int playerIndex, ItemManager.ItemInfo itemInfo)
		{
			throw new System.NotImplementedException();
		}
	}
}