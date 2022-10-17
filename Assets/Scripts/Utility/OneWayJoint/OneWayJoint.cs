using System;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[DisallowMultipleComponent]
public class OneWayJoint : MonoBehaviour
{
    protected ConfigurableJoint joint;
    protected PseudoFixedJoint pseudoJoint;
    protected bool autoConfigureConnectedAnchor;
    public event Action OnInitialize;
    public event Action OnDeInitialize;

    public void Initialize()
    {
        if (pseudoJoint != null)
            DeInitialize();

        joint = GetComponent<ConfigurableJoint>();

        if (joint == null)
            return;

        var pseudoJointObject = new GameObject(
            "PseudoFixedJoint (" + joint.gameObject.name + ")",
            typeof(Rigidbody),
            typeof(PseudoFixedJoint)
        );

        //TODO check if joint break bug fixed

        var pseudoJointBody = pseudoJointObject.GetComponent<Rigidbody>();
        pseudoJointBody.isKinematic = true;

        pseudoJoint = pseudoJointObject.GetComponent<PseudoFixedJoint>();
        pseudoJoint.body = pseudoJointBody;
        pseudoJoint.anchor = Vector3.zero;
        pseudoJoint.connectedBody = joint.connectedBody;
        pseudoJoint.connectedAnchor = joint.connectedAnchor;
        pseudoJoint.relativeRotation = Quaternion.Inverse(pseudoJoint.connectedBody.rotation);
        pseudoJoint.originalBody = GetComponent<Rigidbody>();
        pseudoJoint.originalJoint = joint;
        pseudoJoint.originalRelativeRotation = transform.rotation;

        autoConfigureConnectedAnchor = joint.autoConfigureConnectedAnchor;
        joint.connectedBody = pseudoJointBody;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;

        pseudoJoint.UpdateTransform();

        OnInitialize?.Invoke();
    }

    public void DeInitialize()
    {
        if (pseudoJoint == null)
            return;

        if (joint != null)
        {
            joint.connectedBody = pseudoJoint.connectedBody;
            joint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            joint.connectedAnchor = pseudoJoint.connectedAnchor;
        }

        OnDeInitialize?.Invoke();

        Destroy(pseudoJoint.gameObject);
        pseudoJoint = null;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        DeInitialize();
    }

    private void Update()
    {
        if (joint == null && pseudoJoint != null)
        {
            DeInitialize();
            Destroy(this);
        }
    }
}
