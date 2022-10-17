using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Autohand;
using NaughtyAttributes;

public class Grenade1 : MonoBehaviour
{
	[HorizontalLine(height: 5, color: EColor.Red)]
	[SerializeField][Label("Explosion Mode")][Dropdown(nameof(WipeFloorValues))] private bool wipeFloorFlag;

	[Header("Grenade")]
	[HorizontalLine(1, EColor.Gray)]
	[SerializeField][Required][BoxGroup("References")] private Grabbable grenade;
	[SerializeField][Required][BoxGroup("References")] private Grabbable pin;
	[SerializeField][Required][BoxGroup("References")] private ConfigurableJoint pinJoint;
	[SerializeField][Required][BoxGroup("References")] private GlobalValue<bool> activatableFlag;
	[Header("VFX")]
	[SerializeField][Required][BoxGroup("References")] private VisualEffectConfig explosionVfx;
	[SerializeField][BoxGroup("References")] private bool vfxInheritRotation = false;

	[Space]
	[HorizontalLine(height: 5, color: EColor.Red)]
	[SerializeField][Range(0.1f, 10)] private float explosionDelay = 2;
	[SerializeField][Range(1.5f, 15)][HideIf(nameof(wipeFloorFlag))] private float explosionRadius = 10;
	[SerializeField] private bool startDelayOnRelease = false;
	/// <summary>
	/// LayerMask to configure what should be able to get hit by the Grenade.
	/// </summary>
	[SerializeField] private LayerMask damageLayers;

	[SerializeField][Tag] private string bodyTag;

	[HorizontalLine(1, EColor.Gray)]
	[SerializeField] private float pinJointStrength = 750f;


	[Space]
	[SerializeField] private UnityEvent pinBreakEvent;
	[SerializeField] private UnityEvent explosionEvent;

	private void OnEnable()
	{
		pin.isGrabbable = false;
		grenade.OnGrabEvent += OnGrenadeGrab;
		grenade.OnReleaseEvent += OnGrenadeRelease;
		pin.OnGrabEvent += OnPinGrab;
		pin.OnReleaseEvent += OnPinRelease;
		if (!grenade.jointedBodies.Contains(pin.body))
			grenade.jointedBodies.Add(pin.body);
		if (!pin.jointedBodies.Contains(grenade.body))
			pin.jointedBodies.Add(grenade.body);
	}

	private void OnDisable()
	{
		grenade.OnGrabEvent -= OnGrenadeGrab;
		grenade.OnReleaseEvent -= OnGrenadeRelease;
		pin.OnGrabEvent -= OnPinGrab;
		pin.OnReleaseEvent -= OnPinRelease;
	}

	void OnGrenadeGrab(Hand hand, Grabbable grab)
	{
		if (pinJoint != null)
		{
			pin.isGrabbable = true;
		}
	}

	void OnGrenadeRelease(Hand hand, Grabbable grab)
	{
		if (pinJoint != null)
			pin.isGrabbable = false;
		else if (grenade != null && startDelayOnRelease)
			Invoke(nameof(Explode), explosionDelay + Time.fixedDeltaTime * 3);
	}
	void OnPinGrab(Hand hand, Grabbable grab)
	{
		if (pinJoint != null && activatableFlag.Value)
		{
			pinJoint.breakForce = pinJointStrength;
		}
	}

	void OnPinRelease(Hand hand, Grabbable grab)
	{
		if (pinJoint != null)
		{
			pinJoint.breakForce = 100000;
		}
	}

	private void OnJointBreak(float breakForce)
	{
		Invoke(nameof(CheckJointBreak), Time.fixedDeltaTime * 2);
	}

	void CheckJointBreak()
	{
		if (pinJoint == null)
		{
			pin.maintainGrabOffset = false;
			pin.RemoveJointedBody(grenade.body);
			grenade.RemoveJointedBody(pin.body);
			if (!startDelayOnRelease)
				Invoke(nameof(Explode), explosionDelay);
		}
	}

	[Button]
	void Explode()
	{
		if (wipeFloorFlag)
		{
			var enemies = FindObjectsOfType(typeof(EnemyBaseVariation));

			foreach (EnemyBaseVariation enemy in enemies)
			{
				enemy.Hit(enemy.GetComponentInChildren<MeshRenderer>().bounds.ClosestPoint(grenade.transform.position));
			}
		}
		else
		{
			Collider[] cols = Physics.OverlapSphere(grenade.transform.position, explosionRadius, damageLayers);

			foreach (Collider col in cols)
			{
				if (col.CompareTag(bodyTag))
				{
					//Debug.Log($"Using {col.transform.position} as a base for getting the hit location.\nUsing {col.gameObject.GetComponent<MeshRenderer>().bounds.center} as Mesh center.");
					col.GetComponentInParent<IShootable>().Hit(
						col.gameObject.GetComponent<MeshRenderer>().bounds.center);
				}
			}
		}

		explosionEvent?.Invoke();
		if (explosionVfx != null)
			if (vfxInheritRotation)
				explosionVfx.Spawn(grenade.transform.position, grenade.transform.rotation);
			else
				explosionVfx.Spawn(grenade.transform.position, Quaternion.identity);

		Destroy(grenade.gameObject);

		Invoke(nameof(DeletePin), 5);
	}

	private void DeletePin()
	{
		if (Application.isPlaying)
			Destroy(transform.parent ? transform.parent.gameObject : gameObject);
		else
			DestroyImmediate(transform.parent ? transform.parent.gameObject : gameObject);
	}

	private DropdownList<bool> WipeFloorValues()
	{
		return new DropdownList<bool>()
		{
			{ "Floorwide Explosion", true },
			{ "Radius-limited Explosion", false }
		};
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if (grenade != null && !wipeFloorFlag) Gizmos.DrawWireSphere(grenade.transform.position, explosionRadius);
	}

	public void CheckPinGrabbability()
	{
		if (grenade.IsHeld() && pin.IsHeld())
		{
			if (pinJoint != null)
			{
				pinJoint.breakForce = pinJointStrength;
			}
		}
	}

	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Destroy()
	{
		Destroy(transform.parent.gameObject);
	}
}