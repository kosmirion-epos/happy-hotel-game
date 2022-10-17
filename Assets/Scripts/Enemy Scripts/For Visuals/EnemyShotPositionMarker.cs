using UnityEngine;

public class EnemyShotPositionMarker : MonoBehaviour
{
	[SerializeField] private bool shouldDraw = true;

	private void OnDrawGizmos()
	{
		if (!shouldDraw) return;

		Gizmos.color = Color.green;

		Gizmos.DrawSphere(transform.position, 0.1f);
	}
}
