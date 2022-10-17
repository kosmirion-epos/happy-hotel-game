using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSwitch : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private Axis axis;
    [SerializeField] private float target;
    [SerializeField] private float margin;

    private void OnEnable()
    {
        switch (axis)
        {
            case Axis.X:
                joint.targetRotation = Quaternion.Euler(
                    target,
                    joint.targetRotation.eulerAngles.y,
                    joint.targetRotation.eulerAngles.z
                );
                break;
            case Axis.Y:
                joint.targetRotation = Quaternion.Euler(
                    joint.targetRotation.eulerAngles.x,
                    target,
                    joint.targetRotation.eulerAngles.z
                );
                break;
            case Axis.Z:
                joint.targetRotation = Quaternion.Euler(
                    joint.targetRotation.eulerAngles.x,
                    joint.targetRotation.eulerAngles.y,
                    target
                );
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (axis)
        {
            case Axis.X:
                if (Mathf.Sign(target) != Mathf.Sign(joint.Angles().x) && Mathf.Abs(joint.Angles().x) > margin)
                    joint.targetRotation = Quaternion.Euler(
                        target = -target,
                        joint.targetRotation.eulerAngles.y,
                        joint.targetRotation.eulerAngles.z
                    );
                break;
            case Axis.Y:
                if (Mathf.Sign(target) != Mathf.Sign(joint.Angles().y) && Mathf.Abs(joint.Angles().y) > margin)
                    joint.targetRotation = Quaternion.Euler(
                        joint.targetRotation.eulerAngles.x,
                        target = -target,
                        joint.targetRotation.eulerAngles.z
                    );
                break;
            case Axis.Z:
                if (Mathf.Sign(target) != Mathf.Sign(joint.Angles().z) && Mathf.Abs(joint.Angles().z) > margin)
                    joint.targetRotation = Quaternion.Euler(
                        joint.targetRotation.eulerAngles.x,
                        joint.targetRotation.eulerAngles.y,
                        target = -target
                    );
                break;
        }
    }
}
