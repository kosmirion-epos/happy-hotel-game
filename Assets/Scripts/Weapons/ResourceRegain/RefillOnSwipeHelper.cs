using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColliderDirEvent : UnityEvent<ColliderDirs> { }

public enum ColliderDirs
{
	Front, Back, Up, Down
}

public class RefillOnSwipeHelper : MonoBehaviour
{
	[SerializeField][Tag] private string handTag;

	[Dropdown(nameof(Triggers))]
	[SerializeField] private ColliderDirs triggerDir;

	[SerializeField] private ColliderDirEvent collEvent;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(handTag))
		{
			collEvent?.Invoke(triggerDir);
		}
	}

	private DropdownList<ColliderDirs> Triggers()
	{
		return new DropdownList<ColliderDirs>()
		{
			{ "Front Trigger", ColliderDirs.Front },
			{ "Back Trigger", ColliderDirs.Back },
			{ "Up Trigger", ColliderDirs.Up },
			{ "Down Trigger", ColliderDirs.Down }
		};
	}

	[Button]
	private void Test()
	{
		collEvent?.Invoke(triggerDir);
	}
}
