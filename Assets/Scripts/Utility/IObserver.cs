namespace Utility
{
	/// <summary>
	/// �V�@��
	/// </summary>
	public interface IObserver<T>
	{
		/// <summary>
		/// �X�V�C�R���ċ�
		/// </summary>
		void Update(T data);
	}
}