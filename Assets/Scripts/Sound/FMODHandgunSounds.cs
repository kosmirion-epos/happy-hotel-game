using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODHandgunSounds : MonoBehaviour
{
    [SerializeField] private GlobalFMODSoundManager sm;

    [SerializeField] private int ref1;
    [SerializeField] private int ref2;

    public void PlaySound()
    {
        if (Random.Range(0, 2) == 1) sm.Value.PlaySound(ref1);
        else sm.Value.PlaySound(ref2);
    }
}
