using UnityEngine;

public class EditorEventDestroyable : MonoBehaviour
{
	/// <summary>
	/// For UnityEvent use
	/// </summary>
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
