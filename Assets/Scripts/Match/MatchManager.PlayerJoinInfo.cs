using System.Collections.Generic;
using Utility;

namespace GamePlay
{
	public partial class MatchManager
	{
		private List<IObserver<PlayerJoinInfo>> _joinObservers = new List<IObserver<PlayerJoinInfo>>();
		public void Register(IObserver<PlayerJoinInfo> observer)
		{
			_joinObservers.Add(observer);
		}
		public void Deregister(IObserver<PlayerJoinInfo> observer)
		{
			_joinObservers.Remove(observer);
		}
		private void NotifyJoin()
		{
            foreach (var playerData in _playerDict.Values)
            {
				foreach (var observer in _joinObservers)
				{
					var joinInfo = GetPlayerJoinInfo(playerData);
					observer.Update(joinInfo);
				}
			}
		}
		private PlayerJoinInfo GetPlayerJoinInfo(PlayerData ticket)
		{
			var joinInfo = new PlayerJoinInfo();
			joinInfo.index = ticket.index;
			//TODO: don't use magic number
			joinInfo.maxHealth = 100;
			joinInfo.isReady = ticket.matchStatus == MatchStatus.Ready;
			return joinInfo;
		}
	}
}