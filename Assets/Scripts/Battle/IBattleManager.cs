using Utility;

namespace GamePlay
{
	public interface IBattleManager : ISubject<TimeInfo>
	{
		void StartBattle();
		void PickUpItem(int playerIndex, ItemManager.ItemType type);
		void UseItem(ItemManager.ItemInfo itemInfo);
	}
}