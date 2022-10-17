using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODShopStaticNoise : MonoBehaviour
{
    [SerializeField] private GlobalFMODSoundManager sm;

    public void StartShopStaticNoise(GameObject placeIn3D)
    {
        sm.Value.StartShopStaticNoise(placeIn3D);
    }

    public void StopShopStaticNoise()
    {
        sm.Value.StopShopStaticNoise();
    }
}
