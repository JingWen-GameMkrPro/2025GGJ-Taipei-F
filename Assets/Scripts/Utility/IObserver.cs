namespace Utility
{
	/// <summary>
	/// 觀察者
	/// </summary>
	public interface IObserver<T>
	{
		/// <summary>
		/// 更新，由主題呼叫
		/// </summary>
		void Update(T data);
	}
}