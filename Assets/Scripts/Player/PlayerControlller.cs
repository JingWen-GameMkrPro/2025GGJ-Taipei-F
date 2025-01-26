using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using GamePlay;
using Utility;
using System;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    PlayerData data;
    bool lockUpdateMove = false;

    [Header("Input")]
    PlayerInput playerInput;

    [Header("Behavior")]
    IPlayerBehavior playerBehavior;

    [Header("Character")]
    [SerializeField] CharacterController controller;
    [SerializeField] List<GameObject> characterList;
    [SerializeField] PlayerAnimation aniControl;

    [Header("0 up 1 down 2 right 3 left")]
    [SerializeField] List<Transform> firePoints;

    [Header("Hint")]
    public GameObject defenceHint;
    public GameObject dieHint; 

    private void OnEnable() {
        Setup();
    }

    void Setup() {
        if(SystemService.TryGetService<IStateManager>(out IStateManager stateManager) == false){
            Destroy(gameObject);
            return;
        }
        if(stateManager.CurrentState != null && stateManager.CurrentState.GetType() == typeof(BattleState)){
            Destroy(gameObject);
            return;
        }

        playerInput = GetComponent<PlayerInput>();
        data = new PlayerData(playerInput.devices[0], -1, this);

        //Regis
        if(SystemService.TryGetService<IMatchManager>(out IMatchManager matchManager) == false){
            Destroy(gameObject);
            return;
        }
        matchManager.Join(data);
        
        //spriteRenderer.material = playerMat[data.index];
        for (int i = 0; i < characterList.Count; i++) {
            characterList[i].SetActive(i == data.index);
        }

        DontDestroyOnLoad(gameObject);
        StartCoroutine(Finish());
    }

    IEnumerator Finish() {
        yield return new WaitForSecondsRealtime(0.5f);
        data.isInitlized = true;
    }

    private void Update() {
        HandleHint();
        if(lockUpdateMove == false){
            HandleMove();
        }
    }

    void UpdateBehavior(){
        switch(data.matchStatus){
            case MatchStatus.NotReady:
            case MatchStatus.Ready:
                playerBehavior = new PlayerLobbyBehavior();
                break;
            case MatchStatus.Battle:
                playerBehavior = new PlayerGameBehavior();
                break;
        }
    }

    void HandleMove() {
        UpdateBehavior();
        playerBehavior.HandleMove(this);
    }

    void HandleAttack() {
        UpdateBehavior();
        playerBehavior.HandleAttack(this);
    }

    void HandleHint() {
        UpdateBehavior();
        playerBehavior.HandleHint(this);
    }

    #region Input

    public void DoMove(InputAction.CallbackContext ctx) {
        if (data.IsDied() || !data.canMove || !data.isInitlized) {
            data.UpdateMove(Vector2.zero);
            return;
        }
        data.UpdateMove(ctx.ReadValue<Vector2>());
    }

    public void DoAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            return;
        }
        if (data.IsDied() || !data.isInitlized) {
            return;
        }
        Debug.Log("DoAttack");
        HandleAttack();
    }

    public void DoDefence(InputAction.CallbackContext ctx) {
        if (ctx.performed == false) {
            return;
        }
        if (data.IsDied() || !data.isInitlized) {
            return;
        }
        Debug.Log("DoDefence");

        data.UpdateDefenceTick();
    }

    public void DoDie(){
        if(SoundManager.Instance != null){
            SoundManager.Instance.PlaySoundEffect(SoundEffectType.Death);
        }
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

    public CharacterController GetCharacterController(){
        return controller;
    }
    public PlayerAnimation GetPlayerAnimation(){
        return aniControl;
    }

    public List<Transform> GetFirePointList(){
        return firePoints;
    }

    public void TriggerHit(List<ItemManager.InteractType> typeList, int ownerIndex = -1) {
        UpdateBehavior();
        playerBehavior.TriggerHit(this, typeList, ownerIndex);
    }

    public void UpdatePosition(Vector3 pos){
        lockUpdateMove = true;
        StartCoroutine(DelayUpdatePos(pos));
    }

    IEnumerator DelayUpdatePos(Vector3 pos){
        yield return 0;
        transform.position = pos;
        lockUpdateMove = false;
    }
    #endregion
}

[SerializeField]
public class PlayerData{
    public int index;
    public MatchStatus matchStatus;
    public InputDevice device;
    public int hp;
    public int speed;
    public int ammo {
        get {
            return ItemManager.Instance.GetPlayerBubbleCount(index);
        }
    }
    public bool canMove;

    public Vector2 move = Vector2.zero;
    public Vector2 faceto = Vector2.left;
    public int faceType = 3; //0 up 1 down 2 right 3 left
    public long lastDefenceTick = 0;
    public bool isInitlized = false;

    //GameObject
    public PlayerController playerController;

    public PlayerData(InputDevice device, int index, PlayerController playerController) {
        this.device = device;
        this.index = index;
        this.hp = 100;
        this.speed = 50;
        this.canMove = true;
        this.playerController = playerController;
    }

    public void ModifyHP(int v) {
        hp += v;
        if (hp <= 0) {
            hp = 0;

            move = Vector2.zero;
        }else{
            if(v < 0){
                if(SoundManager.Instance != null){
                    SoundEffectType type = SoundEffectType.Damage1;
                    int r = UnityEngine.Random.Range(0,1);
                    if(r == 1){
                        type = SoundEffectType.Damage2;
                    }
                    SoundManager.Instance.PlaySoundEffect(type);
                }
            }
        }
    }
    public void ModifySpeed(int v) {
        speed += v;
        if (speed <= 0) {
            speed = 0;
        }
    }

    public void UpdateDefenceTick() {
        lastDefenceTick = System.DateTime.Now.Ticks;
    }

    public void UpdateMove(Vector2 v) {
        move = v * Time.fixedDeltaTime * speed * 0.05f;

        float x = move.x;
        float y = move.y;

        //斜方處理
        if (x + y > 1 || x + y < -1 ||
            (x > 0.1f && y < -0.1f) ||
            (x < -0.1f && y > 0.1f)) {

            x = x * Mathf.Sqrt(0.5f);
            y = y * Mathf.Sqrt(0.5f);
        }

        move.x = x;
        move.y = y;

        UpdateFace();
    }

    public void UpdateFace() {
        //face where
        //優先上下
        //再左右
        float x = move.x;
        float y = move.y;
        bool horizonMove = Mathf.Abs(move.x) >= Mathf.Abs(y);
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
    }

    public bool IsDied() {
        return hp <= 0;
    }

    public bool IsDefencing() {
        if (IsDied() || !isInitlized) {
            return false;
        }
        // 1 tick = 0.0000001 sec
        // 1000000 ticks = 0.1 sec
        long safeDefenceTime = 10000000;
        return System.DateTime.Now.Ticks - lastDefenceTick < safeDefenceTime;
    }

    public void Respawn(){
        hp = 100;
        speed = 50;
        canMove = true;

        ItemManager.Instance.ResetPlayerBubble(index);
    }
}