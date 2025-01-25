using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ItemManager : MonoBehaviour
{
    private ItemManager() { }

    private static ItemManager _instance;

    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameObject>("ItemManager").GetComponent<ItemManager>();
            }
            return _instance;
        }
    }

    public enum ItemType
    {
        Bubble = 0,
        SpeedUp = 1,
    }

    public struct ItemInfo
    {
        public GameObject Owner;
        public  Vector2 Position;
        public  Vector2 Direction;
        public  float Speed;
    }

    Queue<ItemType> PlayerItems = new Queue<ItemType>();


    public GameObject BubbleIconPrefab;
    public GameObject BubblePrefab;


    //系統生成Icon，可以指定Type
    public void CreateItemIconInMap(ItemType type)
    {
        switch(type)
        {
            case ItemType.Bubble:
                Instantiate(BubbleIconPrefab);
                return;
            default:
                return;
        }
    }

    //道具本身通知道具管理者玩家撿拾道具
    public void PickUpItem(ItemType type)
    {
        PlayerItems.Enqueue(type);
    }


    //玩家本身主動使用道具，先進先出
    public void UseItem(ItemInfo itemInfo)
    {
        if(PlayerItems.Count > 0)
        {
            var item = PlayerItems.Dequeue();
            switch (item)
            {
                case ItemType.Bubble:
                    var instance = Instantiate(BubblePrefab);
                    instance.GetComponent<ItemBehavior>().Invoke(itemInfo);
                    return;
                default:
                    return;
            }
        }
    }
}

//