using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("State")]
    [SerializeField] float speed = 0;
    [SerializeField] bool faceup = false;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("arm")]
    [SerializeField] List<GameObject> armList;
    [SerializeField] int armType = 0; //0 up 1 down 2 right 3 left

    public void UpdateState(Vector2 move, int armType) {
        speed = move.magnitude;
        Debug.Log(speed);
        if (speed <= 0) {
            speed = -1;
        }

        if (armType != -1) {
            this.armType = armType;

            if (speed > 0) {
                faceup = armType == 0;
            }
        }
    }

    private void Update() {
        HandleAnimation();
        HandleArm();
    }

    void HandleAnimation() {
        animator.SetBool("face", faceup);
        animator.SetFloat("speed", speed);
    }

    void HandleArm() {
        for (int i = 0; i < armList.Count; i++) {
            armList[i].SetActive(i == armType);
        }
    }
}
