using Autohand;
using UnityEngine;

public class InitializeGlobalTransform : MonoBehaviour
{
	[SerializeField] private GlobalValue<Transform> player;

	private void Awake()
	{
		player.Value = transform;
	}
}
