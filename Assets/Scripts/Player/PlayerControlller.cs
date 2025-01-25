using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    PlayerData data;

    //TEMP
    static int indexCount = 0;


    [Header("Input")]
    PlayerInput playerInput;

    [Header("Character")]
    [SerializeField] CharacterController controller;
    [SerializeField] List<GameObject> characterList;
    [SerializeField] PlayerAnimation aniControl;

    #region Self
    [SerializeField]
    Vector2 move = Vector2.zero;

    [SerializeField]
    Vector2 faceto = Vector2.left;

    [SerializeField]
    long lastDefenceTick = 0;
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

        //spriteRenderer.material = playerMat[data.index];
        for (int i = 0; i < characterList.Count; i++) {
            characterList[i].SetActive(i == data.index);
        }

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

            x = x * Mathf.Sqrt(0.5f);
            y = y * Mathf.Sqrt(0.5f); 
        }

        finalMov.x = x;
        finalMov.y = y;

        if (controller != null) {
            controller.Move(finalMov);
        }

        //face where
        //優先上下
        //再左右
        int faceType = -1;
        bool horizonMove = Mathf.Abs(x) >= Mathf.Abs(y);
        if (horizonMove) {
            if (x >= 0.001f) {
                faceto = Vector2.right;
                faceType = 2;
            } else if (x <= -0.001f) {
                faceto = Vector2.left;
                faceType = 3;
            }
        } else {
            if (y >= 0.001f) {
                faceto = Vector2.up;
                faceType = 0;
            } else if (y <= -0.001f) {
                faceto = Vector2.down;
                faceType = 1;
            }
        }

        aniControl.UpdateState(finalMov, faceType);
    }

    public void DoMove(InputAction.CallbackContext ctx) {
        move = ctx.ReadValue<Vector2>();
        move = move * Time.deltaTime * data.speed;
    }

    public void DoAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            StopVibrate();
            return;
        }
        Debug.Log("DoAttack");
        TrggerVibrate(0.1f, 0.5f);
    }

    public void DoDefence(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            StopVibrate();
            return;
        }
        Debug.Log("DoDefence");

        lastDefenceTick = System.DateTime.Now.Ticks;
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

    public bool IsDefencing() {
        // 1 tick = 0.0000001 sec
        // 1000000 ticks = 0.1 sec
        long safeDefenceTime = 1000000;
        return System.DateTime.Now.Ticks - lastDefenceTick < safeDefenceTime;
    }

    public void TriggerHit(List<ItemManager.InteractType> typeList) {

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