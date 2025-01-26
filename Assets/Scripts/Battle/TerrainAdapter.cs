using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
	public class TerrainAdapter : MonoBehaviour, ICoordinateAdapter
	{
		[SerializeField]
		private List<Transform> _cells = new List<Transform>();
		[SerializeField]
		private int _width;
		[SerializeField]
		private int _height;

		public Vector3 GetPosition(int x, int y)
		{
			var index = y * _width + x;
			var transform = _cells[index];

			return transform.position;
		}
	}
}