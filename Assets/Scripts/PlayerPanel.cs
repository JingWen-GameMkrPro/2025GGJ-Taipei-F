using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public int playerIndex;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text killNumText;
    [SerializeField] private Text ammoNumText;
    [SerializeField] GameObject joinedUI;
    [SerializeField] GameObject notJoinedUI;
    public bool isJoin = false;

    public float hpPortion = 1;
    public int kills = 0;
    public int ammo = 999;

    private void Update()
    {
        updatePlayerUI(hpPortion, kills, ammo);
    }

    public void updatePlayerUI(float hpPortion, int kills, int ammo)
    {
        if (isJoin)
        {
            hpSlider.value = hpPortion;
            killNumText.text = "Kill¡G" + kills;
            ammoNumText.text = "x 999";//¼È®É¼g¦º
        }
    }

    public void playerJoin()
    {
        Debug.Log("Player " + playerIndex + "join the game!");
        notJoinedUI.SetActive(false);
        joinedUI.SetActive(true);
        isJoin = true;
    }
}
