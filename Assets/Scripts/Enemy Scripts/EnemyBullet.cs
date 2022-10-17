using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemyBullet : MonoBehaviour
{
	/// <summary>
	/// Stores how much damage the bullet will deal to the <see cref="scoreReference"/>.
	/// </summary>
	[ReadOnly]
	[Tooltip("Stores how much damage the bullet will deal to the Score")]
	public int DamageToDeal;

	/// <summary>
	/// Reference to the Collider on this bullet.
	/// </summary>
	public Collider Coll;

	/// <summary>
	/// Global Reference to the current score of the player.
	/// </summary>
	[SerializeField] 
	private GlobalValue<int> scoreReference;

	/// <summary>
	/// Global Reference to the current score of the player.
	/// </summary>
	[SerializeField]
	private GlobalValue<int> maxScoreReference;

	[SerializeField]
	private GlobalEvent HitPlayer;

	[SerializeField]
	private VisualEffectConfig destroyVFX;

	/// <summary>
	/// The player tag.
	/// </summary>
	[SerializeField]
	[Tag]
	private string playerTag;

	private bool hasHitPlayer;

	private void OnTriggerEnter(Collider other)
	{
		if (!hasHitPlayer && other.transform.CompareTag(playerTag))
		{
			scoreReference.Value = Mathf.Max(0, scoreReference.Value - DamageToDeal);
			maxScoreReference.Value = Mathf.Max(0, maxScoreReference.Value - DamageToDeal);

			HitPlayer?.Invoke();

			Coll.enabled = false;
			hasHitPlayer = true;
		}

		destroyVFX.Spawn(transform.position, transform.rotation);

		// Friendly fire xD?

		//Debug.Log($"Hit {other.transform.name} and as a result this destroyed itself", other.transform);

		// TODO maybe eventually add sound and a decal on the wall?
		Destroy(gameObject);
	}

	/// <summary>
	/// Used by an event to destroy all bullets on round end.
	/// </summary>
	public void SelfDestruct()
	{
		destroyVFX.Spawn(transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
