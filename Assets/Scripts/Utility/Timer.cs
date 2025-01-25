using System;

namespace Utility
{
	public interface ITimer
	{
		void Update(float deltaTime);
	}
	public class Timer : ITimer
	{
		private float _totalTime = 0f;
		public bool IsRunning { get; private set; }
		public event Action<float> OnUpdate;

		void ITimer.Update(float deltaTime)
		{
			if (IsRunning)
			{
				_totalTime -= deltaTime;
				_totalTime = _totalTime > 0 ? _totalTime : 0f;
				IsRunning = _totalTime > 0;
				OnUpdate?.Invoke(_totalTime);
			}
		}
		public void StartComedown(float time)
		{
			_totalTime = time;
			Start();
		}
		public void Start()
		{
			IsRunning = true;
		}

		public void Pause()
		{
			IsRunning = false;
		}

		public void Reset()
		{
			Pause();
			_totalTime = 0f;
		}
	}
}