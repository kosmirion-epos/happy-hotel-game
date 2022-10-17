using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunAmmoHandler : MonoBehaviour
{
    [SerializeField] private Ammo ammo;
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private int materialID;
    [SerializeField] private List<Material> materials;

    private int lastBullets;

    private void Start()
    {
        UpdateNumbers();
    }

    public void UpdateNumbers()
    {
        var newBullets = ammo.Bullets;

        if (newBullets == lastBullets)
            return;

        foreach (var r in renderers)
        {
            var ms = r.materials;
            ms[materialID] = materials[newBullets];
            r.materials = ms;
        }

        lastBullets = newBullets;
    }
}
