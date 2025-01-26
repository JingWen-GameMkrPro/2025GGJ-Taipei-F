using UnityEngine;

public class Keeper : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
