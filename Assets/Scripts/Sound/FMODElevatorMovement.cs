using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODElevatorMovement : MonoBehaviour
{
    [SerializeField] private GlobalFMODSoundManager sm;

    public void StartElevatorMovement(GameObject placeIn3D)
    {
        sm.Value.StartElevatorMovement(placeIn3D);
    }

    public void StopElevatorMovement()
    {
        sm.Value.StopElevatorMovement();
    }
    
}
