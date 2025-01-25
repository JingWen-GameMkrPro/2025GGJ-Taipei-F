using UnityEngine;

public class BouncePlugin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //�I�I�T�{���a�M��Tag
        if (collisionTag == "Bubble")
        {
            //���L�z��
            collision.gameObject.GetComponent<ItemBehavior>().Bounce();
        }
    }
}
