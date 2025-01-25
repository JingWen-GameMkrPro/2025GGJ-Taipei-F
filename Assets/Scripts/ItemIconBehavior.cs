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

        //�I�I�T�{���a�M��Tag
        if (collisionTag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().GetData().index
            //�q�����a�A�çi���ۨ����O
            ItemManager.Instance.PickUpItem(collision.gameObject.GetComponent<Player_HitSensor>().controller.GetData().index, itemType);

            //���ʵe�A�B�~�S��

            //�S�w��ƫ�A�ۨ��D�����
            Destroy(this);
        }
    }
}
