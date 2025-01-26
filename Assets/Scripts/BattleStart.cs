using UnityEngine;
using Utility;

namespace GamePlay
{
	public class BattleStart : MonoBehaviour
	{
		[SerializeField]
		private TerrainAdapter _coordinateAdapter;
		private void Awake()
		{
			if (SystemService.TryGetService<IBattleManager>(out var battleManager))
			{
				//battleManager.StartBattle();
			}
		}
	}
}