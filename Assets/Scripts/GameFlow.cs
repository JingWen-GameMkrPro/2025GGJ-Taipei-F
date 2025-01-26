using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace GamePlay
{
	public class GameFlow 
	{
		private Timer _resultTimer;
		private IStateManager _stateManager;
		public GameFlow(ITimerManager timerManager, IStateManager stateManager)
		{
			_stateManager = stateManager;
			_resultTimer = timerManager.GetTimer();
			_resultTimer.OnUpdate += OnResultCountdown;
			//TODO: 釋放空間、SceneName查表
			stateManager.GetState<MatchState>().OnEnterState += () => ChangeScene("Level02 Lobby");
			stateManager.GetState<BattleState>().OnEnterState += () => ChangeScene("Level01");
			stateManager.GetState<ResultState>().OnEnterState += () => OnEnterResult();
		}

        private void OnResultCountdown(float countdown)
        {
			if (countdown <= 0)
			{
				_stateManager.ChangeState<MatchState>();
			}
        }

        private void OnEnterResult()
        {
			_resultTimer.StartCountdown(10);
        }

        public void ChangeScene(string sceneName)
		{
			if (string.Compare(SceneManager.GetActiveScene().name, sceneName) == 0)
			{
				// 略過當前Scene
				return;
			}	
			if (Application.CanStreamedLevelBeLoaded(sceneName))
			{
				// 載入指定名稱的場景
				Debug.Log($"Change scene to {sceneName}");
				SceneManager.LoadScene(sceneName);
			}
			else
			{
				// 場景名稱不正確時顯示錯誤訊息
				Debug.LogError("Scene not found: " + sceneName);
			}
		}
	}
}