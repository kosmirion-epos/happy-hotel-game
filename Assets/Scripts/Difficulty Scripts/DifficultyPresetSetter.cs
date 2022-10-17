using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPresetSetter : MonoBehaviour
{
	#region Enemy Refs

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyBulletSpeedMultiplier.
	/// </summary>
	[BoxGroup("References")]
	[Header("Enemy")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyBulletSpeedMultiplier")]
	[Label("Scalar: Bullet Speed")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> bulletSpeedMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyFireRateMultiplier.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyFireRateMultiplier")]
	[Label("Scalar: Fire Rate")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> firerateMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyShotAnticipationTime.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyShotAnticipationTime")]
	[Label("Anticipation Time")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> enemyShotAnticipationTime;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyDamageMultiplier.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyDamageMultiplier")]
	[Label("Scalar: Damage")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> damageMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyAegisChance.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyAegisChance")]
	[Label("Chance: Enemy Aegis")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> invulerabilityChance;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyMovementSpeedMultiplier.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyMovementSpeedMultiplier")]
	[Label("Scalar: Movement Speed")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> moveSpeedMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyMovementEase.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyMovementEase")]
	[Label("Movement Ease")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<DG.Tweening.Ease> enemyMoveEase;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyAutoDespawnFlag.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyAutoDespawnFlag")]
	[Label("Use Auto Despawn")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<bool> enemyAutoDespawn;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyAutoDespawnTime.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyAutoDespawnTime")]
	[Label("Auto Despawn Time")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> enemyAutoDespawnTime;

	#endregion

	#region Level Refs

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the Levels.
	/// </summary>
	[BoxGroup("References")]
	[Header("Levels")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the Levels")]
	[Label("Levels List")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<List<SceneReference>> roomSceneIDs;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the ScoreGoal.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the ScoreGoal")]
	[Label("Limit: Score Needed")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<int> scoreGoal;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the MaxNumberOfEnemies.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the MaxNumberOfEnemies")]
	[Label("Limit: Active Enemies")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<int> maxActiveEnemies;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the Round Duration.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the Round Duration")]
	[Label("Round Duration")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<float> roundDuration;

	#endregion

	#region Spawner Weight Refs

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the OnlyStatic Weights.
	/// </summary>
	[BoxGroup("References")]
	[Header("Spawner Weigths")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the OnlyStatic Weights.")]
	[Label("Only Static")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<WeightedRandomSelector<EnemyConfigBase>> onlyStatic;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the OnlyMobile Weights.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the OnlyMobile Weights.")]
	[Label("Only Mobile")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<WeightedRandomSelector<EnemyConfigBase>> onlyMobile;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the OnlyPassive Weights.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the OnlyPassive Weights.")]
	[Label("Only Passive")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<WeightedRandomSelector<EnemyConfigBase>> onlyPassive;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the OnlyActive Weights.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the OnlyActive Weights.")]
	[Label("Only Active")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<WeightedRandomSelector<EnemyConfigBase>> onlyActive;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for all Weights.
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for All Weights.")]
	[Label("All Weights")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<WeightedRandomSelector<EnemyConfigBase>> allWeights;

	#endregion

	#region Other Refs

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the RoundCount.
	/// </summary>
	[BoxGroup("References")]
	[Header("Others")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the RoundCount")]
	[Label("Round Count")]
	[Required("Please set all References")]
	[DisableIf(nameof(_ReferenceLockFlag))]
	private GlobalValue<int> roundCount;

	/// <summary>
	/// Starts at 1, need to adjust in list accesses
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Required("Please set all References")]
	private GlobalValue<int> currentDifficulty;

	/// <summary>
	/// Starts at 1, need to adjust in list accesses
	/// </summary>
	[BoxGroup("References")]
	[SerializeField]
	[Required("Please set all References")]
	private GlobalValue<int> currentMetaDifficulty;

	#endregion

	private int _roundsInDifficulty = 0;
	private int _currentRoundThreshold = 0;


	[SerializeField]
	[Required]
	private GlobalValue<List<GlobalDifficultyList>> metaDifficulties;

	[SerializeField]
	[VariableBoundsRange(1, nameof(MetaDiffCount))]
	[OnValueChanged(nameof(MetaDiffAdjusterHelper))]
	[Label("Meta Difficulty")]
	private int currentMetaDifficultySliderProxy;

	[SerializeField]
	[ReadOnly]
	[Label("Meta Difficulty Description")]
	private string currentMetaDifficultyLabel;

	[SerializeField]
	[VariableBoundsRange(1, nameof(DiffCount))]
	[OnValueChanged(nameof(DiffAdjusterHelper))]
	[Label("Difficulty Level")]
	private int currentDifficultySliderProxy;



	public void TryNextDifficutly()
	{
		if (_currentRoundThreshold == -1)
		{
			_roundsInDifficulty++;
			return;
		}

		if (++_roundsInDifficulty >= _currentRoundThreshold) // BAD CODE! the ++ is very hidden and not indicated by any name or something else
		{
			ApplyDifficultyPreset(++currentDifficulty.Value);
		}
	}

	public void ApplyDifficultyPreset(int difficulty)
	{
		int adjustedDifficulty = Mathf.Min(metaDifficulties.Value[currentMetaDifficulty.Value - 1].Value.Count, difficulty);

		DifficultyPreset preset = metaDifficulties.Value[currentMetaDifficulty.Value - 1].Value[adjustedDifficulty - 1];

		bulletSpeedMultiplier.Value = preset.EnemyBulletSpeed;
		moveSpeedMultiplier.Value = preset.EnemyMoveSpeed;
		firerateMultiplier.Value = preset.EnemyFireRate;
		enemyShotAnticipationTime.Value = preset.EnemyShotAnticipationTime;
		damageMultiplier.Value = preset.EnemyDamage;
		invulerabilityChance.Value = preset.EnemyInvulChance;
		enemyMoveEase.Value = preset.EnemyMoveEase;
		enemyAutoDespawn.Value = preset.AutomaticDespawn;
		enemyAutoDespawnTime.Value = preset.AutomaticDespawnTime;

		scoreGoal.Value = preset.scoreGoal;
		maxActiveEnemies.Value = preset.maxActiveEnemies;

		roundDuration.Value = preset.timeInRound;


		roomSceneIDs.Value = preset.AvailableRooms;

		onlyStatic.Value = preset.OnlyStatic;
		onlyMobile.Value = preset.OnlyMobile;
		onlyPassive.Value = preset.OnlyPassive;
		onlyActive.Value = preset.OnlyActive;
		allWeights.Value = preset.AllWeights;

		//Settle Stuff for in-game difficulty preset adjusting
		_currentRoundThreshold = preset.roundsToLevelUp;
		_roundsInDifficulty = 0;
 	}

	[Button("Apply Preset")]
	private void ApplyDifficultyPreset()
	{
		int difficulty = Mathf.Min(metaDifficulties.Value[currentMetaDifficulty.Value - 1].Value.Count, currentDifficulty.Value);

		var preset = metaDifficulties.Value[currentMetaDifficulty.Value - 1].Value[difficulty - 1];

		bulletSpeedMultiplier.Value = preset.EnemyBulletSpeed;
		moveSpeedMultiplier.Value = preset.EnemyMoveSpeed;
		firerateMultiplier.Value = preset.EnemyFireRate;
		enemyShotAnticipationTime.Value = preset.EnemyShotAnticipationTime;
		damageMultiplier.Value = preset.EnemyDamage;
		invulerabilityChance.Value = preset.EnemyInvulChance;
		enemyMoveEase.Value = preset.EnemyMoveEase;
		enemyAutoDespawn.Value = preset.AutomaticDespawn;
		enemyAutoDespawnTime.Value = preset.AutomaticDespawnTime;

		scoreGoal.Value = preset.scoreGoal;
		maxActiveEnemies.Value = preset.maxActiveEnemies;

		roundDuration.Value = preset.timeInRound;


		onlyStatic.Value = preset.OnlyStatic;
		onlyMobile.Value = preset.OnlyMobile;
		onlyPassive.Value = preset.OnlyPassive;
		onlyActive.Value = preset.OnlyActive;
		allWeights.Value = preset.AllWeights;

		roomSceneIDs.Value = preset.AvailableRooms;
		_currentRoundThreshold = preset.roundsToLevelUp;
		_roundsInDifficulty = 0;
	}


	#region Magic Stuff

	private bool _ReferenceLockFlag;

	[ShowNativeProperty]
	private int RoundCount => roundCount != null ? roundCount.Value : 0;


	[Button]
	private void ToggleReferenceLock() { _ReferenceLockFlag = !_ReferenceLockFlag; }
	private int MetaDiffCount => metaDifficulties.InitialValue.Count;
	private void MetaDiffAdjusterHelper()
	{
		currentMetaDifficulty.Value = currentMetaDifficultySliderProxy;
		currentMetaDifficultyLabel = $"{metaDifficulties.InitialValue[currentMetaDifficultySliderProxy - 1].DisplayName} - Difficulties: {metaDifficulties.InitialValue[currentMetaDifficultySliderProxy - 1].InitialValue.Count}";
	}

	private void DiffAdjusterHelper()
	{
		currentDifficulty.Value = currentDifficultySliderProxy;
	}

	private int DiffCount => metaDifficulties.InitialValue[currentMetaDifficulty.Value >= 1 ? currentMetaDifficulty.Value - 1 : 0].InitialValue.Count;

	#endregion
}
