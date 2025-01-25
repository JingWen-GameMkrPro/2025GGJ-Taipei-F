using UnityEngine;

namespace GamePlay
{
	public class MatchErrorHandler : IErrorHandler
	{
		public void Handle(MatchResult result)
		{
			Debug.Log($"Match Error: {result}");
		}
	}
}