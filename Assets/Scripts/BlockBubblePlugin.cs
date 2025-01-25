using UnityEngine;

public class BlockBubblePlugin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Bubble")
        {
            //讓他爆炸
            collision.gameObject.GetComponent<ItemBehavior>().Block();
        }
    }
}
