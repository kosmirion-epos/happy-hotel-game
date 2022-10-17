using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ConeHitScanWeapon : Weapon
{

	[SerializeField][Foldout("Hit Detection")][Tag] protected string enemyBodyTag;
	[SerializeField][Foldout("Hit Detection")][Tag] protected string enemyHeadTag;
	[SerializeField][Foldout("Hit Detection")] private LayerMask raycastMask;

	[SerializeField][Foldout("Physics")] private ScopedValue<float> impactForce;
	[SerializeField][Foldout("Physics")] private ScopedValue<Vector3> recoilForce;
	[SerializeField][Foldout("Physics")][Required] private Transform bulletEject;

	[SerializeField][Foldout("Sound")] private GlobalFMODSoundManager sm;
	[SerializeField][Foldout("Sound")] private int impactSound;

	[SerializeField][Foldout("VFX")] private VisualEffectConfig fireVFX;
	[SerializeField][Foldout("VFX")] private VisualEffectConfig impactVFX;
	[SerializeField][Foldout("VFX")] private ScopedValue<bool> impactVFXRicochet;
	[SerializeField][Foldout("VFX")] private VisualEffectConfig trailVFX;
	[SerializeField][Foldout("VFX")] private Transform vfxOrigin;

	private new Rigidbody rigidbody;

	[SerializeField][Foldout("Cone Settings")][OnValueChanged(nameof(ConeDefsChanged))] private float range;
	[SerializeField][Foldout("Cone Settings")][OnValueChanged(nameof(ConeDefsChanged))] private float endRadius;
	//[SerializeField][Foldout("Cone Settings")][OnValueChanged(nameof(ConeDefsChanged))] private float closeRadius;
	[SerializeField][Foldout("Cone Settings")] private Vector2 coneScale = Vector2.one;
	[SerializeField][Foldout("Cone Settings")][ReadOnly] private float angle;
	[SerializeField][Foldout("Cone Settings")][OnValueChanged(nameof(ShotsChanged))][Range(0, 32)] private int additionalShots;
	[SerializeField][Foldout("Cone Settings")] private bool randomRange;
	[SerializeField][Foldout("Cone Settings")][ShowIf(nameof(randomRange))][MinMaxSlider(0, 1)] private Vector2 addShotRandomRangeBounds;


	[SerializeField][Foldout("Gizmo Settings")] private Mesh coneMesh;
	[SerializeField][Foldout("Gizmo Settings")][Range(4, 360)] private int resolution;
	[SerializeField][Foldout("Gizmo Settings")] private bool drawIndicator;
	[SerializeField][Foldout("Gizmo Settings")] private bool solidIndicator;
	[SerializeField][Foldout("Gizmo Settings")][OnValueChanged(nameof(ShotsChanged))] private bool drawTestShots;


	private List<Vector3> _testShotDirections;

	#region Unity Methods

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		coneMesh = GenerateConeMesh();
	}

	private void OnValidate()
	{
		coneMesh = GenerateConeMesh();
	}

	private void OnDrawGizmos()
	{
		if (drawIndicator)
		{
			Gizmos.color = Color.red;

			if (solidIndicator)
				Gizmos.DrawMesh(coneMesh, bulletEject.position, bulletEject.rotation);
			else
				Gizmos.DrawWireMesh(coneMesh, bulletEject.position, bulletEject.rotation);
		}

		if (_testShotDirections?.Count > 0 && drawTestShots)
		{
			foreach (var dir in _testShotDirections)
			{
				Gizmos.color = Color.gray;
				Gizmos.DrawLine(bulletEject.position, bulletEject.position + (range * addShotRandomRangeBounds.y * dir));
				Gizmos.color = Color.black;
				Gizmos.DrawLine(bulletEject.position, bulletEject.position + (range * addShotRandomRangeBounds.x * dir));
			}
		}
	}

	#endregion

	protected override void Fire()
	{
		Debug.Log("Fire Called");

		if (fireVFX)
			fireVFX.Spawn(vfxOrigin.position, vfxOrigin.rotation);


		List<Vector3> shotDirs = GetShotDirections();

		for (int i = 0; i < shotDirs.Count; i++)
		{
			if (Physics.Raycast(bulletEject.position,
					   shotDirs[i],
					   out RaycastHit hit,
					   i == 0 || !randomRange ? float.PositiveInfinity : range * Random.Range(addShotRandomRangeBounds.x, addShotRandomRangeBounds.y), //first shot (the straight one) has infinite range)
					   raycastMask))
			{
				var hitBody = hit.transform.GetComponent<Rigidbody>();

				if (trailVFX)
					trailVFX.Spawn(
						vfxOrigin.position,
                        Quaternion.LookRotation(shotDirs[i], vfxOrigin.up)
					).SetFloat("ShotDistance", Vector3.Distance(hit.point, vfxOrigin.position));

				GameObject vfxObj = null;

				if (impactVFX)
					vfxObj = impactVFX.Spawn(
						hit.point,
						Quaternion.LookRotation(
							impactVFXRicochet.Value
								? Vector3.Reflect(bulletEject.forward, hit.normal)
								: hit.normal,
							Vector3.up
						)
					).gameObject;

				if (vfxObj && sm)
					sm.Value.PlaySound(impactSound, vfxObj);

				if (hitBody)
					hitBody.AddForceAtPosition(impactForce.Value * (hit.point - bulletEject.position).normalized, hit.point, ForceMode.Impulse);

				if (hit.transform.CompareTag(enemyHeadTag))
					hit.transform.GetComponentInParent<IShootable>()?.HitHead(hit.point);
				else if (hit.transform.CompareTag(enemyBodyTag))
					hit.transform.GetComponentInParent<IShootable>()?.Hit(hit.point);

				Debug.DrawRay(bulletEject.position, hit.point - bulletEject.position, Color.green, 3);
			}
			else
            {
				if (trailVFX)
					trailVFX.Spawn(
						vfxOrigin.position,
						Quaternion.LookRotation(shotDirs[i], vfxOrigin.up)
					).SetFloat("ShotDistance", 100);
				Debug.DrawRay(bulletEject.position, bulletEject.forward * 1000, Color.red, 1);
            }
		}

		rigidbody.AddForce(transform.TransformDirection(recoilForce.Value), ForceMode.Impulse);
	}

	private List<Vector3> GetShotDirections()
	{
		List<Vector3> dirs = new List<Vector3>(1 + additionalShots)
		{
			(bulletEject.rotation * new Vector3(0, 0, range)).normalized
		};

		for (int i = 0; i < additionalShots; i++)
		{
			var x = Random.insideUnitCircle;
			dirs.Add((bulletEject.rotation * new Vector3(x.x * endRadius, x.y * endRadius, range).ScaleXYZ(x: coneScale.x, y: coneScale.y)).normalized);
		}

		return dirs;
	}

	private Mesh GenerateConeMesh()
	{
		Mesh mesh = new Mesh
		{
			name = "Cone Indicator",
			vertices = GetVertices(),
			triangles = GetTriangles(),
			normals = GetNormals()
		};

		mesh.Optimize();

		return mesh;

		Vector3[] GetVertices()
		{
			List<Vector3> verts = new List<Vector3>();

			verts.Add(Vector3.zero); //Urpsrung - 0

			verts.Add(new Vector3(0, 0, range)); //vorne mitte - 1

			for (int i = 0; i < resolution; i++)
			{
				float cornerAngle = 2f * Mathf.PI / (float)resolution * i;

				verts.Add(new Vector3(Mathf.Cos(cornerAngle) * endRadius, Mathf.Sin(cornerAngle) * endRadius, range).ScaleXYZ(x: coneScale.x, y: coneScale.y));
			}

			return verts.ToArray();
		}

		int[] GetTriangles()
		{
			List<int> tris = new List<int>();

			//Outer Faces
			for (int i = 0; i < resolution - 1; i++)
			{
				tris.AddRange(new int[3] { 0, 3 + i, 2 + i });
			}

			tris.AddRange(new int[3] { 0, 2, 2 + (resolution - 1) });

			//Front Faces

			for (int i = 0; i < resolution - 1; i++)
			{
				tris.AddRange(new int[3] { 1, 2 + i, 3 + i });
			}

			tris.AddRange(new int[3] { 1, 2 + (resolution - 1), 2 });

			return tris.ToArray();
		}

		Vector3[] GetNormals()
		{
			List<Vector3> normals = new List<Vector3>();

			normals.Add(new Vector3(0, 0, -range).normalized); //Urpsrung - 0

			normals.Add(new Vector3(0, 0, range).normalized); //vorne mitte - 1

			for (int i = 0; i < resolution; i++)
			{
				float cornerAngle = 2f * Mathf.PI / (float)resolution * i;

				normals.Add((new Vector3(Mathf.Cos(cornerAngle) * endRadius, Mathf.Sin(cornerAngle) * endRadius, range) - new Vector3(0, 0, range)).normalized);
			}

			return normals.ToArray();
		}
	}

	#region Helper Properties

	private void ConeDefsChanged()
	{
		float sideB = Mathf.Sqrt(Mathf.Pow(range, 2f) + Mathf.Pow(endRadius, 2) - (2 * range * endRadius * Mathf.Cos(0.5f * Mathf.PI)));
		float angleA = Mathf.Acos(((Mathf.Pow(range, 2) - Mathf.Pow(sideB, 2) - Mathf.Pow(endRadius, 2)) / (-2f * sideB * endRadius))) * 180 / Mathf.PI;
		angle = 180 - 90 - angleA;

		_testShotDirections = GetShotDirections();
	}

	private void ShotsChanged()
	{
		_testShotDirections = GetShotDirections();
	}

	[Button]
	private void SetTestDirs()
	{
		_testShotDirections = GetShotDirections();
	}

	[Button]
	private void EmptyTestDirs()
	{
		_testShotDirections.Clear();
	}

	[Button]
	private void ResetConeScale()
	{
		coneScale = Vector2.one;
		coneMesh = GenerateConeMesh();
	}

	#endregion
}
