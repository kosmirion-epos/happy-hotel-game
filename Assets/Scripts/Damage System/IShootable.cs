using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// An Interface to force certain Elements to provide functionality when shot.
/// </summary>
public interface IShootable
{
	/// <summary>
	/// The generic response of this object to being shot.
	/// </summary>
	public int Hit(Vector3 hitLocation);
	/// <summary>
	/// The more special response of this object to being shot in the Headshot zone, if it has one.
	/// </summary>
	public int HitHead(Vector3 hitLocation);


	/// <summary>
	/// The more special response of this object to being shot in the Headshot zone, if it has one.
	/// </summary>
	/// 

}
