using GamePlay;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public struct PlayerInfo
{
	public int index;
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
		SystemService.TryGetService<IMatchManager>(out var matchManager);
		SystemService.TryGetService<IBattleManager>(out var battleManager);
		battleManager.Register(this);
	}

	private void OnDestroy()
	{
		SystemService.TryGetService<IMatchManager>(out var matchManager);
		SystemService.TryGetService<IBattleManager>(out var battleManager);
		battleManager.Deregister(this);
	}

	void IObserver<PlayerInfo>.Update(PlayerInfo data)
	{
		if (data.index == playerIndex)
		{
			hpSlider.value = maxHealth > 0 ? (float)data.currentHealth / maxHealth : (float)data.currentHealth / 100;
			killNumText.text = data.kill.ToString();
			ammoNumText.text = data.ammo.ToString();

			notJoinedUI.SetActive(false);
			joinedUI.SetActive(true);
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
