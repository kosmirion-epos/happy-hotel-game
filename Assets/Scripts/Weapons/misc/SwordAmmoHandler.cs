using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAmmoHandler : MonoBehaviour
{
    [SerializeField] private Ammo ammo;
    [SerializeField] private List<Transform> lines;
    [SerializeField] private float animationDuration;

    private int lastBullets;

    private void Start()
    {
        UpdateFillState();
    }

    public void UpdateFillState()
    {
        var newBullets = ammo.Bullets;

        if (lastBullets == newBullets)
            return;

        foreach (var t in lines)
            t.DOScaleZ(newBullets / (float)ammo.MaxBullets, animationDuration).SetEase(Ease.OutCubic);

        lastBullets = newBullets;
    }
}
