using Autohand;
using NaughtyAttributes;
using System.Linq;
using UnityEngine;

public class AmmoRefillOnPose : MonoBehaviour
{
    [SerializeField] private Ammo ammo;
    [SerializeField] private Transform holdOrigin;
    [SerializeField] private ScopedValue<AutoHandPlayer> player;
    [SerializeField] private bool restrictHandPose;
    [SerializeField][ShowIf(nameof(restrictHandPose))] private GrabbablePose pose;
    [SerializeField] private ScopedValue<float> holdTime;
    [SerializeField] private ScopedValue<float> maxAngle;
    [SerializeField] private ScopedValue<Vector3> direction;
    [SerializeField] private ScopedValue<Vector3> directionFromOffset;
    [SerializeField] private ScopedValue<Vector3> minOffsetFromHead;

    private Grabbable grabbable;
    private float poseEnterTime;
    private float poseExitTime;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (_matchesPose())
        {
            if (poseExitTime >= poseEnterTime)
                poseEnterTime = Time.time;
            else if (Time.time - poseEnterTime > holdTime.Value)
                ammo.Refill();
        }
        else
        {
            if (poseExitTime < poseEnterTime)
                poseExitTime = Time.time;
        }
    }

    private bool _isHeldBySinglePose(GrabbablePose pose)
    {
        var heldBy = grabbable.GetHeldBy();

        if (heldBy.Count != 1)
            return false;

        Hand hand = heldBy.First();
        hand.GetCurrentHeldGrabPose(hand.transform, grabbable, out var grabPose, out var relativeTo);

        return grabPose == pose;
    }

    private bool _matchesPose()
    {
        return (!restrictHandPose || _isHeldBySinglePose(pose)) &&
            Vector3.Dot(holdOrigin.position - player.Value.headCamera.transform.position - minOffsetFromHead.Value, directionFromOffset.Value) > 0.0f &&
            Vector3.Angle(transform.forward, direction.Value) < maxAngle.Value;
    }
}
