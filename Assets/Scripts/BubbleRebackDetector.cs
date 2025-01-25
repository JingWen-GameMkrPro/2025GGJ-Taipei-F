using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BubbleRebackDetector : MonoBehaviour
{
    ItemBehavior behavior;

    private void Awake()
    {
        behavior = this.gameObject.transform.parent.GetComponent<ItemBehavior>();
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    var collisionTag = collision.gameObject.tag;

    //    //！！確認玩家專屬Tag
    //    if (collisionTag == "Player")
    //    {
    //        var controller = collision.gameObject.GetComponent<Player_HitSensor>().controller;

    //        //是撞到別人
    //        if (!controller.GetData().IsDied() && controller.GetData().index != behavior.ItemInfo.PlayerIndex)
    //        {
    //            if (controller.IsDefencing())
    //            {
    //                behavior.Reback(controller);
    //            }
    //            else
    //            {
    //                behavior.SpeedDown(controller);
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Player")
        {
            var controller = collision.gameObject.GetComponent<Player_HitSensor>().controller;

            //是撞到別人
            if (!controller.GetData().IsDied() && controller.GetData().index != behavior.ItemInfo.PlayerIndex)
            {
                if (controller.IsDefencing())
                {
                    behavior.Reback(controller);
                }
                else
                {
                    behavior.SpeedDown(controller);
                }
            }
            else if(!controller.GetData().IsDied() && controller.GetData().index == behavior.ItemInfo.PlayerIndex)
            {
                if (controller.IsDefencing())
                {
                    behavior.Reback(controller);
                }
            }
        }
    }
}
