using Utility;

namespace GamePlay
{
	public interface IBattleManager : ISubject<TimeInfo>
	{
		void StartBattle();
		void PickUpItem(int playerIndex, ItemManager.ItemType type);
		void UseItem(int playerIndex, ItemManager.ItemInfo itemInfo);
		void DoDamage(int attackerIndex, int playerIndex, int damage);
	}
}