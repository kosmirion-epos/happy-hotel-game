using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TimeSlow : MonoBehaviour
{
	/// <summary>
	/// The duration in real time the effect should last.
	/// </summary>
	[SerializeField]
	[Tooltip("The duration in real time the effect should last.")]
	[Label("Effect Duration")]
	private float duration;

	/// <summary>
	/// The timescale to be used for the <see cref="duration"/>
	/// </summary>
	[SerializeField]
	[Tooltip("The timescale to be used for the duration.")]
	[Range(0.05f, 1)]
	[OnValueChanged(nameof(LimitToStepsOfFive))]
	private float tempTimeScale;

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
	/// Buffer field to save the previous timescale to, to be able to restore it.
	/// </summary>
	private float oldTimeScale;

	/// <summary>
	/// Buffer field to save the previous fixedDeltaTime to, to be able to restore it.
	/// </summary>
	private float oldFixedDelta;

	/// <summary>
	/// Buffer field to save the previous VFXManager.fixedTimeStep to, to be able to restore it.
	/// </summary>
	private float oldVFXFixedTimeStep;


	/// <summary>
	/// Reference to the global flag to lookup if consumables are allowed to activate.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("Reference to the global flag to lookup if consumables are allowed to activate.")]
	[SerializeField]
	private ScopedValue<bool> activatableFlag;

	/// <summary>
	/// Reference to the global flag to lookup if timeslow is active.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("Reference to the global flag to lookup if timeslow is active.")]
	[SerializeField]
	private ScopedValue<bool> timeSlowFlag;

	/// <summary>
	/// The Global Event invoked when the consumable is activated.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("The Global Event invoked when the consumable is activated.")]
	[ReadOnly]
	[SerializeField]
	private GlobalEvent timeSlowActivate;

	/// <summary>
	/// The Global Event invoked when the consumable is deactivated.
	/// </summary>
	[Foldout("Editor Set References")]
	[Tooltip("The Global Event invoked when the consumable is deactivated.")]
	[ReadOnly]
	[SerializeField]
	private GlobalEvent timeSlowDeactivate;

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
		if (timeSlowFlag.Value || !activatableFlag.Value)
		{
			rigidbody.AddForce(Random.insideUnitSphere * activationFailForceScale, ForceMode.Impulse);
			return;
		}

		// TODO Soundeffects

		timeSlowFlag.Value = true;

		oldTimeScale = Time.timeScale;
		Time.timeScale = tempTimeScale;
		oldFixedDelta = Time.fixedDeltaTime;
		Time.fixedDeltaTime = Time.fixedDeltaTime * tempTimeScale;
		oldVFXFixedTimeStep = VFXManager.fixedTimeStep;
		VFXManager.fixedTimeStep = VFXManager.fixedTimeStep * tempTimeScale;

		//deactivate Visuals?
		visuals.SetActive(false);
		col.enabled = false;
		grabbable.ForceHandsRelease();
		grabbable.enabled = false;
		splashVFX.Spawn(transform.position, Quaternion.identity);

		StartCoroutine(DeactivateAfterTime());

		_hasThisBeenActivated = true;

		timeSlowActivate.Invoke();
	}

	/// <summary>
	/// Coroutine utilized by <see cref="TryActivate"/>. Disables <see cref="timeSlowFlag"/> after <see cref="duration"/> has passend and destroys this object.
	/// </summary>
	/// <returns></returns>
	private IEnumerator DeactivateAfterTime()
	{
		yield return new WaitForSecondsRealtime(duration);

		Deactivate();
	}

	public void Deactivate()
	{
		if (_hasThisBeenActivated)
		{
			timeSlowFlag.Value = false;
			Time.timeScale = oldTimeScale;
			Time.fixedDeltaTime = oldFixedDelta;
			VFXManager.fixedTimeStep = oldVFXFixedTimeStep;

			timeSlowDeactivate.Invoke();

			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Used in the OnValueChanged of <see cref="duration"/>. Limits the value to 5% steps.
	/// </summary>
	private void LimitToStepsOfFive()
	{
		tempTimeScale = Mathf.Round(tempTimeScale * 100 / 5) * 5 / 100;
	}

	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
