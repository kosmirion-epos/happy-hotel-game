using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DifficultyWindow : EditorWindow
{
	private GlobalValue<List<GlobalDifficultyList>> metaDifficulties;

	private bool _editable;

	private int _selectedDiff;

	private Vector2 _diffSelectButtonsScrollPos;
	private Vector2 _mainStatsScrollPos;

	[MenuItem("Window/Difficulty Inspector")]
	public static void ShowWindow()
	{
		GetWindow<DifficultyWindow>("Difficulty Inspector");
	}


	private void OnGUI()
	{
		EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Set of Difficulties to view and edit", EditorStyles.largeLabel);
			metaDifficulties = EditorGUILayout.ObjectField(metaDifficulties, typeof(GlobalValue<List<GlobalDifficultyList>>), false) as GlobalValue<List<GlobalDifficultyList>>;

			if (GUILayout.Button("EditLock", EditorStyles.miniButtonRight, GUILayout.Width(55))) _editable = !_editable;
		}
		GUILayout.EndHorizontal();


		if (metaDifficulties == null || metaDifficulties.InitialValue == null || metaDifficulties.InitialValue.Any(x => x.InitialValue == null)) return;

		EditorGUI.BeginChangeCheck();

		EditorGUILayout.Space(20);

		MakeDifficultyButtons();

		EditorGUILayout.Space(10);

		EditorGUI.indentLevel++;

		_mainStatsScrollPos = GUILayout.BeginScrollView(_mainStatsScrollPos);
		{
			MakeDifficultyAssetFields();

			GUILayout.Space(5);

			MakeDifficultyMaterialFields();

			GUILayout.Space(5);

			MakeEnemyTypeToggles();

			GUILayout.Space(5);

			MakeEnemyStatsFields();

			GUILayout.Space(5);

			MakeRoundStatsFields();
		}
		EditorGUILayout.EndScrollView();

		EditorGUI.indentLevel--;

		EditorGUI.EndChangeCheck();
	}

	private void MakeDifficultyAssetFields()
	{
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Difficulty Assets", EditorStyles.boldLabel, GUILayout.MinWidth(EditorGUIUtility.labelWidth - 20));

			if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
			{
				AddNewDifficultyToMetaDifficult();
			}

			for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
			{
				EditorGUI.BeginDisabledGroup(true);
				{
					float temp = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 25;
					EditorGUILayout.PrefixLabel(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].Difficulty.ToString(), EditorStyles.miniLabel);
					EditorGUIUtility.labelWidth = temp;

					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i] = EditorGUILayout.ObjectField(
						metaDifficulties.InitialValue[_selectedDiff].InitialValue[i],
						typeof(DifficultyPreset),
						false,
						GUILayout.MinWidth(100))
						as DifficultyPreset;
				}
				EditorGUI.EndDisabledGroup();

				//if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20)))
				//{
				//	DeleteDifficulty(i);
				//}
			}
		}
		GUILayout.EndHorizontal();
	}

	private void AddNewDifficultyToMetaDifficult()
	{
		string x = metaDifficulties.InitialValue[_selectedDiff].DisplayName;
		int y = metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count + 1;

		DifficultyPreset newDifficulty = CreateInstance<DifficultyPreset>();
		newDifficulty.Difficulty = y;
		newDifficulty.roundsToLevelUp = -1;
		metaDifficulties.InitialValue[_selectedDiff].InitialValue[^1].roundsToLevelUp =
			metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count >= 2 ?
			metaDifficulties.InitialValue[_selectedDiff].InitialValue[^2].roundsToLevelUp + 1 : 1;


		AssetDatabase.CreateAsset(newDifficulty, $"Assets/ScriptableObjects/Difficulty Stuff/Presets/{x}/{x} Difficulty {y}.asset");

		metaDifficulties.InitialValue[_selectedDiff].InitialValue.Add(newDifficulty);
		EditorGUIUtility.PingObject(newDifficulty);

		Debug.Log("New Difficulty created.", newDifficulty);
	}

	private void AddNewMetaDifficulty()
	{

	}

	private void DeleteDifficulty(int i)
	{
		//Remember ARE YOU SURE POPUP

		Debug.LogWarning($"tried to delete difficulty {i}. For now, please do it normally and don't forget to clean up in the Difficulty List where the reference may be missing now.");
	}

	private void MakeRoundStatsFields()
	{
		GUILayout.Label("Round related Stats", EditorStyles.boldLabel);

		EditorGUI.BeginDisabledGroup(_editable);
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("ScoreGoal");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].scoreGoal = EditorGUILayout.IntField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].scoreGoal, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Maximum Active Enemies");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].maxActiveEnemies = EditorGUILayout.IntField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].maxActiveEnemies, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Rounds Until Next Difficulty");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].roundsToLevelUp = EditorGUILayout.IntField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].roundsToLevelUp, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Alloted Roundtime");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].timeInRound = EditorGUILayout.IntField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].timeInRound, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
		}
		EditorGUI.EndDisabledGroup();
	}

	private void MakeEnemyStatsFields()
	{
		GUILayout.Label("Enemy Stat Modifiers", EditorStyles.boldLabel);

		EditorGUI.BeginDisabledGroup(_editable);
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Bullet Speed");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyBulletSpeed = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyBulletSpeed, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Move Speed");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyMoveSpeed = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyMoveSpeed, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Fire Rate");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyFireRate = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyFireRate, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Shot Anticipation Time");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyShotAnticipationTime = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyShotAnticipationTime, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Damage");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyDamage = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyDamage, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Invul Chance");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyInvulChance = EditorGUILayout.Slider(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyInvulChance, 0, 1, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Movement Ease");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyMoveEase = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].EnemyMoveEase, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Use Auto Despawn");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].AutomaticDespawn = EditorGUILayout.Toggle(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].AutomaticDespawn, GUILayout.MinWidth(100));
				}
			}
			GUILayout.EndHorizontal();
			if (metaDifficulties.InitialValue[_selectedDiff].InitialValue.Any(x => x.AutomaticDespawn == true))
			{
				GUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Auto Despawn Time");

					for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
					{
						EditorGUI.BeginDisabledGroup(!metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].AutomaticDespawn);
						{
							metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].AutomaticDespawnTime = EditorGUILayout.FloatField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].AutomaticDespawnTime, GUILayout.MinWidth(100));
						}
						EditorGUI.EndDisabledGroup();
					}
				}
				GUILayout.EndHorizontal(); 
			}
		}
		EditorGUI.EndDisabledGroup();
	}

	private void MakeEnemyTypeToggles()
	{
		GUILayout.Label("Enemy Types", EditorStyles.boldLabel);

		EditorGUI.BeginDisabledGroup(_editable);
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Has Static Enemies");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasStaticEnemies = EditorGUILayout.Toggle(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasStaticEnemies);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Has Moving Enemies");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasMovingEnemies = EditorGUILayout.Toggle(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasMovingEnemies);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Has Shooting Enemies");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasShootingEnemies = EditorGUILayout.Toggle(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].HasShootingEnemies);
				}
			}
			GUILayout.EndHorizontal();
		}
		EditorGUI.EndDisabledGroup();
	}

	private void MakeDifficultyButtons()
	{
		GUILayout.BeginHorizontal();
		{
			_diffSelectButtonsScrollPos = GUILayout.BeginScrollView(_diffSelectButtonsScrollPos, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight + 5));
			{
				GUILayout.BeginHorizontal();
				{
					for (int i = 0; i < metaDifficulties.InitialValue.Count; i++)
					{
						if (GUILayout.Button(metaDifficulties.InitialValue[i].DisplayName, GUILayout.MaxWidth(90)))
						{
							_selectedDiff = i;
						}
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(17)))
			{
				Debug.Log("Try to make a new metadifficulty");
				Debug.LogWarning("Currently not supported");
			}
		}
		GUILayout.EndHorizontal();
	}

	private void MakeRoomsFields()
	{
		GUILayout.Label("Available Rooms", EditorStyles.boldLabel);

		EditorGUI.BeginDisabledGroup(_editable);
		{
			GUILayout.BeginHorizontal();
			{
				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					EditorGUILayout.PropertyField(new SerializedObject(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i]).FindProperty("AvailableRooms"), true);
				}
			}
			GUILayout.EndHorizontal();
		}
		EditorGUI.EndDisabledGroup();
	}

	private void MakeDifficultyMaterialFields()
	{
		EditorGUI.BeginDisabledGroup(_editable);
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");

				for (int i = 0; i < metaDifficulties.InitialValue[_selectedDiff].InitialValue.Count; i++)
				{
					metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].DifficultyMaterial = EditorGUILayout.ObjectField(metaDifficulties.InitialValue[_selectedDiff].InitialValue[i].DifficultyMaterial, typeof(Material), false, GUILayout.MinWidth(100)) as Material;
				}
			}
			GUILayout.EndHorizontal();
		}
		EditorGUI.EndDisabledGroup();
	}
}