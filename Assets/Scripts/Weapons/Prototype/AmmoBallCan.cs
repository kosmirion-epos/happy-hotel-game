using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBallCan : ExtendedBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballEject;
    [SerializeField] private ScopedValue<float> spawnRate;
    [SerializeField] private ScopedValue<Vector3> upDirection;
    [SerializeField] private ScopedValue<Vector3> ejectDirection;
    [SerializeField] private ScopedValue<float> maxEjectAngle;

    private bool ejecting;

    private void Update()
    {
        if (!ejecting && Vector3.Angle(transform.InverseTransformDirection(upDirection.Value), ejectDirection.Value) <= maxEjectAngle.Value)
        {
            ejecting = true;
            Loop();
        }
    }

    private void Loop()
    {
        WithDelay(
            1 / spawnRate.Value,
            () => {
                if (Vector3.Angle(transform.InverseTransformDirection(upDirection.Value), ejectDirection.Value) > maxEjectAngle.Value)
                {
                    ejecting = false;
                    return;
                }

                SpawnBall();
                Loop();
            }
        );
    }

    private void SpawnBall() => Instantiate(ballPrefab, ballEject.position, ballEject.rotation);
}
