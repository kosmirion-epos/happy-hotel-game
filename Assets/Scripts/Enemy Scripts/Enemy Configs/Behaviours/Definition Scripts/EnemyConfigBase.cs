using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the definition of the scriptable object that holds the configuration of the enemies that get's loaded by the <see cref="EnemyBase"/>
/// </summary>
[CreateAssetMenu(menuName = "Enemy Config")]
public class EnemyConfigBase : ScriptableObject, IShootable
{
	/// <summary>
	/// The amount of points this enemy is worth when killed.
	/// </summary>
	[HorizontalLine(color: EColor.Blue, order = 1)]
	[BoxGroup("Points Related Values and References")]
	[Tooltip("The amount of points this enemy is worth when killed.")]
	public int PointsWorth;

	/// <summary>
	/// The amount <see cref="PointsWorth"/> will be multiplied with when the player scores a headshot.
	/// <para> Restricted to Numbers &gt;= 1.</para>
	/// </summary>
	[BoxGroup("Points Related Values and References")]
	[Tooltip("The amount PointsWorth will be multiplied with when the player scores a headshot.")]
	[Min(1)]
	[OnValueChanged("RoundDecimals")]
	public float HeadshotMultiplier;

	/// <summary>
	/// Reference to the GlobalInt that holds the current score.
	/// </summary>
	[BoxGroup("Points Related Values and References")]
	[Tooltip("Reference to the GlobalInt that holds the current score.")]
	public GlobalValue<int> ScoreReference;
	
	/// <summary>
	/// Reference to the GlobalInt that holds the current score.
	/// </summary>
	[BoxGroup("Points Related Values and References")]
	[Tooltip("Reference to the GlobalInt that holds the current score.")]
	public GlobalValue<int> MaxScoreReference;

	/// <summary>
	/// Reference to the GlobalBool telling wether double points should be given.
	/// </summary>
	[BoxGroup("Points Related Values and References")]
	[Tooltip("Reference to the GlobalBool telling wether double points should be given.")]
	public GlobalValue<bool> DoublePointsFlag;

	/// <summary>
	/// The duration the enemy will stay on the same slot in seconds.
	/// </summary>
	[Tooltip("The duration the enemy will stay on the same slot in seconds.")]
	public float StayDuration;

	[Tooltip("The time range that the spawner needs to wait before spawning a new enemy.")]
	[MinMaxSlider(0.6f, 10)]
	public Vector2 CoolDownAfterDeath;

	/// <summary>
	/// All the options for the visuals of this enemy.
	/// <para>
	///		You can use the supplied extension method <see cref="IListExtensions.GetRandomElement{T}(IList{T})"/> to get one of these elements to be used further.
	/// </para>
	/// </summary>
	[Tooltip("All the options for the visuals of this enemy are to be entered here.")]
	public List<GameObject> PrefabOptions;

	[Tooltip("The Decal used on this Enemy when hit.")]
	[Label("Prefab: Hitmarker")]
	public GameObject hitmarkerDecal;

	[HorizontalLine(color: EColor.Blue, order = 0)]

	[BoxGroup("Sound References and Settings")]
	public AudioClip hitmarkerSound;
	[BoxGroup("Sound References and Settings")]
	public AudioClip impactSound;
	[BoxGroup("Sound References and Settings")]
	public float hitmarkerVolume = 1f;

	[HorizontalLine(color: EColor.Blue)]

	[BoxGroup("VFX")]
	[Tooltip("The effect used on this Enemy when its body is hit.")]
	[Label("Hit Body")]
	public VisualEffectConfig hitVFX;

	[BoxGroup("VFX")]
	[Tooltip("The effect used on this Enemy when its head is hit.")]
	[Label("Hit Head")]
	public VisualEffectConfig hitHeadVFX;

	[BoxGroup("VFX")]
	[Tooltip("The effect used on this Enemy when its body is hit during the double points powerup's duration.")]
	[Label("Hit Body - Double Points")]
	public VisualEffectConfig hit2xVFX;

	[BoxGroup("VFX")]
	[Tooltip("The effect used on this Enemy when its head is hit during the double points powerup's duration.")]
	[Label("Hit Head - Double Points")]
	public VisualEffectConfig hitHead2xVFX;

	#region Interface and Proxy Methods

	/// <summary>
	/// Default Implementation for shots. Points are being awarded.
	/// </summary>
	/// <param name="hitLocation"></param>
	/// <returns>The Amount of points awarded.</returns>
	public int Hit(Vector3 hitLocation)
	{
		int tempPoints = PointsWorth;
		if (DoublePointsFlag.Value) tempPoints *= 2;

		ScoreReference.Value += tempPoints;
		MaxScoreReference.Value += tempPoints;

		Debug.Log($"Normal Hit, awarded {tempPoints} points.");

		return tempPoints;
	}

	/// <summary>
	/// Default Implementation for shots. Points are being awarded after being multiplied with <see cref="HeadshotMultiplier"/>
	/// </summary>
	/// <param name="hitLocation"></param>
	/// <returns>The Amount of points awarded.</returns>
	public int HitHead(Vector3 hitLocation)
	{
		int tempPoints = (int)(PointsWorth * HeadshotMultiplier);
		if (DoublePointsFlag.Value) tempPoints *= 2;

		ScoreReference.Value += tempPoints;
		MaxScoreReference.Value += tempPoints;

		Debug.Log($"Headshot! Awarded {tempPoints} points.");

		return tempPoints;
	}
	#endregion

	#region Utility Methods
	/// <summary>
	/// Used by <see cref="HeadshotMultiplier"/>s <see cref="OnValueChangedAttribute"/> to limit the decimal places to 1
	/// </summary>
	private void RoundDecimals()
	{
		HeadshotMultiplier = Mathf.Round(HeadshotMultiplier * 10) / 10;
	}
	#endregion
}
