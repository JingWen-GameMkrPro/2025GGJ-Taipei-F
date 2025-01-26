using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static ItemManager;

public class ItemBehavior: MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D RB;

    [HideInInspector]
    public ItemManager.ItemInfo ItemInfo;

    [HideInInspector]
    public Animator Animator;

    public GameObject BoomEffect;

    public float LifeTime = 3f;
    float currentLifeTime = 0f;

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime >= LifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetAnimatorIndex(int index)
    {
        Animator.SetInteger("Index", index);
    }

    //碰到尖刺會爆炸
    List<GameObject> boomRangePlayers = new();
    public void Boom()
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Explosion);
        SetAnimatorIndex(1);
        RB.linearVelocity = Vector2.zero;

        var effect = Instantiate(BoomEffect);
        effect.transform.position = this.transform.position;
        effect.GetComponent<ParticleSystem>().Play();

        foreach (GameObject player in boomRangePlayers)
        {
            var types = new List<ItemManager.InteractType>();
            types.Add(ItemManager.InteractType.Boom);
            player.GetComponent<Player_HitSensor>().controller.TriggerHit(types, ItemInfo.PlayerIndex);
        }

        Destroy(this.gameObject);
    }

    public void Bounce()
    {
        //反彈
        RB.linearVelocity = -RB.linearVelocity; 
    }
    public void Block()
    {
        Destroy(this.gameObject);
    }

    public void Reback(PlayerController target)
    {
        ItemInfo.PlayerIndex = target.GetData().index;
        ChangeColor(ItemInfo.PlayerIndex);

        RB.linearVelocity = -RB.linearVelocity;
    }

    public void SpeedDown(PlayerController target)
    {
        var types = new List<ItemManager.InteractType>();
        types.Add(ItemManager.InteractType.SpeedDown);
        types.Add(ItemManager.InteractType.Damage);
        target.TriggerHit(types, ItemInfo.PlayerIndex);
        SoundManager.Instance.PlaySoundEffect(SoundEffectType.Stick);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Player")
        {
            Debug.Log("awsdaw");
            boomRangePlayers.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionTag = collision.gameObject.tag;

        //！！確認玩家專屬Tag
        if (collisionTag == "Player")
        {
            boomRangePlayers.Remove(collision.gameObject);
        }
    }


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        LifeTime = 3f;
    }


    public void Invoke(ItemManager.ItemInfo itemInfo)
    {
        ChangeColor(itemInfo.PlayerIndex);

        this.transform.position = itemInfo.Position;
        ItemInfo = itemInfo;

        Vector2 velocity = ItemInfo.Direction.normalized * ItemInfo.Speed;
        RB.linearVelocity = velocity;
    }

    public void ChangeColor(int index)
    {
        var renderer = this.gameObject.GetComponent<SpriteRenderer>();

        switch(index)
        {
            case 0:
                renderer.color = new UnityEngine.Color(1f, 0f, 0f, 1f); 
                break;
            case 1:
                renderer.color = new UnityEngine.Color(0f, 1f, 0f, 1f);
                break;
            case 2:
                renderer.color = new UnityEngine.Color(0f, 0f, 1f, 1f);
                break;
            case 3:
                renderer.color = new UnityEngine.Color(1f, 1f, 0f, 1f);
                break;
            default:
                break;
        }
    }
}
