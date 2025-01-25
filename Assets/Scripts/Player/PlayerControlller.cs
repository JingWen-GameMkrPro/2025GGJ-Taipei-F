using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

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

    [Header("0 up 1 down 2 right 3 left")]
    [SerializeField] List<Transform> firePoints;

    [Header("Hint")]
    [SerializeField] GameObject defenceHint;
    [SerializeField] GameObject dieHint;

    #region Self
    [SerializeField]
    Vector2 move = Vector2.zero;

    [SerializeField]
    Vector2 faceto = Vector2.left;

    [SerializeField]
    int faceType = 3; //0 up 1 down 2 right 3 left

    [SerializeField]
    long lastDefenceTick = 0;

    [SerializeField]
    bool isInitlized = false;
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
        StartCoroutine(Finish());
    }

    IEnumerator Finish() {
        yield return new WaitForSecondsRealtime(0.5f);
        isInitlized = true;
    }

    private void Update() {
        HandleMove();
        HandleHint();
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

    void HandleAttack() {
        ItemManager.ItemInfo info = new ItemManager.ItemInfo();
        info.PlayerIndex = data.index;
        info.Owner = gameObject;
        info.Position = firePoints[faceType].position;
        info.Direction = faceto;
        info.Speed = 10;
        ItemManager.Instance.UseItem(info);
    }

    void HandleDamage(int v) {
        data.ModifyHP(v);
    }

    void HandleSpeedModify(int v) {
        data.ModifySpeed(v);
    }

    void HandleHint() {
        if (defenceHint != null) {
            defenceHint.SetActive(IsDefencing());
        }
        if (dieHint != null) {
            dieHint.SetActive(data.IsDied());
        }
    }

    #region Input

    public void DoMove(InputAction.CallbackContext ctx) {
        if (data.IsDied() || !data.canMove || !isInitlized) {
            return;
        }
        move = ctx.ReadValue<Vector2>();
        move = move * Time.deltaTime * data.speed;
    }

    public void DoAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            return;
        }
        if (data.IsDied() || !isInitlized) {
            return;
        }
        Debug.Log("DoAttack");
        HandleAttack();
    }

    public void DoDefence(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            return;
        }
        if (data.IsDied() || !isInitlized) {
            return;
        }
        Debug.Log("DoDefence");

        lastDefenceTick = System.DateTime.Now.Ticks;
    }

    #endregion

    //void TrggerVibrate(float low, float high) {
    //    if (Gamepad.current != null) {
    //        //Gamepad.current.SetMotorSpeeds(low, high);
    //    }
    //}

    //void StopVibrate() {
    //    if (data.device != null) {
    //        //data.device.ResetHaptics();
    //    }
    //}

    #region Public Functon
    public PlayerData GetData() {
        return data;
    }

    public bool IsDefencing() {
        if (data.IsDied() || !isInitlized) {
            return false;
        }
        // 1 tick = 0.0000001 sec
        // 1000000 ticks = 0.1 sec
        long safeDefenceTime = 10000000;
        return System.DateTime.Now.Ticks - lastDefenceTick < safeDefenceTime;
    }

    public void TriggerHit(List<ItemManager.InteractType> typeList) {
        if (data.IsDied() || !isInitlized) {
            return;
        }
        foreach (ItemManager.InteractType type in typeList) {
            switch (type) {
                case ItemManager.InteractType.Boom:
                    HandleDamage(10);
                    break;
                case ItemManager.InteractType.SpeedDown:
                    HandleSpeedModify(-2);
                    break;
                case ItemManager.InteractType.SpeedUp:
                    HandleSpeedModify(2);
                    break;
            }
        }
    }

    #endregion
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

    public void ModifyHP(int v) {
        hp += v;
        if (hp <= 0) {
            hp = 0;
        }
    }
    public void ModifySpeed(int v) {
        speed += v;
        if (speed <= 0) {
            speed = 0;
        }
    }

    public bool IsDied() {
        return hp <= 0;
    }
}