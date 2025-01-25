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
		void Join(PlayerData playerData);
		/// <summary>
		/// 準備開始，所有人準備開始後會自動開始
		/// </summary>
		void Ready(PlayerData playerData);
		/// <summary>
		/// 開始，需所有玩家進入準備
		/// </summary>
		void Start();
	}
}
