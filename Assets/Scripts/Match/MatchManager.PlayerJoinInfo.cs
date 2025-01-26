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
		private void NotifyJoin(int playerID)
		{
			var ticket = _playerDict[playerID];
			var index = ticket.index;
			if (index >= 0 && index < _joinObservers.Count)
			{
				var observer = _joinObservers[index];
				var joinInfo = GetPlayerJoinInfo(ticket);
				observer.Update(joinInfo);
			}
		}
		private PlayerJoinInfo GetPlayerJoinInfo(PlayerData ticket)
		{
			var joinInfo = new PlayerJoinInfo();
			//TODO: don't use magic number
			joinInfo.maxHealth = 3;
			return joinInfo;
		}
	}
}