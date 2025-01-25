using UnityEngine;

public class BubbleRebackDetector : MonoBehaviour
{
    ItemBehavior behavior;

    private void Awake()
    {
        behavior = this.gameObject.transform.parent.GetComponent<ItemBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Player")
        {
            //是撞到別人
            if(collision.gameObject.GetComponent<PlayerController>().GetData().index != behavior.ItemInfo.PlayerIndex)
            {
                if(collision.gameObject.GetComponent<PlayerController>().IsDefencing())
                {
                    behavior.Reback(collision.gameObject.GetComponent<PlayerController>().GetData().index);
                }
            }
        }
    }
}
