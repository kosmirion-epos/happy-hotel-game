using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatsAssigner : MonoBehaviour
{
	[SerializeField] private GlobalValue<List<GlobalDifficultyList>> metaDifficulties;

	[SerializeField] private GlobalValue<int> currentDifficulty;

	[SerializeField] private GlobalValue<int> currentMetaDifficulty;

	[SerializeField] private TMP_Text pointGoal;
	[SerializeField] private TMP_Text levelTime;

	[SerializeField] private Image difficultyIcon;

	[SerializeField] private Image enemyTypeStatic;
	[SerializeField] private Image enemyTypeMoving;
	[SerializeField] private Image enemyTypeShooting;

	private void OnEnable()
	{
		Apply();
	}

	public void Apply()
	{
		DifficultyPreset preset = metaDifficulties.Value[currentMetaDifficulty.Value - 1].Value[currentDifficulty.Value - 1];

		pointGoal.text = preset.scoreGoal.ToString();
		levelTime.text = preset.timeInRound.ToString() + " Seconds";

		difficultyIcon.material = preset.DifficultyMaterial;

		enemyTypeStatic.enabled = preset.HasStaticEnemies;
		enemyTypeMoving.enabled = preset.HasMovingEnemies;
		enemyTypeShooting.enabled = preset.HasShootingEnemies;
	}
}
