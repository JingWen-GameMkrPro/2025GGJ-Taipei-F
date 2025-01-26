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
	public int index;
	public bool isReady;
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
	[SerializeField] GameObject readyUI;
	private bool _isJoined = false;
	private int maxHealth = 0;
	private StateBase _state;
	private void Start()
	{

		SystemService.TryGetService<IStateManager>(out var stateManager);
		_state = stateManager.CurrentState;

		if (_state == stateManager.GetState<MatchState>())
		{
			SystemService.TryGetService<IMatchManager>(out var matchManager);
			matchManager.Register(this);
		}
		else if (_state == stateManager.GetState<BattleState>())
		{
			SystemService.TryGetService<IBattleManager>(out var battleManager);
			battleManager.Register(this);
		}
	}

	private void OnDestroy()
	{
		SystemService.TryGetService<IStateManager>(out var stateManager);

		if (_state == stateManager.GetState<MatchState>())
		{
			SystemService.TryGetService<IMatchManager>(out var matchManager);
			matchManager.Deregister(this);
		}
		else if (_state == stateManager.GetState<BattleState>())
		{
			SystemService.TryGetService<IBattleManager>(out var battleManager);
			battleManager.Deregister(this);
		}

		_state = null;
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
		if (data.index == playerIndex)
		{
			maxHealth = data.maxHealth;
			notJoinedUI.SetActive(false);
			joinedUI.SetActive(true);
			readyUI.SetActive(data.isReady);
			_isJoined = true;
		}
	}
}
