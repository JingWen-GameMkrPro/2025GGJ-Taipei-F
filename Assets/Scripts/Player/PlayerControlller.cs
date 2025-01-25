using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    PlayerData data;

    //TEMP
    static int indexCount = 0;


    [Header("Input")]
    PlayerInput playerInput;

    [Header("Character")]
    public Rigidbody2D rb;
    public CharacterController controller;
    public SpriteRenderer spriteRenderer;
    public List<Material> playerMat; 

    #region Self
    [SerializeField]
    Vector2 move = Vector2.zero;
    #endregion 

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

        data = new PlayerData(playerInput.devices[0], indexCount++, this);

        spriteRenderer.material = playerMat[data.index];

        //Regis

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        HandleMove();
    }

    void HandleMove() {
        if (data.canMove == false) {
            return;
        }
        Vector2 finalMov = new Vector2(move.x, move.y);
        float x = finalMov.x;
        float y = finalMov.y;

        //斜方處理
        if (x + y > 1 || x + y < -1 ||
            (x > 0.1f && y < -0.1f) ||
            (x < -0.1f && y > 0.1f)) {

            finalMov.x = x * Mathf.Sqrt(0.5f);
            finalMov.y = y * Mathf.Sqrt(0.5f); 
        }

        if (controller != null) {
            controller.Move(finalMov);
        }
    }

    public void DoMove(InputAction.CallbackContext ctx) {
        move = ctx.ReadValue<Vector2>();
        move = move * Time.deltaTime * data.speed;
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

    public PlayerData GetData() {
        return data;
    }
}

public class PlayerData{
    public int index;
    public InputDevice device;
    public int hp;
    public int speed;
    public bool canMove;

    //GameObject
    public PlayerController playerController;

    public PlayerData(InputDevice device, int index, PlayerController playerController) {
        this.device = device;
        this.index = index;
        this.hp = 100;
        this.speed = 5;
        this.canMove = true;
        this.playerController = playerController;
    }
}