using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.VFX;
using UnityEngine.Events;

public class PreciseRangedAttack : AttackType
{
	/// <summary>
	/// Describes how often/fast the enemy will attack. How many seconds between attacks.
	/// <br>Must be positive.</br>
	/// </summary>
	[SerializeField]
	[Tooltip("Describes how often/fast the enemy will attack. How many seconds between attacks.\nMust be positive.")]
	[BoxGroup("Base Parameters")]
	[MinMaxSlider(0.6f, 10)]
	private Vector2 AttackRate;

	private float _attackDelay;

	/// <summary>
	/// The duration of the anticipation VFX.
	/// </summary>
	[SerializeField]
	[Tooltip("The duration of the anticipation VFX.")]
	[BoxGroup("Base Parameters")]
	private ScopedValue<float> anticipateDuration;

	/// <summary>
	/// How many points the player will loose when hit by the attack.
	/// </summary>
	[SerializeField]
	[Tooltip("How many points the player will loose when hit by the attack.")]
	[BoxGroup("Base Parameters")]
	private int AttackDamage;
	/// <summary>
	/// How fast will the spawned projectile move.
	/// </summary>
	[SerializeField]
	[Tooltip("How fast will the spawned projectile move.")]
	[BoxGroup("Base Parameters")]
	private float BulletSpeed;

	/// <summary>
	/// The reference of the visual to use for the enemy projectile.
	/// </summary>
	[SerializeField]
    [Tooltip("The reference of the visual to use for the enemy projectile.")]
    [Required]
    [Space]
    private GameObject projectilePrefab;

	[SerializeField]
	[Tooltip("The reference of the visual effect config to use before shooting.")]
	private VisualEffectConfig anticipateVFX;

	[SerializeField]
	[Tooltip("The reference of the visual effect config to use when shooting.")]
	private VisualEffectConfig shootVFX;

    [SerializeField]
    private ScopedValue<Transform> playerPosition;

	[SerializeField]
	private EnemyBaseVariation baseVariation;

	[SerializeField]
	[Required]
	[HideIf("HasShotMarker")]
	private EnemyShotPositionMarker shotPos;

	private bool HasShotMarker => shotPos != null;

	[SerializeField]
	private UnityEvent onAnticipateAttack;

	[SerializeField]
	private UnityEvent onAttack;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyBulletSpeedMultiplier.
	/// </summary>
	[BoxGroup("Global Value References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyBulletSpeedMultiplier")]
	[Label("Scalar: Bullet Speed")]
	private GlobalValue<float> bulletSpeedMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyFireRateMultiplier.
	/// </summary>
	[BoxGroup("Global Value References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyFireRateMultiplier")]
	[Label("Scalar: Fire Rate")]
	private GlobalValue<float> firerateMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyDamageMultiplier.
	/// </summary>
	[BoxGroup("Global Value References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyDamageMultiplier")]
	[Label("Scalar: Damage")]
	private GlobalValue<float> damageMultiplier;

	private VisualEffect anticipateVFXObj;
	private bool didSpawnAnticipateVFX;





	[Button("Add Position Marker", EButtonEnableMode.Editor)]
	[HideIf("HasShotMarker")]
	public void GenerateShotPositionMarker()
	{
		if (transform.GetChild(1).TryGetComponent(out shotPos)) return;


		GameObject marker = new GameObject("Shot Position Marker");
		marker.transform.SetParent(transform);
		shotPos = marker.AddComponent<EnemyShotPositionMarker>();
	}

	private void Awake()
	{
		_attackDelay = 0.5f;
	}

	private void Start()
    {
		//Debug.Log("Spawn Shooting Enemy");
    }

    // Update is called once per frame
    void Update()
    {
		// Timer
		if (!baseVariation.CanAttack)
			return;

		if (_attackDelay <= 0)
		{
			SpawnProjectile();
			_resetAttackDelay();

			didSpawnAnticipateVFX = false;

			onAttack.Invoke();
		}
		else if (_attackDelay <= anticipateDuration.Value && !didSpawnAnticipateVFX)
        {
			anticipateVFXObj = anticipateVFX.Spawn(shotPos.transform);
			anticipateVFXObj.SetFloat("Duration", anticipateDuration.Value);
			Destroy(anticipateVFXObj.gameObject, anticipateDuration.Value);

			baseVariation.OnBodyshot.AddListener(_cleanUpAnticipateVFX);
			baseVariation.OnHeadshot.AddListener(_cleanUpAnticipateVFX);

			didSpawnAnticipateVFX = true;

			onAnticipateAttack.Invoke();
		}

		_attackDelay -= Time.deltaTime;
	}

	public override void StopAttacking()
	{
		if (anticipateVFXObj)
			anticipateVFX.Destroy(anticipateVFXObj);
	}

	private void _cleanUpAnticipateVFX()
    {
		if (anticipateVFXObj)
			anticipateVFX.Destroy(anticipateVFXObj);
    }

	private void _resetAttackDelay() => _attackDelay = Random.Range(AttackRate.x, AttackRate.y) / firerateMultiplier.Value;

	/// <summary>
	/// Instantiates a new projectile and sends it towards the player.
	/// </summary>
	/// <param name="origin">From where the projectile is to be spawned.</param>
	private void SpawnProjectile()
	{
		//Debug.Log("Tried shooting");

		Vector3 aimDirection = (playerPosition.Value.position - shotPos.transform.position).normalized;
		Rigidbody proj = Instantiate(projectilePrefab, shotPos.transform.position, Quaternion.FromToRotation(Vector3.forward, aimDirection)).GetComponent<Rigidbody>();

		proj.transform.GetComponent<EnemyBullet>().DamageToDeal = (int)(AttackDamage * damageMultiplier.Value);
		proj.AddForce(BulletSpeed * bulletSpeedMultiplier.Value * aimDirection);

		shootVFX.Spawn(shotPos.transform.position, shotPos.transform.rotation);
	}
}
