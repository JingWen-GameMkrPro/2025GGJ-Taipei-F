using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace GamePlay
{
	public partial class MatchManager : IMatchManager, IMatchHelper
	{
		//TODO: 最低開賽人數，2個?
		//TODO: 人數上限
		private readonly int MinPlayerCount = 2;
		private Dictionary<int, PlayerData> _playerDict = new Dictionary<int, PlayerData>();
		private MatchErrorHandler _errorHandler = new MatchErrorHandler();
		private IStateManager _stateManager;
		//private List<IObserver<PlayerInfo>> _infoObservers;

		public MatchManager(IStateManager stateManager)
		{
			_stateManager = stateManager;
			_stateManager.GetState<MatchState>().OnEnterState += Init;
		}

		public int GeneratePlayerID()
		{
			return _playerDict.Count;
		}

		public void Join(PlayerData playerData)
		{
			if (_playerDict.Values.Contains(playerData))
			{
				_errorHandler.Handle(MatchResult.AlreadyJoin);
				return;
			}
			//TODO: 優化Ticket生成
			var playerID = GeneratePlayerID();
			if (_playerDict.TryAdd(playerID, playerData))
			{
				//成功時再刷新資料
				playerData.index = playerID;
				//TODO: 要做配對狀態的檢查
				playerData.matchStatus = MatchStatus.NotReady;
				NotifyJoin(playerID);
				return;
			}
			_errorHandler.Handle(MatchResult.JoinFail);

		}

		public void Ready(int index)
		{
			var playerID = index;
			if (!_playerDict.TryGetValue(playerID, out var ticket))
			{
				_errorHandler.Handle(MatchResult.NotJoin);
				return;
			}
			switch (ticket.matchStatus)
			{
				case MatchStatus.NotReady:
					ticket.matchStatus = MatchStatus.Ready;
					break;
				case MatchStatus.Ready:
					_errorHandler.Handle(MatchResult.AlreadyReady);
					break;
				default:
					_errorHandler.Handle(MatchResult.ReadyFail);
					break;
			}
		}

		public void Start()
		{
			if (_stateManager.CurrentState != _stateManager.GetState<MatchState>())
			{
				_errorHandler.Handle(MatchResult.BattleIsStarted);
				return;
			}
			if (_playerDict.Count < MinPlayerCount)
			{
				_errorHandler.Handle(MatchResult.PlayerNotEnough);
				return;
			}
			var isNotReady = _playerDict.Values.Any(data => data.matchStatus != MatchStatus.Ready);

			if (isNotReady)
			{
				_errorHandler.Handle(MatchResult.PlayerNotReady);
				return;
			}

			_stateManager.ChangeState<BattleState>();
			//TODO: 進入遊戲
			SystemService.TryGetService<IBattleManager>(out var battleManager);
			battleManager.StartBattle(_playerDict);
		}
		private void Init()
		{
			_playerDict.Clear();
			_joinObservers.Clear();
			Debug.Log("Init MathManager");
		}
	}
}
