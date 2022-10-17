using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreObjectLoader : MonoBehaviour
{
	[SerializeField] private GlobalValue<List<HighScoreContainer>> highScores;
	
	[SerializeField][Required] private GameObject templatePrefab;


	//[SerializeField] private TMPro.TMP_Dropdown playerNameDropdown;
	//[SerializeField] private TMPro.TMP_InputField playerNameInputField;



	private List<GameObject> scoreObjects = new List<GameObject>();

	private void Start()
	{
		GenerateUI();
	}



	private void GenerateUI()
	{
		if (highScores.Value == null) return;

		highScores.Value.Sort();

		foreach (var score in highScores.Value)
		{
			HighscoreReferenceHolder x = Instantiate(templatePrefab, transform).GetComponent<HighscoreReferenceHolder>();

			x.HighscoreHolderName.text = score.PlayerName;
			x.DateAchieved.text = score.TimeAchievedAsString;
			x.Score.text = score.Score.ToString();

			scoreObjects.Add(x.gameObject);
		}
	}

	//private void UpdateScores()
	//{
	//	for (int i = scoreObjects.Count; i > 0; --i)
	//	{
	//		Destroy(scoreObjects[i]);
	//	}

	//	scoreObjects.Clear();

	//	if (highScores == null) return;

	//	highScores.Value.Sort();

	//	foreach (var score in highScores.Value)
	//	{
	//		HighscoreReferenceHolder x = Instantiate(templatePrefab, transform).GetComponent<HighscoreReferenceHolder>();

	//		x.HighscoreHolderName.text = score.PlayerName;
	//		x.DateAchieved.text = score.TimeAchieved.ToString();
	//		x.Score.text = score.Score.ToString();

	//		scoreObjects.Add(x.gameObject);
	//	}
	//}
}