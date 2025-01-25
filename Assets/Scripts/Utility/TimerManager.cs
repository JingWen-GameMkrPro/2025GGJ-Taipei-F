using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public class TimerManager : MonoBehaviour, ITimerManager
	{
		private HashSet<Timer> _runningTimers = new HashSet<Timer>();
		private HashSet<Timer> _sleepTimers = new HashSet<Timer>();
		private HashSet<Timer> _waitRemoveTimers = new HashSet<Timer>();
		private HashSet<Timer> _waitAddTimers = new HashSet<Timer>();

		private void FixedUpdate()
		{
			foreach (var runningTimer in _runningTimers)
			{
				var timer = runningTimer as ITimer;
				timer.Update(Time.deltaTime);

				if (!runningTimer.IsRunning)
				{
					_waitRemoveTimers.Add(runningTimer);
				}
			}

			foreach (var sleepTimer in _sleepTimers)
			{
				if (sleepTimer.IsRunning)
				{
					_waitAddTimers.Add(sleepTimer);
				}
			}

			foreach (var timer in _waitRemoveTimers)
			{
				_runningTimers.Remove(timer);
				_sleepTimers.Add(timer);
			}
			_waitRemoveTimers.Clear();

			foreach (var timer in _waitAddTimers)
			{
				_runningTimers.Add(timer);
				_sleepTimers.Remove(timer);
			}
			_waitAddTimers.Clear();
		}

		public Timer GetTimer(float totalTime)
		{
			var timer = new Timer();
			_sleepTimers.Add(timer);
			return timer;
		}
	}
}