using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    PlayerData data;

    //TEMP
    static int indexCount = 0;


    [Header("Input")]
    PlayerInput playerInput;

    [Header("Character")]
    public Rigidbody2D rb;
    public CharacterController controller;
    public SpriteRenderer spriteRenderer;

    private void OnEnable() {
        Setup();
    }

    void Setup() {
        playerInput = GetComponent<PlayerInput>();

        Debug.Log(playerInput.devices[0].GetType());
        if (playerInput.devices[0].GetType().ToString().Equals("")) {
            Destroy(gameObject);
            return;
        }

        data = new PlayerData(playerInput.devices[0], indexCount++);

        //TEMP
        List<Color> colors = new List<Color> { Color.black, Color.cyan, Color.gray, Color.blue};
        spriteRenderer.color = colors[data.index];

        //Regis
    }

    public void DoMove(InputAction.CallbackContext ctx) {
        Vector2 move = ctx.ReadValue<Vector2>();
        Debug.Log(move * Time.deltaTime * data.speed);

        move = move.normalized * data.speed * Time.deltaTime;
        if (controller != null) {
            controller.Move(move);
        }
        if (rb != null) {
            rb.MovePosition(move);
        }

    }

    public void DoConFirm(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            StopVibrate();
            return;
        }
        Debug.Log("DOConfrim");

        TrggerVibrate(0.1f, 0.5f);
    }

    void TrggerVibrate(float low, float high) {
        if (Gamepad.current != null) {
            //Gamepad.current.SetMotorSpeeds(low, high);
        }
    }

    void StopVibrate() {
        if (data.device != null) {
            //data.device.ResetHaptics();
        }
    }
}

public class PlayerData{
    public int index;
    public InputDevice device;
    public int hp;
    public int speed;

    public PlayerData(InputDevice device, int index) {
        this.device = device;
        this.index = index;
        this.hp = 100;
        this.speed = 10;
    }
}