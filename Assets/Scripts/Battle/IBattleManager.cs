using System.Collections.Generic;
using Utility;

namespace GamePlay
{
	public interface IBattleManager : ISubject<TimeInfo>, ISubject<PlayerInfo>
	{
		void StartBattle(Dictionary<int, PlayerData> playerTable);
		void PickUpItem(int playerIndex, ItemManager.ItemType type);
		void UseItem(int playerIndex, ItemManager.ItemInfo itemInfo);
		void DoDamage(int attackerIndex, int playerIndex, int damage);
		void SetCoordinateAdapter(ICoordinateAdapter coordinateAdapter);
	}
}