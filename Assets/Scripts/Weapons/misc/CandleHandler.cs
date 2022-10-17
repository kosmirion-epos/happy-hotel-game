using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CandleHandler : MonoBehaviour
{
    [SerializeField] private Ammo ammo;
    [SerializeField] private List<VisualEffect> candles;
    [SerializeField] private VisualEffectConfig snuffVFX;

    private int burningCandles;

    private void Start()
    {
        burningCandles = ammo.MaxBullets;
    }

    public void UpdateCandles()
    {
        int newBurningCandles = ammo.Bullets;
        
        // Turn on candles
        for (var i = burningCandles; i < newBurningCandles; ++i)
        {
            candles[i].SetBool("Alive", true);
            candles[i].Play();
        }

        // Turn off candles
        for (var i = newBurningCandles; i < burningCandles; ++i)
        {
            candles[i].SetBool("Alive", false);
            candles[i].Stop();
            snuffVFX.Spawn(candles[i].transform.position, candles[i].transform.rotation);
        }

        burningCandles = newBurningCandles;
    }
}
 