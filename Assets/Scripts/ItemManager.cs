using GamePlay;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Utility;

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
    public enum InteractType
    {
        Boom = 0,
        SpeedUp = 1,
        SpeedDown = 2,
        Damage = 3,
    }
    public struct ItemInfo
    {
        public int PlayerIndex;
        public GameObject Owner;
        public  Vector2 Position;
        public  Vector2 Direction;
        public  float Speed;
    }

    Queue<ItemType>[] PlayerItems = new Queue<ItemType>[4]
    {
        new Queue<ItemType>(),
        new Queue<ItemType>(),
        new Queue<ItemType>(),
        new Queue<ItemType>()
    };

    public GameObject BubbleIconPrefab;
    public GameObject BubblePrefab;

    //系統生成Icon，可以指定Type
    public GameObject CreateItemIconInMap(ItemType type)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Emerge);
        switch(type)
        {
            case ItemType.Bubble:
                return Instantiate(BubbleIconPrefab);
            default:
                return null;
        }
    }

    //道具本身通知道具管理者玩家撿拾道具
    public void PickUpItem(int playerIndex , ItemType type)
    {
        PlayerItems[playerIndex].Enqueue(type);
        if (SystemService.TryGetService<IBattleManager>(out IBattleManager battleManager) == false)
        {
            return;
        }
        //_pController.GetPlayerAnimation().PlayHurtAnimation();
        battleManager.PickUpItem(playerIndex, type);

    }


    //玩家本身主動使用道具，先進先出
    public void UseItem(ItemInfo itemInfo)
    {
        if (PlayerItems[itemInfo.PlayerIndex].Count > 0)
        {
            var item = PlayerItems[itemInfo.PlayerIndex].Dequeue();
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

        //var instance = Instantiate(BubblePrefab);
        //instance.GetComponent<ItemBehavior>().Invoke(itemInfo);
        //SoundManager.Instance.PlaySoundEffect(SoundEffectType.Shoot);
    }

    public int GetPlayerBubbleCount(int playerIndex)
    {
        return PlayerItems[playerIndex].Count;
    }

    public void ResetAllPlayerBubble()
    {
        PlayerItems = new Queue<ItemType>[4]
        {
                new Queue<ItemType>(),
                new Queue<ItemType>(),
                new Queue<ItemType>(),
                new Queue<ItemType>()
        };

    }
    public void ResetPlayerBubble(int index)
    {
        PlayerItems[index] = new Queue<ItemType>();
    }



}

//