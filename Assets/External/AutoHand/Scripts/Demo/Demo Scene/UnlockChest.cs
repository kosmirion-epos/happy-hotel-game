using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockChest : MonoBehaviour
{
    public HingeJoint joint;

    public void Unlock() {
        joint.limits = new JointLimits {
            bounceMinVelocity = joint.limits.bounceMinVelocity,
            bounciness = joint.limits.bounciness,
            contactDistance = joint.limits.contactDistance,
            min = 0,
            max = 160
        };
        joint.spring = new JointSpring() { spring = 5, targetPosition = 160 };
    }

    public void Lock() {
        joint.limits = new JointLimits {
            bounceMinVelocity = joint.limits.bounceMinVelocity,
            bounciness = joint.limits.bounciness,
            contactDistance = joint.limits.contactDistance,
            min = -2,
            max = 2
        };
    }
}
