using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DoublePoints : MonoBehaviour
{
	/// <summary>
	/// The duration in real time the effect should last.
	/// </summary>
	[SerializeField]
	[Tooltip("The duration in real time the effect should last.")]
	[Label("Effect Duration")]
	private float duration;

	/// <summary>
	/// The VFX that's spawned on squeeze
	/// </summary>
	[Tooltip("The VFX that's spawned on squeeze")]
	[SerializeField]
	private VisualEffectConfig splashVFX;

	/// <summary>
	/// The force scale to apply to the hand on failed activation.
	/// </summary>
	[SerializeField]
	[Tooltip("The force scale to apply to the hand on failed activation.")]
	private float activationFailForceScale = 3;

	/// <summary>
	/// Reference to the global flag to lookup if consumables are allowed to activate.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("Reference to the global flag to lookup if consumables are allowed to activate.")]
	[SerializeField]
	private ScopedValue<bool> activatableFlag;

	/// <summary>
	/// Reference to the global flag for enemies if they should grant double the score reward.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("Reference to the global flag for enemies if they should grant double the score reward.")]
	[SerializeField]
	private ScopedValue<bool> doublePointsFlag;

	/// <summary>
	/// The Global Event invoked when the consumable is activated.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("The Global Event invoked when the consumable is activated.")]
    [ReadOnly]
	[SerializeField]
	private GlobalEvent doublePointsActivate;

	/// <summary>
	/// The Global Event invoked when the consumable is deactivated.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("The Global Event invoked when the consumable is deactivated.")]
    [ReadOnly]
	[SerializeField]
	private GlobalEvent doublePointsDeactivate;

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

	private bool _hasThisBeenActivated;

	/// <summary>
	/// Called by the OnSqueeze Method of the Grabbable utility. Enables infinite ammo and starts a Coroutine to disable it after the <see cref="duration"/>
	/// </summary>
	[Button(enabledMode: EButtonEnableMode.Playmode)]
	public void TryActivate()
	{
		if (doublePointsFlag.Value || !activatableFlag.Value)
		{
			rigidbody.AddForce(Random.insideUnitSphere * activationFailForceScale, ForceMode.Impulse);
			return;
		}

		// TODO Soundeffects

		doublePointsFlag.Value = true;

		//deactivate Visuals?
		visuals.SetActive(false);
		col.enabled = false;
		grabbable.ForceHandsRelease();
		grabbable.enabled = false;
		splashVFX.Spawn(transform.position, Quaternion.identity);

		StartCoroutine(DeactivateAfterTime());

		_hasThisBeenActivated = true;

		doublePointsActivate.Invoke();
	}

	/// <summary>
	/// Coroutine utilized by <see cref="Activate"/>. Disables <see cref="doublePointsFlag"/> after <see cref="duration"/> has passend and destroys this object.
	/// </summary>
	/// <returns></returns>
	private IEnumerator DeactivateAfterTime()
	{
		yield return new WaitForSeconds(duration);

		Deactivate();
	}

	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Deactivate()
	{
		if (_hasThisBeenActivated)
		{
			doublePointsFlag.Value = false;
			doublePointsDeactivate.Invoke();

			Destroy(gameObject);
		}
	}

	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
