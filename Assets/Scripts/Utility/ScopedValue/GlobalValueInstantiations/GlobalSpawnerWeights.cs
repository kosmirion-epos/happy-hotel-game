using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Value/Global SpawnerWeights")]
public class GlobalSpawnerWeights : GlobalValue<WeightedRandomSelector<EnemyConfigBase>>
{
	protected override WeightedRandomSelector<EnemyConfigBase> GetEquivalentInstance(WeightedRandomSelector<EnemyConfigBase> t)
	{
		return new WeightedRandomSelector<EnemyConfigBase>(new(Value.SpawnObjects));
	}

	private void OnValidate()
	{
		InitialValue.ValidateFields();
		Value.ValidateFields();
	}
}