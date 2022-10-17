using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PseudoFixedJoint : MonoBehaviour
{
    public Rigidbody body;
    public Vector3 anchor;
    public Rigidbody connectedBody;
    public Vector3 connectedAnchor;
    public Quaternion relativeRotation;
    public Rigidbody originalBody;
    public ConfigurableJoint originalJoint;
    public Quaternion originalRelativeRotation;

    //TODO try to fix lag
    private void FixedUpdate()
    {
        //UpdateTransform();
        StartCoroutine(UpdateTransformLater());
    }

    private IEnumerator UpdateTransformLater()
    {
        yield return new WaitForFixedUpdate();

        if (this == null)
            yield return null;

        UpdateTransform();
    }

    public void UpdateTransform()
    {
        Transform connectedTransform = connectedBody.transform;
        transform.rotation = connectedTransform.rotation * relativeRotation;
        transform.position = connectedTransform.TransformPoint(connectedAnchor) - transform.TransformDirection(anchor);
        //TODO fix lag
    }

    private void OnDrawGizmos()
    {
        Vector3 anchorPosition = transform.TransformPoint(anchor);
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawLine(anchorPosition, anchorPosition + transform.right);
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawLine(anchorPosition, anchorPosition + transform.up);
        Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
        Gizmos.DrawLine(anchorPosition, anchorPosition + transform.forward);

        if (connectedBody == null)
            return;

        Vector3 connectedAnchorPosition = connectedBody.transform.TransformPoint(connectedAnchor);
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        Gizmos.DrawLine(connectedAnchorPosition, connectedAnchorPosition + connectedBody.transform.right);
        Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
        Gizmos.DrawLine(connectedAnchorPosition, connectedAnchorPosition + connectedBody.transform.up);
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawLine(connectedAnchorPosition, connectedAnchorPosition + connectedBody.transform.forward);
    }
}
