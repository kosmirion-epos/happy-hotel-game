using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ShieldConsumable : MonoBehaviour
{
	/// <summary>
	/// How long the Shield is supposed to stay up.
	/// </summary>
	[SerializeField]
	[Tooltip("How long the Shield is supposed to stay up.")]
	private float shieldStayDuration;

	/// <summary>
	/// The amount the spawned shield prefab should be scaled by.
	/// </summary>
	[SerializeField]
	[Tooltip("The amount the spawned shield prefab should be scaled by.")]
	[Range(0.3f, 5)]
	private float shieldScale = 1;

	/// <summary>
	/// The force scale to apply to the hand on failed activation.
	/// </summary>
	[SerializeField]
	[Tooltip("The force scale to apply to the hand on failed activation.")]
	private float activationFailForceScale = 3;

	/// <summary>
	/// The VFX that's spawned on squeeze
	/// </summary>
	[Tooltip("The VFX that's spawned on squeeze")]
	[SerializeField]
	private VisualEffectConfig splashVFX;

	/// <summary>
	/// Reference to prefab for the actual shield.
	/// </summary>
	[Foldout("Editor Set References")]
	[Label("Shield Prefab")]
	[Tooltip("Reference to prefab for the actual shield.")]
	[SerializeField]
	private GameObject shieldPrefab;

	/// <summary>
	/// Reference to the global flag to lookup if consumables are allowed to activate.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("Reference to the global flag to lookup if consumables are allowed to activate.")]
	[SerializeField]
	private ScopedValue<bool> activatableFlag;

	/// <summary>
	/// Reference to this object's visuals.
	/// </summary>
	[Foldout("Editor Set References")]
	[Label("The Visuals")]
	[Tooltip("Reference to this object's visuals.")]
	[ReadOnly]
	[Required("Please fill the reference. You may need to temporarily need to remove the [ReadOnly] Attribute.")]
	[SerializeField]
	private GameObject visuals;

	[Foldout("Editor Set References")]
    [ReadOnly]
	[SerializeField]
	private ScopedValue<Transform> playerTransform;

	/// <summary>
	/// Reference to this object's rigid body.
	/// </summary>
	[Foldout("Editor Set References")]
	[Label("This Rigidbody")]
	[Tooltip("Reference to this object's collider.")]
	[ReadOnly]
	[Required("Please fill the reference. You may need to temporarily need to remove the [ReadOnly] Attribute.")]
	[SerializeField]
	private new Rigidbody rigidbody;

	/// <summary>
	/// Reference to this object's collider.
	/// </summary>
	[Foldout("Editor Set References")]
	[Label("This Collider")]
	[Tooltip("Reference to this object's collider.")]
	[ReadOnly]
	[Required("Please fill the reference. You may need to temporarily need to remove the [ReadOnly] Attribute.")]
	[SerializeField]
	private Collider col;

	/// <summary>
	/// Reference to this object's Grabbable.
	/// </summary>
	[Foldout("Editor Set References")]
	[Label("This Grabbable")]
	[Tooltip("Reference to this object's grabbable.")]
	[ReadOnly]
	[Required("Please fill the reference. You may need to temporarily need to remove the [ReadOnly] Attribute.")]
	[SerializeField]
	private Autohand.Grabbable grabbable;

	/// <summary>
	/// Called by the OnSqueeze Method of the Grabbable utility. Enables infinite ammo and starts a Coroutine to disable it after the <see cref="shieldStayDuration"/>
	/// </summary>
	[Button]
	public void Activate()
	{
		if (!activatableFlag.Value)
		{
			rigidbody.AddForce(Random.insideUnitSphere * activationFailForceScale, ForceMode.Impulse);
			return;
		}

		//deactivate Visuals?
		visuals.SetActive(false);
		col.enabled = false;
		grabbable.ForceHandsRelease();
		grabbable.enabled = false;
		splashVFX.Spawn(transform.position, Quaternion.identity);

		GameObject x = Instantiate(shieldPrefab, transform.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(playerTransform.Value.forward, Vector3.up), Vector3.up));

		x.GetComponent<ShieldDespawn>().DestroyDelay = shieldStayDuration;
		x.transform.localScale *= shieldScale;

		Destroy(gameObject);
	}

	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
