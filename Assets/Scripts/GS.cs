using UnityEngine;

public class GS: MonoBehaviour
{
    private void Start()
    {
        //e.g. 系統生成道具
        ItemManager.Instance.CreateItemIconInMap(ItemManager.ItemType.Bubble);

        ItemManager.Instance.PickUpItem(0, ItemManager.ItemType.Bubble);

        //e.g. 玩家或系統使用道具
        var itemInfo = new ItemManager.ItemInfo();
        itemInfo.Owner = this.gameObject;
        itemInfo.Position = new Vector2(0, 0);
        itemInfo.Direction = Vector2.right;
        itemInfo.Speed = 20;
        ItemManager.Instance.UseItem(0, itemInfo);
    }
}
