using UnityEngine;
using UnityEngine.UI;
using Utility;

public struct PlayerInfo
{
    public int currentHealth;
    public int kill;
    public int ammo;
}

public struct PlayerJoinInfo
{
    public int maxHealth;
}

public class PlayerPanel : MonoBehaviour, IObserver<PlayerInfo>, IObserver<PlayerJoinInfo>
{
    public int playerIndex;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text killNumText;
    [SerializeField] private Text ammoNumText;
    [SerializeField] GameObject joinedUI;
    [SerializeField] GameObject notJoinedUI;
    private bool _isJoined = false;
    private int maxHealth = 0;
    
    private void Start()
    {
        SystemService.AddService(this);
    }

    void IObserver<PlayerInfo>.Update(PlayerInfo data)
    {
        if (_isJoined)
        {
            hpSlider.value = data.currentHealth;
            killNumText.text = data.kill.ToString();
            ammoNumText.text = data.ammo.ToString();
        }
    }

    void IObserver<PlayerJoinInfo>.Update(PlayerJoinInfo data)
    {
        maxHealth = data.maxHealth;
        notJoinedUI.SetActive(false);
        joinedUI.SetActive(true);
        _isJoined = true;
    }
}
