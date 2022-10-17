using UnityEngine;

public static class VectorExtensions
{
	/// <summary>
	/// Scale the X component of the supplied <see cref="Vector3"/> <paramref name="v"/> by <paramref name="scale"/>
	/// </summary>
	/// <param name="v">The <see cref="Vector3"/> this method is called upon.</param>
	/// <param name="scale">The factor the X component is to be scaled by.</param>
	/// <returns>A scaled version of the vector.</returns>
	public static Vector3 ScaleX(this Vector3 v, float scale)
	{
		return new Vector3(scale * v.x, v.y, v.z);
	}

	/// <summary>
	/// Scale the Y component of the supplied <see cref="Vector3"/> <paramref name="v"/> by <paramref name="scale"/>
	/// </summary>
	/// <param name="v">The <see cref="Vector3"/> this method is called upon.</param>
	/// <param name="scale">The factor the X component is to be scaled by.</param>
	/// <returns>A scaled version of the vector.</returns>
	public static Vector3 ScaleY(this Vector3 v, float scale)
	{
		return new Vector3(v.x, v.y * scale, v.z);
	}

	/// <summary>
	/// Scale the Z component of the supplied <see cref="Vector3"/> <paramref name="v"/> by <paramref name="scale"/>
	/// </summary>
	/// <param name="v">The <see cref="Vector3"/> this method is called upon.</param>
	/// <param name="scale">The factor the X component is to be scaled by.</param>
	/// <returns>A scaled version of the vector.</returns>
	public static Vector3 ScaleZ(this Vector3 v, float scale)
	{
		return new Vector3(v.x, v.y, scale * v.z);
	}

	/// <summary>
	/// More generic scale method for <see cref="Vector3"/>s.
	/// <br>Usage: someVector.ScaleXYZ(x: 2, z: 0.8f)</br>
	/// <br>This will scale the X component by 2, leave the Y as is and scale Z by 0.8</br>
	/// </summary>
	/// <param name="v">The vector to scale.</param>
	/// <param name="x">The scalar for the X component.</param>
	/// <param name="y">The scalar for the Y component.</param>
	/// <param name="z">The scalar for the Z component.</param>
	/// <returns>The scaled <see cref="Vector3"/></returns>
	public static Vector3 ScaleXYZ(this Vector3 v, float x = 1, float y = 1, float z = 1)
	{
		return new Vector3(v.x * x, v.y * y, v.z * z);
	}

	/// <summary>
	/// More generic scale method for <see cref="Vector2"/>s.
	/// <br>Usage: someVector.ScaleXY(x: 2)</br>
	/// <br>This will scale the X component by 2 and leave the Y as is.</br>
	/// </summary>
	/// <param name="v">The vector to scale.</param>
	/// <param name="x">The scalar for the X component.</param>
	/// <param name="y">The scalar for the Y component.</param>
	/// <returns>The scaled <see cref="Vector3"/></returns>
	public static Vector2 ScaleXY(this Vector2 v, float x = 1, float y = 1)
	{
		return new Vector2(v.x * x, v.y * y);
	}
}
