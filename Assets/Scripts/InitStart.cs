using UnityEngine;
using Utility;

namespace GamePlay
{
	public class InitStart : MonoBehaviour
	{
		private void Start()
		{
			var stateManager = new StateManager();
			var timerManagerGO = new GameObject(nameof(TimerManager));
			DontDestroyOnLoad(timerManagerGO);
			var timerManager = timerManagerGO.AddComponent<TimerManager>();
			var battleManager = new BattleManager(stateManager, timerManager);
			var matchManager = new MatchManager(stateManager);
			var gameFlow = new GameFlow(stateManager);

			SystemService.AddService<IStateManager>(stateManager);
			SystemService.AddService<IMatchManager>(matchManager);
			SystemService.AddService<GameFlow>(gameFlow);
			SystemService.AddService<ITimerManager>(timerManager);
			SystemService.AddService<IBattleManager>(battleManager);
		}

		public void StartGame()
		{
			SystemService.TryGetService<IStateManager>(out var stateManager);
			stateManager.ChangeState<MatchState>();
		}

		void Update()
		{
			if (Input.anyKeyDown) 
			{
				StartGame();
			}
		}
	}
}