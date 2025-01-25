namespace GamePlay
{
	public enum MatchResult : byte
	{
		Success,
		NotJoin,
		JoinFail,
		AlreadyReady,
		ReadyFail,
		PlayerNotReady,
		PlayerNotEnough,
		BattleIsStarted
	}
}
