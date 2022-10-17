using Autohand;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectsWhileInTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 force;

    private HashSet<Rigidbody> pushedBodies;

    private void Awake()
    {
        pushedBodies = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        var body = other.attachedRigidbody ? other.attachedRigidbody : other.GetComponent<GrabbableChild>()?.grabParent?.GetComponent<Rigidbody>();

        if (body != null)
            pushedBodies.Add(body);
    }

    private void OnTriggerExit(Collider other)
    {
        var body = other.attachedRigidbody ? other.attachedRigidbody : other.GetComponent<GrabbableChild>()?.grabParent?.GetComponent<Rigidbody>();

        if (body != null)
            pushedBodies.Remove(body);
    }

    private void Update()
    {
        pushedBodies.RemoveWhere(b => b == null);

        foreach (var b in pushedBodies)
            b.AddForce(transform.InverseTransformDirection(force));
    }
}
