using UnityEngine;

public class ItemIconBehavior : MonoBehaviour
{
    private ItemManager.ItemType itemType;

    private Rigidbody2D rb;

    private Animator animator;

    private int animationIndex = 0;

    public ItemIconBehavior(ItemManager.ItemType itemType) 
    { 
        this.itemType = itemType;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().GetData().index
            //通知玩家，並告知自身類別
            ItemManager.Instance.PickUpItem(collision.gameObject.GetComponent<Player_HitSensor>().controller.GetData().index, itemType);

            //更改動畫，額外特效

            //特定秒數後，自身道具消失
            Destroy(this);
        }
    }
}
