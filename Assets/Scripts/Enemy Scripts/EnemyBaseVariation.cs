using DG.Tweening;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class FloatUnityEvent : UnityEvent<float> { }
public class EnemyBaseVariation : ExtendedBehaviour, IShootable
{
	/// <summary>
	/// The <see cref="EnemyConfigBase"/> the enemy is using.
	/// </summary>
	[Expandable]
	[SerializeField]
	private EnemyConfigBase config;

	/// <summary>
	/// How long the enemy is currently standing there.
	/// </summary>
	[HideIf("HasNoConfig")]
	[ProgressBar("How long 'til disappearance", nameof(MaxStayDuration), EColor.Red, order = 0)]
	public float currentStandingDuration;

	/// <summary>
	/// Reference to the shared flag if Enemies should despawn automatically.
	/// </summary>
	[Tooltip("Reference to the shared flag if Enemies should despawn automatically. If true, enemies despawn.")]
	[Label("Flag: Enemy Despawn")]
	[BoxGroup("Global Value References")]
	[SerializeField]
	private ScopedValue<bool> AutomaticDespawnFlag;

	[BoxGroup("Global Value References")]
	[SerializeField]
	private ScopedValue<Quaternion> preSpawnRotation;

	[BoxGroup("Global Value References")]
	[SerializeField]
	private ScopedValue<float> spawnDuration;

	[SerializeField]
	private GlobalEvent roundEndEvent;

	/// <summary>
	/// The Corresponding <see cref="EnemySpawner"/> susbcribes to this via Code.
	/// </summary>
	public FloatUnityEvent EnemyDies = new FloatUnityEvent();
	public UnityEvent OnEnemyFinishedStandingUp;
	public UnityEvent OnHeadshot;
	public UnityEvent OnBodyshot;

	public bool CanAttack { get; private set; } = true;
	private bool isDead;

	[HorizontalLine(color: EColor.Blue, order = 0)]

	[SerializeField]
	[Required("Please add a Movement behaviour.")]
	[HideIf("HasMovement")]
	private MovementType movementType;

	[SerializeField]
	[Required("Please add an Attack behaviour.")]
	[HideIf("HasAttack")]
	private AttackType attackType;

	private void OnValidate()
	{
		TryGetComponent(out movementType);
		TryGetComponent(out attackType);
	}

	#region Proxy / Helper Properties
	/// <summary>
	/// Proxy necessary for the Progressbar on <see cref="currentStandingDuration"/>
	/// </summary>
	[HideInInspector]
	public float MaxStayDuration => !HasNoConfig ? config.StayDuration : 0;

	/// <summary>
	/// Another small proxy check method
	/// </summary>
	public bool HasNoConfig => config == null;

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
	private bool HasMovement => movementType != null;
	private bool HasAttack => attackType != null;
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen

	private void Awake()
	{
		if (EnemyDies == null)
		{
			EnemyDies = new FloatUnityEvent();
		}

		UnityEvent onRoundEnd = new();
		onRoundEnd.AddListener(Disappear);
		onRoundEnd.AddListener(() => roundEndEvent.RemoveListener(onRoundEnd));

		roundEndEvent.AddListener(onRoundEnd);
	}

	private void Start()
	{
		var rot = transform.rotation;
		transform.rotation *= preSpawnRotation.Value;
		transform.DORotateQuaternion(rot, spawnDuration.Value).SetEase(Ease.OutCubic).OnComplete(() => OnEnemyFinishedStandingUp.Invoke());

		WithDelay(spawnDuration.Value, () => { if (!isDead) CanAttack = true; });
	}
	#endregion

	[Button]
	public void UpdateFields()
	{
		OnValidate();
	}

	/// <summary>
	/// Called upon receiving a hit from a weapon. 
	/// <para>
	///		Creates the decal (point banner usually)<br></br>
	///		Delegates to <see cref="HitCommon"/> afterwards.
	///	</para>
	/// </summary>
	/// <param name="hitLocation"></param>
	/// <returns>The Amount of points awarded.</returns>
	public int Hit(Vector3 hitLocation)
	{
		if (isDead)
			return 0;

		int pointsAwarded = config.Hit(hitLocation);

		GameObject x = Instantiate(config.hitmarkerDecal, hitLocation, transform.rotation);
		x.GetComponentInChildren<TextMeshProUGUI>().text = $"+{pointsAwarded}";

		if (config.DoublePointsFlag.Value)
			config.hit2xVFX.Spawn(hitLocation, transform.rotation);
		else
			config.hitVFX.Spawn(hitLocation, transform.rotation);

		OnBodyshot.Invoke();

		HitCommon();



		return pointsAwarded;
	}

	/// <summary>
	/// Delegates actions to take after a Headshot. Also see <see cref="Hit(Vector3)"/>
	/// </summary>
	/// <param name="hitLocation"></param>
	/// <returns>The Amount of points awarded.</returns>
	public int HitHead(Vector3 hitLocation)
	{
		if (isDead)
			return 0;

		int pointsAwarded = config.HitHead(hitLocation);

		GameObject x = Instantiate(config.hitmarkerDecal, hitLocation, transform.rotation);
		x.GetComponentInChildren<TextMeshProUGUI>().text = $"+{pointsAwarded}";

		if (config.DoublePointsFlag.Value)
			config.hitHead2xVFX.Spawn(hitLocation, transform.rotation);
		else
			config.hitHeadVFX.Spawn(hitLocation, transform.rotation);

		OnHeadshot.Invoke();

		HitCommon();

		return pointsAwarded;
	}

	/// <summary>
	/// Contains the shared actions taken by both <see cref="Hit(Vector3)"/> and <see cref="HitHead(Vector3)"/>
	///  <para>
	///		Plays the defined sounds.<br></br>
	///		Triggers the base <see cref="EnemyConfigBase.Hit(Vector3)"/> method.<br></br>
	///		Notifies the corresponding spawner of the death of this enemy.<br></br>
	///		Clears this Enemy afterwards.
	///	</para>
	/// </summary>
	/// <param name="hitLocation"></param>
	private void HitCommon()
	{
		CanAttack = false;

		foreach (Collider c in GetComponentsInChildren<Collider>())
		{
			c.enabled = false;
		}


		isDead = true;

		//Despawn(false);
	}

	/// <summary>
	/// Called by the <see cref="EnemySpawner"/> upon spawning an enemy.
	/// Applies meshes and positions and readies the enemy for use.
	/// </summary>
	/// <param name="configBase"></param>
	public void Setup(EnemyConfigBase configBase)
	{
		transform.localPosition = Vector3.zero;

		config = configBase;

		currentStandingDuration = 0;

		gameObject.SetActive(true);
	}

	private void Update()
	{
		if (AutomaticDespawnFlag.Value)
		{
			//Time tracking
			currentStandingDuration += Time.deltaTime;

			if (currentStandingDuration >= config.StayDuration)
			{
				Despawn();
			}
		}
	}

	public void Disappear()
	{
		CanAttack = false;
		attackType.StopAttacking();

		if (!isDead)
		{
			isDead = true;
			transform.DORotateQuaternion(transform.rotation * preSpawnRotation.Value, spawnDuration.Value).SetEase(Ease.OutCubic);
		}
	}

	public void Despawn(bool useStandardDeathCD = true)
	{
		//gameObject.SetActive(false);


		//Currently Hardcoded wait time for new enemy. Waiting on design answer. TODO
		EnemyDies.Invoke(useStandardDeathCD ? 0.6f : UnityEngine.Random.Range(config.CoolDownAfterDeath.x, config.CoolDownAfterDeath.y));
		Destroy(gameObject);

		config = null;
	}
}
