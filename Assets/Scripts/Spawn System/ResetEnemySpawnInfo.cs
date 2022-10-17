using UnityEngine;

public class ResetEnemySpawnInfo : ExtendedBehaviour
{
    [SerializeField] private GlobalValue<int> updatedSpawnerNumber;
    [SerializeField] private GlobalValue<bool> hasSpawnOccurred;

    private void LateUpdate()
    {
        updatedSpawnerNumber.Value = 0;
        hasSpawnOccurred.Value = false;
    }
}
