using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(menuName = "DifficultyPreset")]
public class DifficultyPreset : ScriptableObject, IComparable
{
	[DisableIf(nameof(_ReferenceLockFlag))] public int Difficulty;
	
	
	[DisableIf(nameof(_ReferenceLockFlag))][ShowAssetPreview] public Material DifficultyMaterial;
	[InfoBox("Please sync this up with your Spawner Weights", EInfoBoxType.Warning)]
	[DisableIf(nameof(_ReferenceLockFlag))] public bool HasStaticEnemies;
	[DisableIf(nameof(_ReferenceLockFlag))] public bool HasMovingEnemies;
	[DisableIf(nameof(_ReferenceLockFlag))] public bool HasShootingEnemies;


	#region Scalars
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Min(1)] public float EnemyBulletSpeed = 1;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Min(1)] public float EnemyMoveSpeed = 1;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Min(1)] public float EnemyFireRate = 1;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Min(1)] public float EnemyShotAnticipationTime = 1;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Min(1)] public float EnemyDamage = 1;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][Range(0, 1)] public float EnemyInvulChance = 0;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")] public DG.Tweening.Ease EnemyMoveEase = DG.Tweening.Ease.Linear;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")] public bool AutomaticDespawn = false;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Enemy Affecting")][ShowIf(nameof(AutomaticDespawn))] public float AutomaticDespawnTime = 5;
	#endregion

	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Level Affecting")] public int scoreGoal = 0;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Level Affecting")] public int maxActiveEnemies = 0;
	[InfoBox("The Number of Rounds the player stays in this difficulty. Mark the last Difficutly with -1 to let the player stayt here indefenitely.")]
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Level Affecting")][Min(-1)] public int roundsToLevelUp = 2;
	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Level Affecting")] public int timeInRound;


	[DisableIf(nameof(_ReferenceLockFlag))][BoxGroup("Level Affecting")] public List<SceneReference> AvailableRooms;

	[DisableIf(nameof(_ReferenceLockFlag))][Foldout("Spawner Weights")] public WeightedRandomSelector<EnemyConfigBase> OnlyStatic;
	[DisableIf(nameof(_ReferenceLockFlag))][Foldout("Spawner Weights")] public WeightedRandomSelector<EnemyConfigBase> OnlyMobile;
	[DisableIf(nameof(_ReferenceLockFlag))][Foldout("Spawner Weights")] public WeightedRandomSelector<EnemyConfigBase> OnlyPassive;
	[DisableIf(nameof(_ReferenceLockFlag))][Foldout("Spawner Weights")] public WeightedRandomSelector<EnemyConfigBase> OnlyActive;
	[HorizontalLine()]
	[DisableIf(nameof(_ReferenceLockFlag))][Foldout("Spawner Weights")] public WeightedRandomSelector<EnemyConfigBase> AllWeights;

	private void OnValidate()
	{
		OnlyStatic.ValidateFields();
		OnlyMobile.ValidateFields();
		OnlyPassive.ValidateFields();
		OnlyActive.ValidateFields();
		AllWeights.ValidateFields();

		AvailableRooms.Sort();
	}

	#region Magic Stuff

	private bool _ReferenceLockFlag;

	public int CompareTo(object obj)
	{
		DifficultyPreset other = obj as DifficultyPreset;
		return Difficulty.CompareTo(other.Difficulty);
	}

	[Button("(Un-)Lock Values")]
#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
	private void ToggleReferenceLock() { _ReferenceLockFlag = !_ReferenceLockFlag; }
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen 

	#endregion
}
