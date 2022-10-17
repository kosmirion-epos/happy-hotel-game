using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region Saving Class
internal class HighscoreSaveData
{
	public List<HighScoreContainer> Highscores;


	internal void SaveData(string path)
	{
		string dataAsJson = JsonUtility.ToJson(this, true);

		File.WriteAllText(path, dataAsJson);
	}

	internal void LoadData(string path)
	{
		if (File.Exists(path))
		{
			string dataAsJson = File.ReadAllText(path);
			JsonUtility.FromJsonOverwrite(dataAsJson, this);
		}
	}

	public HighscoreSaveData()
	{

	}
	public HighscoreSaveData(string path)
	{
		LoadData(path);
	}
}

#endregion
#region Highscore Class

[System.Serializable]
public class HighScoreContainer : IComparable<HighScoreContainer>/*, ISerializationCallbackReceiver*/
{
	public string PlayerName;
	//public DateTime TimeAchieved;
	public string TimeAchievedAsString;
	public int Score;

	public int CompareTo(HighScoreContainer other)
	{
		if (other == null)
		{
			return 1;
		}
		else
		{
			// Inverted should make it so biggest first?
			return other.Score.CompareTo(this.Score);
			//return this.Score.CompareTo(other.Score);
		}
	}

	//public void OnBeforeSerialize()
	//{
	//	TimeAchievedAsString = TimeAchieved.ToString();
	//}

	//public void OnAfterDeserialize()
	//{
	//	TimeAchieved = DateTime.Parse(TimeAchievedAsString);
	//}

	public HighScoreContainer(string playerName/*, DateTime timeAchieved*/, string timeAchievedString, int score)
	{
		PlayerName = playerName;
		//TimeAchieved = timeAchieved;
		TimeAchievedAsString = timeAchievedString;
		Score = score;
	}

	public override string ToString()
	{
		return $"Score: {Score}";
	}
}


#endregion

public class HighscoreSaveManager : MonoBehaviour
{
	[SerializeField][Required] private GlobalValue<List<HighScoreContainer>> highScores;
	[SerializeField][Required] private GlobalValue<int> maxScore;

	[SerializeField][ReadOnly] string dataSavePath;

	private bool _scoresLoaded = false;

	private void Awake()
	{
		// hhhs = Happy Hotel High Scores
		dataSavePath = Application.persistentDataPath + "/Highscores.hhhs";
	}

	private void OnEnable()
	{
		LoadData();

		_scoresLoaded = true;
	}

	private void OnDestroy()
	{
		SaveData();
	}

	private void SaveData()
	{
		HighscoreSaveData data = new HighscoreSaveData()
		{
			Highscores = highScores.Value
		};

		data.SaveData(dataSavePath);
	}

	[Button]
	private void LoadData()
	{
		Debug.Log("Trying to load data");
		HighscoreSaveData data = new HighscoreSaveData(dataSavePath);

		highScores.Value = data.Highscores;

		if (highScores.Value == null) highScores.Value = new List<HighScoreContainer>();
	}


	public void TryAddScore()
	{
		HighScoreContainer toAdd = new HighScoreContainer(/*playerNameDropdown.captionText.text*/""/*, DateTime.Now*/, DateTime.Now.ToString(), maxScore.Value);

		if (highScores.Value.Count >= 10)
		{
			List<HighScoreContainer> scoresPlusOne = new List<HighScoreContainer>(highScores.Value)
			{
				toAdd
			};

			scoresPlusOne.Sort();

			// if new score makes top 10
			if (scoresPlusOne.FindIndex(x => x == toAdd) != scoresPlusOne.Count - 1)
			{
				highScores.Value.Add(toAdd);
				highScores.Value.Sort();
				highScores.Value = highScores.Value.GetRange(0, 10);
			}
		}
		else
		{
			highScores.Value.Add(toAdd);
			highScores.Value.Sort();
		}

		string p = "";
		foreach (var score in highScores.Value)
		{
			p += score.ToString() + "\n";
		}

		Debug.Log($"After Trying to Add: \n{p}");
	}

	//[Button]
	public void DebugTryAddScore()
	{
		Debug.Log("Loaded Data, trying to add score");

		HighScoreContainer toAdd = new HighScoreContainer(/*playerNameDropdown.captionText.text*/""/*, DateTime.Now*/, DateTime.Now.ToString(), maxScore.Value);

		if (highScores.Value.Count >= 10)
		{
			Debug.Log("Already more than 10 scores, need to check");

			List<HighScoreContainer> scoresPlusOne = new List<HighScoreContainer>(highScores.Value)
			{
				toAdd
			};
			string s = "";
			foreach (var score in scoresPlusOne)
			{
				s += score.ToString() + "\n";
			}

			Debug.Log($"Before .Sort(): \n{s}");

			scoresPlusOne.Sort();

			s = "";
			foreach (var score in scoresPlusOne)
			{
				s += score.ToString() + "\n";
			}

			Debug.Log($"After .Sort(): \n{s}");

			// if new score makes top 10
			if (scoresPlusOne.FindIndex(x => x == toAdd) != scoresPlusOne.Count - 1)
			{
				highScores.Value.Add(toAdd);
				highScores.Value.Sort();
				highScores.Value = highScores.Value.GetRange(0, 10);
			}
		}
		else
		{
			highScores.Value.Add(toAdd);
			highScores.Value.Sort();
		}

		string p = "";
		foreach (var score in highScores.Value)
		{
			p += score.ToString() + "\n";
		}

		Debug.Log($"After Trying to Add: \n{p}");
	}
}
