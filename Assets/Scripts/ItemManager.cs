using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        Bubble = 0
    }

    List<(GameObject, ItemBehavior, ItemType)> AllItems = new();

    public GameObject BubblePrefab;

    public GameObject Create(ItemType type)
    {
        switch(type)
        {
            case ItemType.Bubble:
                var instance = Instantiate(BubblePrefab);
                AllItems.Add((instance, instance.GetComponent<ItemBehavior>(), ItemType.Bubble));
                return instance;
            default:
                return null;
        }
    }


    public void Invoke(GameObject target)
    {
        foreach (var item in AllItems)
        {
            if(item.Item1 == target)
            {
                Debug.Log($"{target.name} is in the list.");
                item.Item2.Shoot(Vector2.right, 10f);
                return;
            }
        }

        Debug.LogError("This Item is not existing¡I");
    }
}
