using System.Collections.Generic;
using System.Linq;

namespace GamePlay
{
	public class MatchManager : IMatchManager, IMatchHelper
	{
		//TODO: 最低開賽人數，2個?
		private readonly int MinPlayerCount = 2;
		private Dictionary<int, MatchTicket<PlayerData>> _playerDict = new Dictionary<int, MatchTicket<PlayerData>>();
		private MatchErrorHandler _errorHandler = new MatchErrorHandler();
		private IStateManager _stateManager;

		public MatchManager(IStateManager stateManager)
		{
			_stateManager = stateManager;
		}
		public int GeneratePlayerID()
		{
			return _playerDict.Count;
		}

		public void Join(PlayerData playerData)
		{
			if (_playerDict.Values.Select(ticket => ticket.PlayerData).Contains(playerData))
			{
				_errorHandler.Handle(MatchResult.AlreadyJoin);
				return;
			}
			//TODO: 優化Ticket生成
			var playerID = GeneratePlayerID();
			if (_playerDict.TryAdd(playerID, new MatchTicket<PlayerData>() { PlayerData = playerData }))
			{
				//成功時再刷新資料
				playerData.index = playerID;
				return;
			}
			_errorHandler.Handle(MatchResult.JoinFail);

		}

		public void Ready(PlayerData playerData)
		{
			var playerID = playerData.index;
			if (!_playerDict.TryGetValue(playerID, out var ticket))
			{
				_errorHandler.Handle(MatchResult.NotJoin);
				return;
			}
			switch (ticket.MatchStatus)
			{
				case MatchStatus.NotReady:
					ticket.MatchStatus = MatchStatus.Ready;
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
			if (_stateManager.GameState != GameState.Match)
			{
				_errorHandler.Handle(MatchResult.BattleIsStarted);
				return;
			}
			if (_playerDict.Count < MinPlayerCount)
			{
				_errorHandler.Handle(MatchResult.PlayerNotEnough);
				return;
			}
			var isNotReady = _playerDict.Values.Any(data => data.MatchStatus != MatchStatus.Ready);

			if (isNotReady)
			{
				_errorHandler.Handle(MatchResult.PlayerNotReady);
				return;
			}

			_stateManager.GameState = GameState.Battle;
			//TODO: 進入遊戲
		}
	}
}
