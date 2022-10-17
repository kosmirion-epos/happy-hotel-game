using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ShiftVisibilityDependingOnPlayerDistance : MonoBehaviour
{
	[SerializeField][NaughtyAttributes.Tag] private string playerTag;
	[SerializeField] private CanvasGroup canvasToShiftAlpha;
	[SerializeField] private AnimationCurve distanceAlphaMap;
	[SerializeField] private AnimationCurve distanceInteracableMap;

	[SerializeField] private GlobalValue<Transform> playerPos;

	[SerializeField] private CapsuleCollider col;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, col.radius);
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag(playerTag)) return;

		float distance = Vector2.Distance(
			new Vector2(playerPos.Value.position.x, playerPos.Value.position.z), 
			new Vector2(canvasToShiftAlpha.transform.position.x, canvasToShiftAlpha.transform.position.z));

		float distancePerc = distance / col.radius;


		canvasToShiftAlpha.alpha = distanceAlphaMap.Evaluate(distancePerc);
		canvasToShiftAlpha.interactable = distanceInteracableMap.Evaluate(distancePerc).ToBool();
	}
}
