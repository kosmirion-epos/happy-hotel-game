using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInterfaceController : MonoBehaviour
{

    [SerializeField] new Renderer renderer;
    [SerializeField] int materialID;
    [SerializeField] GlobalValue<float> remainingTime;
    [SerializeField] GlobalValue<float> totalTime;
    [SerializeField] float progress;




    private void Update()
    {
        if (remainingTime.Value <= 0)
            return;

        progress = remainingTime.Value / totalTime.Value;


        renderer.materials[materialID].SetFloat("_ProgressValue", progress);
    }


    public void ResetProgress()
    {
        progress = 0;
        renderer.materials[materialID].SetFloat("_ProgressValue", progress);

    }

}
