using System;
using System.Collections.Generic;

namespace GamePlay
{
	public class StateManager : IStateManager
	{
		private Dictionary<string, StateBase> _stateDict = new Dictionary<string, StateBase>();
		public StateBase CurrentState { get; private set; }

		public StateManager()
		{
			//TODO: 由外部建立
			_stateDict[typeof(MatchState).Name] = new MatchState();
			_stateDict[typeof(BattleState).Name] = new BattleState();
			_stateDict[typeof(ResultState).Name] = new ResultState();
		}

		public T GetState<T>() where T : StateBase
		{
			T state = default(T);
			if (_stateDict.TryGetValue(typeof(T).Name, out var _state))
			{
				state = (T)_state;
			}
			return state;
		}

		public void ChangeState<T>() where T : StateBase
		{
			var nextState = GetState<T>();
			CurrentState?.OnExitState?.Invoke();
			CurrentState = nextState;
			CurrentState?.OnEnterState?.Invoke();
		}
	}

	public class StateBase
	{
		public Action OnEnterState;
		public Action OnExitState;
	}

	public class MatchState : StateBase
	{
	}

	public class BattleState : StateBase
	{
	}

	public class ResultState : StateBase
	{
	}
}