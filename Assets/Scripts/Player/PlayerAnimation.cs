using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("State")]
    [SerializeField] float speed = 0;
    [SerializeField] bool faceup = false;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("body")]
    [SerializeField] List<GameObject> bodyList;
    [SerializeField] bool isLeft = true;

    [Header("arm")]
    [SerializeField] List<GameObject> leftArmList;
    [SerializeField] List<GameObject> rightArmList;
    [SerializeField] List<GameObject> upArmList;
    [SerializeField] List<GameObject> downArmList;
    [SerializeField] int armType = 0; //0 up 1 down 2 right 3 left

    public void UpdateState(Vector2 move, int armType) {
        speed = move.magnitude;
        this.armType = armType;
        if (speed <= 0) {
            speed = -1;
        }

        if (speed > 0) {
            if (armType != -1) {
                faceup = armType == 0;
            }

            isLeft = move.x < 0;
        }
    }

    private void Update() {
        HandleAnimation();
        HandldBody();
    }

    void HandleAnimation() {
        animator.SetBool("face", faceup);
        animator.SetFloat("speed", speed);
    }

    void HandldBody() {
        List<GameObject> armList = null;
        List<GameObject> otherArmList = new List<GameObject>();
        switch (armType) {
            case 0:
                armList = upArmList;
                otherArmList.AddRange(downArmList);
                otherArmList.AddRange(rightArmList);
                otherArmList.AddRange(leftArmList);
                break;
            case 1:
                armList = downArmList;
                otherArmList.AddRange(upArmList);
                otherArmList.AddRange(rightArmList);
                otherArmList.AddRange(leftArmList);
                break;
            case 2:
            case 3:
                armList = leftArmList;
                otherArmList.AddRange(downArmList);
                otherArmList.AddRange(rightArmList);
                otherArmList.AddRange(upArmList);
                break;
        }

        for (int i = 0; armList != null && i < armList.Count; i++) {
            armList[i].SetActive(true);
        }
        for (int i = 0; otherArmList != null && i < otherArmList.Count; i++) {
            otherArmList[i].SetActive(false);
        }
        for (int i = 0; i < bodyList.Count; i++) {
            bodyList[i].transform.localEulerAngles = new Vector3(0, isLeft? 0 : 180, 0);
        }
    }
}
