using UnityEngine;

[CreateAssetMenu(menuName = "Global Value/Global Int")]
public class GlobalInt : GlobalStructValue<int> 
{ 
	public void Add(int x)
	{
		Value += x;
	}
}