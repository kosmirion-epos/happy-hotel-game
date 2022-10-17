using Autohand;
using UnityEngine;

public class InitializeGlobalAutoHandPlayer : MonoBehaviour
{
	[SerializeField] private GlobalValue<AutoHandPlayer> player;

	private void Awake()
	{
		player.Value = GetComponent<AutoHandPlayer>();
	}
}
