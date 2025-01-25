using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay
{
	/// <summary>
	/// 大廳服務，提供加入、準備、開始
	/// </summary>
	public interface IMatchManager
	{
		/// <summary>
		/// 加入大廳
		/// </summary>
		/// <param name="playerID">用於更新、確認玩家狀態的唯一key，也可以是Index</param>
		void Join(int playerID);
		/// <summary>
		/// 準備開始，所有人準備開始後會自動開始
		/// </summary>
		/// <param name="playerID">用於更新、確認玩家狀態的唯一key，也可以是Index</param>
		void Ready(int playerID);
		/// <summary>
		/// 開始，需所有玩家進入準備
		/// </summary>
		void Start();
	}
}
