namespace GamePlay
{
	public interface IStateManager
	{
		StateBase CurrentState { get; }

		T GetState<T>() where T : StateBase;
		void ChangeState<T>() where T : StateBase;
	}
}