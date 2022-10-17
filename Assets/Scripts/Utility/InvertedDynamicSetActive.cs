using UnityEngine;

public class InvertedDynamicSetActive : MonoBehaviour
{
	public void InverseSetActive(bool active)
	{
		gameObject.SetActive(!active);
	}
}
