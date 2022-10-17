using DG.Tweening;
using UnityEngine;

/// <summary>
/// Destroys the Shield after <see cref="DestroyDelay"/> is over.
/// </summary>
public class ShieldDespawn : ExtendedBehaviour
{
	/// <summary>
	/// The delay after which the Object get's destroyed.
	/// </summary>
	[HideInInspector] public float DestroyDelay;

	[SerializeField] private float spawnAnimationDuration;
	[SerializeField] private float despawnAnimationDuration;
	[SerializeField] private new Renderer renderer;
	[SerializeField] private int dissolveMaterialID;

	private float spawnTime;
	private float lastDissolveState;
	private float age => Time.time - spawnTime;

	private void Start()
	{
		WithDelay(DestroyDelay, () => { Destroy(gameObject); });
		spawnTime = Time.time;
		lastDissolveState = 0;
	}

    private void Update()
    {
		float dissolvePercentage = Mathf.Max(
			age.Remap(0, spawnAnimationDuration, 1, 0, true),
			age.Remap(DestroyDelay - despawnAnimationDuration, DestroyDelay, 0, 1, true)
		);

		if (dissolvePercentage != lastDissolveState)
			renderer.materials[dissolveMaterialID].SetFloat("_Dissolve", dissolvePercentage);

		lastDissolveState = dissolvePercentage;
    }

    /// <summary>
    /// FOR EVENT USE ONLY! Destroys this scripts gameobject.
    /// </summary>
    public void Destroy()
	{
#if UNITY_EDITOR
		DestroyImmediate(gameObject);
#else
		Destroy(gameObject);
#endif
	}
}
