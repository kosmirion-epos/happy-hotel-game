using NaughtyAttributes;
using UnityEngine;
using UnityEditor;

public class EnemySpawner : ExtendedBehaviour
{
	/// <summary>
	/// Reference to the number of spawners currently active.
	/// </summary>
	[Tooltip("Reference to the number of spawners currently active.")]
	[SerializeField]
	[BoxGroup("Global Value References")]
	private GlobalValue<int> currentNumberOfFreeSpawners;

	/// <summary>
	/// Reference to the number of spawners, which have
	/// already had Update called on them this frame.
	/// </summary>
	[Tooltip("Reference to the number of spawners, which have " +
		"already had Update called on them this frame.")]
	[SerializeField]
	[BoxGroup("Global Value References")]
	private GlobalValue<int> numberOfUpdatedSpawnersThisFrame;

	/// <summary>
	/// Reference to the flag, which records whether a spawn has occurred each frame.
	/// </summary>
	[Tooltip("Reference to the flag, which records whether a spawn has occurred each frame.")]
	[SerializeField]
	[BoxGroup("Global Value References")]
	private GlobalValue<bool> hasSpawnOccurredThisFrame;

	/// <summary>
	/// Reference to the shared Maximum of enemies that all spawners together can have present at the same time.
	/// </summary>
	[Tooltip("Reference to the shared Maximum of enemies that all spawners together can have present at the same time.")]
	[SerializeField]
	[BoxGroup("Global Value References")]
	private GlobalValue<int> maxNumberOfEnemies;

	/// <summary>
	/// Reference to the shared number of enemies that all spawners currently have together.
	/// </summary>
	[Tooltip("Reference to the shared number of enemies that all spawners currently have together.")]
	[SerializeField]
	[BoxGroup("Global Value References")]
	private GlobalValue<int> currentNumberOfEnemies;

	/// <summary>
	/// The time until this spawner can produce a new enemy.
	/// </summary>
	[Tooltip("The time until this spawner can produce a new enemy.")]
	[SerializeField]
	[ReadOnly]
	[ShowIf("SpawnDelayNotZero")]
	[ProgressBar("Spawn Delay", "_startSpawnDelay", EColor.Violet)]
	private float currentSpawnDelay;

	/// <summary>
	/// How much time the delay initially was. In case we want indicators when an enemy may spawn again. Also for the progressbar on <see cref="currentSpawnDelay"/>.
	/// </summary>
	[HideInInspector]
	public float _startSpawnDelay;

	/// <summary>
	/// Describes the upper and lower bounds of the initial delay a spawner can get. X component is lower bounds, Y component is upper bounds.
	/// </summary>
	[Tooltip("Describes the upper and lower bounds of the initial delay a spawner can get. X component is lower bounds, Y component is upper bounds.")]
	[SerializeField]
	[MinMaxSlider(0.1f, 8)]
	private Vector2 initialDelayBounds;

	/// <summary>
	/// Holds all the different <see cref="EnemyConfigBase"/>s this <see cref="EnemySpawner"/> is allowed to spawn.
	/// </summary>
	[Tooltip("Holds all the different EnemyConfigBases this EnemySpawner is allowed to spawn.")]
	[SerializeField]
	[InfoBox("Don't edit weights while set to Global!", EInfoBoxType.Warning)]
	private ScopedValue<WeightedRandomSelector<EnemyConfigBase>> enemyConfigs;

	/// <summary>
	/// This spawners reference to its EnemyBase.
	/// </summary>
	[Tooltip("This spawners reference to its EnemyBase.")]
	[SerializeField]
	[Label("Enemy Reference")]
	private EnemyBaseVariation enemy;

	private GameObject _enemyInstance;
	/*################################################> Helper Properties <################################################*/

	#region Helper Properties

	/// <summary>
	/// Tells wether spawning is allowed for this spawner at this time, depends on Round state.
	/// </summary>
	[Tooltip("Tells wether spawning is allowed for this spawner at this time, depends on Round state.")]
	public bool SpawningAllowed
	{
		get => _spawningAllowed;
		set
		{
			//Debug.Log($"SpawningAllowed is now: {value}");
			_spawningAllowed = value;
		}
	}

	/// <summary>
	/// Private storage of of <see cref="SpawningAllowed"/>, needed to make the property work.
	/// </summary>
	private bool _spawningAllowed;

	/// <summary>
	/// Helper property to determine whether the spawn delay is shown in the inspector.
	/// </summary>
	private bool SpawnDelayNotZero => currentSpawnDelay != 0;

	/// <summary>
	/// Unifies the various conditions for spawning into one expression.
	/// 
	/// <br>Those are: System Confirmation for Spawning, the Spawndelay being 0, not already having an enemy and not too much enemies already spawned.</br>
	/// </summary>
	public bool CanSpawn => SpawningAllowed && !SpawnDelayNotZero && transform.childCount == 0 && (currentNumberOfEnemies.Value < maxNumberOfEnemies.Value);

	#endregion


	/*##################################################> Unity Methods <##################################################*/

	#region Unity Methods

	// Start is called before the first frame update
	void Start()
	{
		//Debug.Log("EnemySpawner got created.", gameObject);



		currentSpawnDelay = Random.Range(initialDelayBounds.x, initialDelayBounds.y);
		_startSpawnDelay = currentSpawnDelay;
	}

	// Update is called once per frame
	void Update()
	{
		if (SpawnDelayNotZero)
		{
			var aboveZero = currentSpawnDelay > 0;
			currentSpawnDelay -= Time.deltaTime;
			if (currentSpawnDelay <= 0)
			{
				//Debug.Log("Current SpawnDelay reached 0.");
				currentSpawnDelay = 0;

				if (aboveZero)
					++currentNumberOfFreeSpawners.Value;
			}
		}

		if (CanSpawn)
		{
			if (
				!hasSpawnOccurredThisFrame.Value &&
				Random.Range(0, currentNumberOfFreeSpawners.Value - numberOfUpdatedSpawnersThisFrame.Value) == 0
			)
			{
				SpawnEnemy();
				--currentNumberOfFreeSpawners.Value;
				hasSpawnOccurredThisFrame.Value = true;
			}

			++numberOfUpdatedSpawnersThisFrame.Value;
		}
	}

	/// <summary>
	/// Used in the Unity Editor to Draw the Holograms of the models. Could also use OnDrawGizmosSelected.
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.up);
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);

		//if (enemyConfigs. != 0)
		//{
		//Calculates and Shows one of the valid meshes of the first available config
		//Mesh toShow = enemyConfigs[0].PrefabOptions.GetRandomElement().GetComponent<MeshFilter>().sharedMesh;
		//Vector3 offset = new(0, toShow.bounds.min.y, 0);
		//Gizmos.DrawMesh(toShow, transform.position - offset, transform.rotation);
		//}
	}

	private void OnValidate()
	{
		if (enemyConfigs.IsValueAccessible)
			enemyConfigs.Value.ValidateFields();
	}

	#endregion

	/*##################################################> Other Methods <##################################################*/

	#region Other Methods
	/// <summary>
	/// Spawns an enemy and triggers its setup.
	/// Increases the tracker of current enemies.
	/// </summary>
	private void SpawnEnemy()
	{

		EnemyConfigBase _config = enemyConfigs.Value.GetRandomValue();
		_enemyInstance = Instantiate(_config.PrefabOptions.GetRandomElement(), transform);
		enemy = _enemyInstance.GetComponent<EnemyBaseVariation>();
		enemy.Setup(_config);
		enemy.EnemyDies.AddListener(EnemyDies);


		currentNumberOfEnemies.Value += 1;
	}

	/// <summary>
	/// Triggered by a <see cref="FloatUnityEvent"/> in <see cref="EnemyBase"/> when the enemy is hit.
	/// Starts the Cooldown and reduces the number of enemies again.
	/// </summary>
	/// <param name="cooldown">How long this spawner has to wait till it has to spawn its next enemy.</param>
	public void EnemyDies(float cooldown)
	{
		currentNumberOfEnemies.Value--;

		currentSpawnDelay = cooldown;
		_startSpawnDelay = cooldown;
	}

	/// <summary>
	/// Used by an <see cref="GlobalEventListener"/> to despawn all enemies on round end.
	/// </summary>
	public void DespawnEnemy()
	{
		//enemy.gameObject.SetActive(false);
	}

	#endregion
}
