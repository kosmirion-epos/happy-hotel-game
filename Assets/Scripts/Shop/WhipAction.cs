using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhipAction : MonoBehaviour
{
    enum Mode { Velocity , AngularVelocity, Both}

    [SerializeField] Mode mode;

    Rigidbody rb;

    Vector3 localVelocity;
    Vector3 localangularvelocity;

    [SerializeField, Tooltip("The Driection a whip can be performed. Range 0 to 1")] Vector3 direction;
    public Vector3 whipForce { get; private set; }

    [SerializeField, Tooltip("Velocity required to trigger a whip")] float triggerVelocity;

    public UnityEvent OnWhip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get local Velocity
        localVelocity = transform.InverseTransformDirection(rb.velocity);

        localangularvelocity = transform.InverseTransformDirection(rb.angularVelocity);



        //Velocity Mode
        //
        //Local Velocity Scaled by direction
        whipForce = Vector3.Scale(localVelocity, direction);

        //Angular Velocity
        if (mode == Mode.AngularVelocity)
            whipForce = Vector3.Scale(localangularvelocity, direction);

        //Angular Velocity
        if (mode == Mode.Both)
            whipForce = Vector3.Scale((localangularvelocity + localVelocity) / 2, direction);

        //Return if velocity is not great enough
        if (whipForce.magnitude < triggerVelocity)
            return;

        //Trigger Event
        OnWhip.Invoke();
    }
}
