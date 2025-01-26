using UnityEngine;

public class PlayerInputInit : MonoBehaviour
{
    public static GameObject playerInputManager;
    public GameObject managerPrefab;

    void OnEnable()
    {
        if(playerInputManager == null){
            playerInputManager = Instantiate(managerPrefab);
            DontDestroyOnLoad(playerInputManager);
        }
    }
}
