using UnityEngine;

public class Player_HitSensor : MonoBehaviour
{
    public PlayerController controller;

    public void step(){
        controller.GetPlayerAnimation().Step();
    }
}
