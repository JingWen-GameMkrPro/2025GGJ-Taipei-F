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
    public enum InteractType
    {
        Boom = 0,
        SpeedUp = 1,
    }
    public struct ItemInfo
    {
        public int PlayerIndex;
        public GameObject Owner;
        public  Vector2 Position;
        public  Vector2 Direction;
        public  float Speed;
    }

    List<Queue<ItemType>> PlayerItems = new List<Queue<ItemType>>();

    public GameObject BubbleIconPrefab;
    public GameObject BubblePrefab;


    //�t�Υͦ�Icon�A�i�H���wType
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

    //�D�㥻���q���D��޲z�̪��a�߬B�D��
    public void PickUpItem(int playerIndex , ItemType type)
    {
        PlayerItems[playerIndex].Enqueue(type);
    }


    //���a�����D�ʨϥιD��A���i���X
    public void UseItem(ItemInfo itemInfo)
    {
        if(PlayerItems.Count > 0)
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
    }
}

//