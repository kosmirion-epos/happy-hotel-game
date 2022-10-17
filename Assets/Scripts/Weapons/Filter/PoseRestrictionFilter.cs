using Autohand;
using System.Collections.Generic;
using UnityEngine;

public class PoseRestrictionFilter : GrabbableEventFilter
{
    [SerializeField] private List<GrabbablePose> poses;

    public override bool RejectsSqueeze(Hand hand, Grabbable grabbable)
    {
        hand.GetCurrentHeldGrabPose(hand.transform, grabbable, out var grabPose, out var relativeTo);

        return !poses.Contains(grabPose);
    }
}
