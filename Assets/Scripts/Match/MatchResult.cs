namespace GamePlay
{
	public enum MatchResult : byte
	{
		Success,
		NotJoin,
		AlreadyJoin,
		JoinFail,
		AlreadyReady,
		ReadyFail,
		PlayerNotReady,
		PlayerNotEnough,
		BattleIsStarted
	}
}
