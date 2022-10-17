using UnityEngine;

[CreateAssetMenu(menuName = "Global Value/Global Float")]
public class GlobalFloat : GlobalStructValue<float> 
{ 
	public void Set(int x)
	{
		Value = x;
	}
}