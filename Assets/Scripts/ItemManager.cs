using System;
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
        Bubble = 0
    }

    //public GameObject ItemManagerPrefab;
    public GameObject BubblePrefab;

    public GameObject Create(ItemType type)
    {
        switch(type)
        {
            case ItemType.Bubble:
                return Instantiate(BubblePrefab);
            default:
                return null;
        }
    }

    //使用道具
    public void Invoke()
    {

    }
}
