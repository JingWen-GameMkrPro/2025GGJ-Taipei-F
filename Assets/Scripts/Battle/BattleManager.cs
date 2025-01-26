using System.Collections.Generic;
using UnityEngine;
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
		private ICoordinateAdapter _coordinateAdapter;
		private Timer _battleCountdown;
		private Timer _bullectSpawnCountdown;
		private Dictionary<int, PlayerData> _playerDict;
		private Dictionary<int, PlayerInfo> _playerBattleInfos = new Dictionary<int, PlayerInfo>();
		private Dictionary<int, int> _killCount = new Dictionary<int, int>();
		//private Dictionary<int, Timer> _playerRespawnTimers = new Dictionary<int, Timer>();
		private List<Timer> _playerRespawnTimers = new List<Timer>();
		private HashSet<IObserver<TimeInfo>> _countdownObservers = new HashSet<IObserver<TimeInfo>>();
		private List<IObserver<PlayerInfo>> _playerInfoObservers = new List<IObserver<PlayerInfo>>();
		public BattleManager(IStateManager stateManager, ITimerManager timerManager)
		{
			_stateManager = stateManager;
			_timerManager = timerManager;
			_battleCountdown = timerManager.GetTimer();

			_stateManager.GetState<BattleState>().OnEnterState += OnEnterBattle;
			_battleCountdown.OnUpdate += OnCountdownUpdate;

			_bullectSpawnCountdown = timerManager.GetTimer();

			_bullectSpawnCountdown.OnUpdate += OnBullectSpawnCountdown;

			_playerRespawnTimers.Add(timerManager.GetTimer());
			_playerRespawnTimers.Add(timerManager.GetTimer());
			_playerRespawnTimers.Add(timerManager.GetTimer());
			_playerRespawnTimers.Add(timerManager.GetTimer());
            for (int i = 0; i < _playerRespawnTimers.Count; i++)
            {
				var index = i;
				_playerRespawnTimers[index].OnUpdate += (deltaTime) => PlayerRespawnCountdown(index, deltaTime);
			}
		}

        private void OnBullectSpawnCountdown(float totalTime)
        {
			if (totalTime <= 0 && _stateManager.GetState<BattleState>() == _stateManager.CurrentState)
			{
				var bubble = ItemManager.Instance.CreateItemIconInMap(ItemManager.ItemType.Bubble);
				var count = Random.Range(1, 4);
                for (int i = 0; i < count; i++)
                {
					bubble.transform.position = _coordinateAdapter.GetPosition(Random.Range(0, 19), Random.Range(0, 6));
                }
				_bullectSpawnCountdown.StartCountdown(3);
			}
        }

        private void OnEnterBattle()
		{
			_bullectSpawnCountdown.StartCountdown(3);
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

				_playerBattleInfos[playerID] = new PlayerInfo()
				{
					//TODO: magic number
					currentHealth = playerData.hp,
					ammo = 0,
					kill = 0
				};
				//這裡會有問題，需要解註冊
				//_playerRespawnTimers[playerID].OnUpdate += (daltaTime) => { PlayerRespawnCountdown(playerID, daltaTime); };
			}
			
			_battleCountdown.StartCountdown(99);
		}

		private void PlayerRespawnCountdown(int playerID, float countdown)
		{
			if (countdown <= 0)
			{
				var playerData = _playerDict[playerID];
				playerData.Respawn();
				playerData.playerController.UpdatePosition(_coordinateAdapter.GetPosition(0, 0));
			}
		}

		private void OnCountdownUpdate(float countdown)
		{
			foreach (var observer in _countdownObservers)
			{
				observer.Update(new TimeInfo() { Countdown = countdown });
			}
			if (countdown <= 0)
			{
				_stateManager.ChangeState<ResultState>();
			}
		}

		public void DoDamage(int attackerIndex, int playerIndex, int damage)
		{
			var playerData = _playerDict[playerIndex];
			playerData.ModifyHP(damage);
			if (playerData.IsDied())
			{
				_killCount.TryGetValue(attackerIndex, out var killCount);
				killCount++;
				_killCount[attackerIndex] = killCount;

				_playerRespawnTimers[playerIndex].StartCountdown(3);
			}

			UpdatePlayerInfo();
		}

		public void UpdatePlayerInfo()
		{
			foreach (var playerData in _playerDict.Values)
			{
				foreach (var observers in _playerInfoObservers)
				{
					observers.Update(GetPlayerInfo(playerData));
				}
			}
			
		}

		private PlayerInfo GetPlayerInfo(PlayerData playerData)
		{
			var info = new PlayerInfo();
			info.index = playerData.index;
			info.currentHealth = playerData.hp;
			info.ammo = playerData.ammo;
			_killCount.TryGetValue(playerData.index, out var killCount);
			info.kill = killCount;
			return info;
		}

		public void SetCoordinateAdapter(ICoordinateAdapter coordinateAdapter)
		{
			_coordinateAdapter = coordinateAdapter;
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
		public void Register(IObserver<PlayerInfo> observer)
		{
			_playerInfoObservers.Add(observer);
		}
		public void Deregister(IObserver<PlayerInfo> observer)
		{
			_playerInfoObservers.Remove(observer);
		}
	}
}