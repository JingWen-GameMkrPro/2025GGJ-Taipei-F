using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
	public class GameFlow 
	{
		public GameFlow(IStateManager stateManager)
		{
			//TODO: 釋放空間、SceneName查表
			stateManager.GetState<MatchState>().OnEnterState += () => ChangeScene("Lobby");
			stateManager.GetState<BattleState>().OnEnterState += () => ChangeScene("Level01");
			stateManager.GetState<ResultState>().OnEnterState += () => ChangeScene("");
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