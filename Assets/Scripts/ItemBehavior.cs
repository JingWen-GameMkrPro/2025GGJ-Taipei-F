using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static ItemManager;
using static UnityEngine.Rendering.DebugUI;

public class ItemBehavior: MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D RB;

    [HideInInspector]
    public ItemManager.ItemInfo ItemInfo;

    [HideInInspector]
    public Animator Animator;

    public void SetAnimatorIndex(int index)
    {
        Animator.SetInteger("Index", index);
    }

    //�I��y��|�z��
    List<GameObject> boomRangePlayers = new();
    public void Boom()
    {
        SetAnimatorIndex(1);
        RB.linearVelocity = Vector2.zero;

        foreach (GameObject player in boomRangePlayers)
        {
            //�I�s�ˮ`���a�禡
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //�I�I�T�{���a�M��Tag
        if (collisionTag == "Player")
        {
            boomRangePlayers.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //�I�I�T�{���a�M��Tag
        if (collisionTag == "Player")
        {
            boomRangePlayers.Remove(collision.gameObject);
        }
    }


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }


    public void Invoke(ItemManager.ItemInfo itemInfo)
    {
        ItemInfo = itemInfo;
        Vector2 velocity = ItemInfo.Direction.normalized * ItemInfo.Speed;
        RB.linearVelocity = velocity;
    }
}
