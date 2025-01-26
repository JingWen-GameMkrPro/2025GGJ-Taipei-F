using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("State")]
    [SerializeField] float speed = 0;
    [SerializeField] bool faceup = false;
    [SerializeField] bool isDied = false;

    [Header("body")]
    [SerializeField] List<GameObject> bodyList;
    [SerializeField] bool isLeft = true;

    [Header("arm")]
    [SerializeField] List<GameObject> leftArmList;
    [SerializeField] List<GameObject> rightArmList;
    [SerializeField] List<GameObject> upArmList;
    [SerializeField] List<GameObject> downArmList;
    [SerializeField] int armType = 0; //0 up 1 down 2 right 3 left

    [SerializeField] [Min(0)] private float hurtTime;
    [SerializeField] private Color hurtColor;

    public void UpdateState(Vector2 move, int armType, bool isDied) {
        speed = move.magnitude;
        this.isDied = isDied;
        this.armType = armType;
        if (speed <= 0) {
            speed = -1;
        }

        if (speed > 0) {
            if (armType != -1) {
                faceup = armType == 0;
            }

            isLeft = move.x <= 0;
        }
    }

    private void Update() {
        HandleAnimation();
        HandldBody();
    }

    void HandleAnimation() {
        foreach (GameObject obj in bodyList) {
            if (obj.activeSelf == true && obj.activeInHierarchy == true && obj.GetComponent<Animator>() != null) {
                obj.GetComponent<Animator>().SetBool("face", faceup);
                obj.GetComponent<Animator>().SetFloat("speed", speed);
                obj.GetComponent<Animator>().SetBool("die", isDied);
            }
        }
    }

    public void PlayHurtAnimation() {
        StartCoroutine(Hurt());
    }
    
    private IEnumerator Hurt() {
        MeshRenderer meshRenderer = null;
        foreach (GameObject obj in bodyList) {
            MeshRenderer _meshRenderer = obj.GetComponent<MeshRenderer>();
            if (obj.activeSelf == true && obj.activeInHierarchy == true && _meshRenderer != null) {
                meshRenderer = _meshRenderer;
            }
        }
        if(meshRenderer == null){
            yield break;
        }

        meshRenderer.material.color = hurtColor;
        yield return new WaitForSeconds(hurtTime);
        meshRenderer.material.color = Color.white;
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

    public void step(){
        Debug.Log("step!");
    }
}
