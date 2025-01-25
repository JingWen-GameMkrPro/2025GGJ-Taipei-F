using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public static class SystemService
	{
		private static Dictionary<string, object> _services = new Dictionary<string, object>();
		public static void AddService<T>(T service)
		{
			_services.Add(typeof(T).Name, service);
		}
		public static void RemoveService<T>()
		{
			_services.Remove(typeof(T).Name);
		}
		public static bool TryGetService<T>(out T service)
		{
			service = default(T);
			if (_services.TryGetValue(typeof(T).Name, out var _service))
			{
				service = (T)_service;
				return true;
			}
			Debug.LogError($"{typeof(T).Name} is not exist");
			return false;
		}
	}
}