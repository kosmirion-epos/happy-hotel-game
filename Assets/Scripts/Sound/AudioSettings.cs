using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.Bus master;
    FMOD.Studio.Bus sfx;
    FMOD.Studio.Bus music;
    float volumeMaster = 0.8f;
    float volumeMusic = 0.5f;
    float volumeSFX = 0.8f;

    void Awake()
    {
        music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    void Update()
    {
        master.setVolume(volumeMaster);
        music.setVolume(volumeMusic);
        sfx.setVolume(volumeSFX);
    }

    public void MasterVolumeLevel(float newVolume)
    {
        volumeMaster = newVolume;
    }

    public void MusicVolumeLevel(float newVolume)
    {
        volumeMusic = newVolume;
    }

    public void SFXVolumeLevel(float newVolume)
    {
        volumeSFX = newVolume;
    }
}