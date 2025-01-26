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

    //    //�I�I�T�{���a�M��Tag
    //    if (collisionTag == "Player")
    //    {
    //        var controller = collision.gameObject.GetComponent<Player_HitSensor>().controller;

    //        //�O����O�H
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

        //�I�I�T�{���a�M��Tag
        if (collisionTag == "Player")
        {
            var controller = collision.gameObject.GetComponent<Player_HitSensor>().controller;

            //�O����O�H
            if (!controller.GetData().IsDied() && controller.GetData().index != behavior.ItemInfo.PlayerIndex)
            {
                if (controller.GetData().IsDefencing() && controller.GetData().faceto == -behavior.RB.linearVelocity.normalized)
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
                if (controller.GetData().IsDefencing() && controller.GetData().faceto == -behavior.RB.linearVelocity.normalized)
                {
                    behavior.Reback(controller);
                }
            }
        }
    }

    //bool isPlayerFacingBubble(Vector2 playerDirection)
    //{
    //    switch

    //}
}
