using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBehavior: MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, float speed)
    {
        Vector2 velocity = direction.normalized * speed;
        rb.linearVelocity = velocity;
    }
}
