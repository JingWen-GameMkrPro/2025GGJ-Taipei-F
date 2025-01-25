namespace Utility
{
	/// <summary>
	/// 可被觀察者訂閱的主題，傳遞的資料為T
	/// </summary>
	public interface ISubject<T>
	{
		/// <summary>
		/// 訂閱
		/// </summary>
		void Register(IObserver<T> observer);
		/// <summary>
		/// 解訂閱
		/// </summary>
		void Deregister(IObserver<T> observer);
		/// <summary>
		/// 呼叫觀察者的更新
		/// </summary>
		void Notify(T notify);
	}
}