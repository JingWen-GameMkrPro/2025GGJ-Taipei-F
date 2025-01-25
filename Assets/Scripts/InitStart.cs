using UnityEngine;
using Utility;

namespace GamePlay
{
	public class InitStart : MonoBehaviour
	{
		private void Start()
		{
			var stateManager = new StateManager();
			var matchManager = new MatchManager(stateManager);
			var gameFlow = new GameFlow(stateManager);
			var timerManagerGO = new GameObject(nameof(TimerManager));
			DontDestroyOnLoad(timerManagerGO);
			var timerManager = timerManagerGO.AddComponent<TimerManager>();

			SystemService.AddService<IStateManager>(stateManager);
			SystemService.AddService<IMatchManager>(matchManager);
			SystemService.AddService<GameFlow>(gameFlow);
			SystemService.AddService<ITimerManager>(timerManager);

			stateManager.ChangeState<MatchState>();
		}
	}
}