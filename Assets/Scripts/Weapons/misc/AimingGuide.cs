using Autohand;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingGuide : MonoBehaviour
{
	[SerializeField][Required] private GlobalValue<bool> guidesVisibleFlag;
	[SerializeField][Required] private GlobalValue<bool> guidesOverride;
    [SerializeField][Required] private Grabbable weaponGrabbable;

    [Header("Pointing Options")]
    public Transform bulletEject;
    public LineRenderer line;
    [Space]
    public float maxRange = 5;
    [Tooltip("Defaults to grabbable on start if none")]
    public LayerMask layers;

	private void Update()
	{
        if (!guidesVisibleFlag.Value || !weaponGrabbable.IsHeld() || guidesOverride.Value)
		{
			line.enabled = false;

			return;
		}

		line.enabled = true;

		if (Physics.Raycast(bulletEject.position, bulletEject.forward, out RaycastHit hit, float.PositiveInfinity, layers))
		{
			line.positionCount = 2;
            line.SetPositions(new Vector3[] { bulletEject.position, hit.point });
        }
		else
		{
			line.positionCount = 2;
			line.SetPositions(new Vector3[] { bulletEject.position, bulletEject.position + bulletEject.forward * maxRange });
		}
	}
}
