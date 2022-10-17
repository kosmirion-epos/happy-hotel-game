using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct SceneReference : IComparable<SceneReference>
{
    public static SceneReference Invalid = -1;

    [Scene][AllowNesting][SerializeField] private int sceneID;

    public int SceneID
    {
        get => sceneID;
        set => sceneID = value;
    }

    public void Unset() => SceneID = -1;

    public bool IsSet() => SceneID >= 0 && SceneID < SceneManager.sceneCountInBuildSettings;

    public Scene GetScene() => SceneManager.GetSceneByBuildIndex(SceneID);

	public int CompareTo(SceneReference other)
	{
        return this.sceneID.CompareTo(other.sceneID);
	}

	public static implicit operator int(SceneReference s) => s.SceneID;
    public static implicit operator SceneReference(int i) => new() { SceneID = i };
}