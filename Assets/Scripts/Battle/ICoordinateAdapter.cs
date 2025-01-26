using UnityEngine;

namespace GamePlay
{
	public interface ICoordinateAdapter
	{
		Vector3 GetPosition(int x, int y);
	}
}