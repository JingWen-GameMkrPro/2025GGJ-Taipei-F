using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //�t�Υͦ�Icon�A�i�H���wType
    public void CreateItemIconInMap(ItemType type)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Emerge);
        switch(type)
        {
            case ItemType.Bubble:
                Instantiate(BubbleIconPrefab);
                return;
            default:
                return;
        }
    }

    //�D�㥻���q���D��޲z�̪��a�߬B�D��
    public void PickUpItem(int playerIndex , ItemType type)
    {
        PlayerItems[playerIndex].Enqueue(type);
    }


    //���a�����D�ʨϥιD��A���i���X
    public void UseItem(ItemInfo itemInfo)
    {
        //if (PlayerItems[itemInfo.PlayerIndex].Count > 0)
        //{
        //    var item = PlayerItems[itemInfo.PlayerIndex].Dequeue();
        //    switch (item)
        //    {
        //        case ItemType.Bubble:
        //            var instance = Instantiate(BubblePrefab);
        //            instance.GetComponent<ItemBehavior>().Invoke(itemInfo);
        //            return;
        //        default:
        //            return;
        //    }
        //}

        var instance = Instantiate(BubblePrefab);
        instance.GetComponent<ItemBehavior>().Invoke(itemInfo);
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Shoot);
    }
}

//