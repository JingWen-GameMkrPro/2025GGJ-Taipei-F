using GamePlay;
using System.Collections.Generic;
using UnityEngine;
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

    //�t�Υͦ�Icon�A�i�H���wType
    public GameObject CreateItemIconInMap(ItemType type)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Spawn);
        switch(type)
        {
            case ItemType.Bubble:
                return Instantiate(BubbleIconPrefab);
            default:
                return null;
        }
    }

    //�D�㥻���q���D��޲z�̪��a�߬B�D��
    public void PickUpItem(int playerIndex , ItemType type)
    {
        PlayerItems[playerIndex].Enqueue(type);
        if(SoundManager.Instance != null){
            SoundManager.Instance.PlaySoundEffect(SoundEffectType.Pickup);
        }
        if (SystemService.TryGetService<IBattleManager>(out IBattleManager battleManager) == false)
        {
            return;
        }
        //_pController.GetPlayerAnimation().PlayHurtAnimation();
        battleManager.PickUpItem(playerIndex, type);

    }


    //���a�����D�ʨϥιD��A���i���X
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
                    if(SoundManager.Instance != null){
                        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Shoot);
                    }
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