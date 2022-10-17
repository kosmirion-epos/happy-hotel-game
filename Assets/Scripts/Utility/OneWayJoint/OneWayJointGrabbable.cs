using Autohand;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class OneWayJointGrabbable : OneWayJoint
{
    private Grabbable grabbable;

    protected void Awake()
    {
        OnInitialize += Init;
        OnDeInitialize += DeInit;
    }

    private void Init()
    {
        grabbable = GetComponent<Grabbable>();
        grabbable.AddJointedBody(pseudoJoint.body);
    }

    private void DeInit()
    {
        grabbable.RemoveJointedBody(pseudoJoint.body);
        grabbable = null;
    }
}
