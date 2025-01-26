using GamePlay;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class BattlePanel : MonoBehaviour, IObserver<TimeInfo>
{
    [SerializeField]
    private Text _countDown;

    private IBattleManager _battleManager;

    private void Awake()
    {
        SystemService.TryGetService<IBattleManager>(out _battleManager);
        _battleManager.Register(this);
    }
    private void OnDestroy()
    {
        _battleManager.Deregister(this);
    }

    void IObserver<TimeInfo>.Update(TimeInfo data)
    {
        _countDown.text = data.Countdown.ToString("F0");
    }
}
