using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODSoundCaller : MonoBehaviour
{
    [SerializeField] private GlobalFMODSoundManager sm;

    public void PlaySound(int id)
    {
        sm.Value.PlaySound(id);
    }

}
