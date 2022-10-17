using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefillOnSwipe : MonoBehaviour
{
	[SerializeField] private Ammo ammo;
	[Tooltip("The time allowed between triggering the first and second collider in the swipe motion.")]
	[SerializeField] private ScopedValue<float> graceTime;

	private Coroutine coroutine;

	private bool horizontalSwipe;
	private bool verticalSwipe;

	[SerializeField][BoxGroup("Collider Refs")] private BoxCollider frontColl;
	[SerializeField][BoxGroup("Collider Refs")] private BoxCollider backColl;
	[SerializeField][BoxGroup("Collider Refs")] private BoxCollider upColl;
	[SerializeField][BoxGroup("Collider Refs")] private BoxCollider downColl;

	public void CollEventMethod(ColliderDirs dir)
	{
		switch (dir)
		{
			case ColliderDirs.Front:

				if (horizontalSwipe)
				{
					ammo.Refill();
					return;
				}
				else
				{
					horizontalSwipe = true;

					frontColl.enabled = false;
					upColl.enabled = false;
					downColl.enabled = false;
				}

				break;

			case ColliderDirs.Back:

				if (horizontalSwipe)
				{
					ammo.Refill();
					return;
				}
				else
				{
					horizontalSwipe = true;

					backColl.enabled = false;
					upColl.enabled = false;
					downColl.enabled = false;
				}

				break;
			case ColliderDirs.Up:

				if (verticalSwipe)
				{
					ammo.Refill();
					return;
				}
				else
				{
					verticalSwipe = true;

					frontColl.enabled = false;
					backColl.enabled = false;
					upColl.enabled = false;
				}

				break;
			case ColliderDirs.Down:

				if (verticalSwipe)
				{
					ammo.Refill();
					return;
				}
				else
				{
					verticalSwipe = true;

					frontColl.enabled = false;
					backColl.enabled = false;
					downColl.enabled = false;
				}

				break;
			default:
				break;
		}


		if (coroutine != null)
			StopCoroutine(coroutine);

		coroutine = StartCoroutine(ResetBoolsAndColls());
	}

	private IEnumerator ResetBoolsAndColls()
	{
		yield return new WaitForSeconds(graceTime.Value);

		horizontalSwipe = false;
		verticalSwipe = false;

		frontColl.enabled = true;
		backColl.enabled  = true;
		upColl.enabled = true;
		downColl.enabled = true;
	}
}
