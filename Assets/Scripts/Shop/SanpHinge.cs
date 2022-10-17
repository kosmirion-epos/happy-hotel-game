using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Autohand;

public class SanpHinge : MonoBehaviour
{


    enum Mode { Greater, Smaller }

    Rigidbody rb;
    HingeJoint joint;

    JointLimits baseLimits;
    JointLimits snapLimits = new JointLimits();
    JointLimits currentLimits = new JointLimits();


    [SerializeField] Mode mode;

    [SerializeField] float snapTriggerAngle;

    [SerializeField] Vector3 angle;

    [SerializeField] Vector3 snapAngle;

    [SerializeField] float delay;



    public UnityEvent OnSnap;

    public bool active = true;

    public bool locked;

    Grabbable grabber;
    [SerializeField] PlacePoint placer;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        baseLimits = joint.limits;
        snapLimits.min = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }

        if (!active)
            return;

        if(locked && joint.limits.min != snapLimits.min)
        {
            currentLimits.min = Mathf.Lerp(currentLimits.min, snapLimits.min,Time.deltaTime*0.4f);
            joint.limits = currentLimits;

            return;
        }

        if (!locked && joint.limits.min != baseLimits.min)
            joint.limits = baseLimits;

    }

    public void SnapTo()
    {
        if (delay > 0)
            return;

        delay += 0.9f;

        locked = !locked;

        if (!locked)
        {
            EnableGrabber();
            return;
        }

        DiabaleGrabber();


        /*transform.localEulerAngles = rotation;
        if (joint.limits.min == baseLimits.min)
        {
            joint.limits = snapLimits;
            return;
        }
        joint.limits = baseLimits;
        */
    }

    

    public void DiabaleGrabber()
    {
        if (!grabber)
            grabber = GetComponentInChildren<Grabbable>();

        if (grabber)
            grabber.enabled = false;

        if (placer)
            placer.enabled = false;
    }

    public void EnableGrabber()
    {
        if (grabber)
            grabber.enabled = true;

        if (placer)
            placer.enabled = true;
    }
}
