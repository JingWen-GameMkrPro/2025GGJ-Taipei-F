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
			
			SystemService.AddService<IStateManager>(stateManager);
			SystemService.AddService<IMatchManager>(matchManager);
			SystemService.AddService<GameFlow>(gameFlow);

			stateManager.ChangeState<MatchState>();
		}
	}
}