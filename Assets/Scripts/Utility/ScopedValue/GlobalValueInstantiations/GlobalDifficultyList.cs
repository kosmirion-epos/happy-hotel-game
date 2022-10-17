using UnityEngine;

[CreateAssetMenu(menuName = "Global Value/Global Difficulty List")]
public class GlobalDifficultyList : GlobalList<DifficultyPreset> 
{
	public string DisplayName;
}
