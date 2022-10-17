using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using NaughtyAttributes;

public class LinearAxiswiseMovement : MovementType
{
	/// <summary>
	/// How far this movement will go to both sides. Effective range is double this value.
	/// </summary>
	[SerializeField]
	[BoxGroup("Tween Settings")]
	[Tooltip("How far this movement will go to both sides. Effective range is double this value.")]
	private float sidewardsDistance;

	/// <summary>
	/// Along which <see cref="Axis"/> the movement shall happen.
	/// </summary>
	[SerializeField]
	[BoxGroup("Tween Settings")]
	[Tooltip("Along which Axis the movement shall happen.")]
	private Axis moveAxis;

	/// <summary>
	/// How long one cycle of the movement takes.
	/// </summary>
	[SerializeField]
	[BoxGroup("Tween Settings")]
	[Tooltip("How long one cycle of the movement takes.")]
	private float moveDuration;

	/// <summary>
	/// The <see cref="Ease"/> to be used.
	/// </summary>
	[SerializeField]
	[BoxGroup("Tween Settings")]
	[Tooltip("The Ease to be used.")]
	[DisableIf(nameof(useGlobalEase))]
	private Ease ease;

	/// <summary>
	/// The <see cref="Ease"/> to be used.
	/// </summary>
	[SerializeField]
	[BoxGroup("Tween Settings")]
	[Tooltip("The Ease to be used.")]
	private bool useGlobalEase;

	/// <summary>
	/// Buffer field to reference the tween.
	/// </summary>
	private Tween tween;

	[SerializeField]
	[BoxGroup("Gizmo Settings")]
	private Color gizmoColor;

	[SerializeField]
	[BoxGroup("Gizmo Settings")]
	private bool drawGizmo;

	[SerializeField]
	[BoxGroup("Gizmo Settings")]
	private bool drawInPlaymode;

	private Vector3 startPosition;

	public bool IsDead { get; set; }

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyMovementSpeedMultiplier.
	/// </summary>
	[BoxGroup("Global Value References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyMovementSpeedMultiplier")]
	[Label("Scalar: Movement Speed")]
	private GlobalValue<float> moveSpeedMultiplier;

	/// <summary>
	/// A reference to the GlobalValue ScriptableObject for the EnemyMovementEase.
	/// </summary>
	[BoxGroup("Global Value References")]
	[SerializeField]
	[Tooltip("Enter a reference to the GlobalValue ScriptableObject for the EnemyMovementEase")]
	[Label("Movement Ease")]
	[EnableIf(nameof(useGlobalEase))]
	private GlobalValue<DG.Tweening.Ease> enemyMoveEase;

	void Start()
	{
		startPosition = transform.localPosition;
		IsDead = false;
	}

	private Vector3[] GetWaypoints()
	{
		Vector3 offset = moveAxis switch
		{
			Axis.X => sidewardsDistance * transform.right,
			Axis.Y => sidewardsDistance * transform.up,
			Axis.Z => sidewardsDistance * transform.forward,
			_ => Vector3.zero
		};

		return new Vector3[4]
		{
			transform.position,
			transform.position - offset,
			transform.position + offset,
			transform.position
		};
	}

	private void OnValidate()
	{
		ReStartTween();
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (!drawGizmo) return;
		if (EditorApplication.isPlaying && !drawInPlaymode) return;

		Gizmos.color = gizmoColor;

		Vector3[] waypoints = GetWaypoints();

		Vector3 offset = Vector3.up;

		Gizmos.DrawLine(waypoints[1] + offset, waypoints[2] + offset);
	}
#endif

	public void MakeTween()
	{
		if (IsDead) return;

		tween = transform.DOPath(GetWaypoints(), moveDuration / moveSpeedMultiplier.Value, PathType.Linear, PathMode.Ignore, gizmoColor: gizmoColor)
			.SetEase(useGlobalEase ? enemyMoveEase.Value : ease)
			.SetLoops(-1, LoopType.Restart);
	}

	[Button]
	private void ReStartTween()
	{
		if (!Application.isPlaying) return;
		if (tween == null) return;
		if (IsDead) return;

		transform.localPosition = startPosition;

		tween.Kill(true);
		tween = null;

		MakeTween();
	}


	public void StopTweens()
	{
		tween.Kill(true);
		tween = null;
	}
}
