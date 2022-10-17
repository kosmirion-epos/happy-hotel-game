using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODRandomElevatorRamble : MonoBehaviour
{
    [SerializeField] private GlobalFMODSoundManager sm;
    [SerializeField, Range(30f, 180f)] private float randomTime = 60f;

    private void Start()
    {
        StartCoroutine(RandomSound());
    }

    public void PlaySoundRandom(int id)
    {
        sm.Value.PlaySound(id,  transform.gameObject);
    }

    private IEnumerator RandomSound()
    {
        while (true)
        {   
            yield return new WaitForSeconds(randomTime);
            PlaySoundRandom(14);
            randomTime = Random.Range(30f, 180f);
        }
    }

}
