using Autohand;
using UnityEngine;
using UnityEngine.Events;

public class ScrewThread : MonoBehaviour
{
    [SerializeField] private PlacePoint placePoint;
    [SerializeField] private Rigidbody connectedBody;

    [SerializeField] private string screwingLayer = "Grabbable";
    [SerializeField] private float screwingEffectiveness = 1;
    [SerializeField] private Vector3 axis = new(0, 0, 1);
    [SerializeField] private float initialOffset = 1;
    [SerializeField] private float screwingAngularDrag = 5;
    [SerializeField] private float screwingGrabPriority = 1;

    [SerializeField] private UnityEvent OnFinishScrew;

    private int screwingLayerId;

    private Grabbable screw;
    private ConfigurableJoint screwJoint;
    private OneWayJoint screwOneWayJoint;
    private Rigidbody screwBody;

    private float offset;
    private float lastRotation;

    private int overwrittenLayer;
    private float overwrittenAngularDrag;
    private float overwrittenGrabPriority;

    private void OnEnable()
    {
        screwingLayerId = LayerMask.NameToLayer(screwingLayer);
        axis = axis.normalized;
        placePoint.OnPlaceEvent += OnPlace;
    }

    private void OnDisable()
    {
        placePoint.OnPlaceEvent -= OnPlace;
    }

    private void OnPlace(PlacePoint _, Grabbable grabbable)
    {
        screw = grabbable;

        overwrittenGrabPriority = grabbable.grabPriorityWeight;
        grabbable.grabPriorityWeight = screwingGrabPriority;

        placePoint.enabled = false;
        placePoint.Remove(screw);

        overwrittenLayer = screw.gameObject.layer;
        screw.gameObject.layer = screwingLayerId;

        screwJoint = screw.gameObject.AddComponent<ConfigurableJoint>();
        screwJoint.axis = axis;
        screwJoint.secondaryAxis = new Vector3(axis.z, axis.x, -axis.y);
        screwJoint.connectedBody = connectedBody;
        screwJoint.autoConfigureConnectedAnchor = false;
        screwJoint.anchor = Vector3.zero;
        screwJoint.connectedAnchor = connectedBody.transform.InverseTransformPoint(transform.position);
        screwJoint.xMotion = ConfigurableJointMotion.Locked;
        screwJoint.yMotion = ConfigurableJointMotion.Locked;
        screwJoint.zMotion = ConfigurableJointMotion.Locked;
        screwJoint.angularXMotion = ConfigurableJointMotion.Free;
        screwJoint.angularYMotion = ConfigurableJointMotion.Locked;
        screwJoint.angularZMotion = ConfigurableJointMotion.Locked;
        screwJoint.targetPosition = axis * initialOffset;

        screwOneWayJoint = screw.gameObject.AddComponent<OneWayJointGrabbable>();

        screwBody = screw.GetComponent<Rigidbody>();
        overwrittenAngularDrag = screwBody.angularDrag;
        screwBody.angularDrag = screwingAngularDrag;

        offset = initialOffset;
        lastRotation = screwJoint.Angles().x;
    }

    private void Update()
    {
        if (screwJoint == null)
            return;

        var rotation = screwJoint.Angles().x;

        offset = Mathf.Clamp(offset - screwingEffectiveness * ((rotation - lastRotation) % 180f) / 360, 0, initialOffset);
        screwJoint.targetPosition = axis * offset;

        lastRotation = rotation;

        if (offset == 0)
        {
            screw.grabPriorityWeight = overwrittenGrabPriority;
            screwJoint.gameObject.layer = overwrittenLayer;
            Destroy(screwOneWayJoint);
            Destroy(screwJoint);
            screwBody.angularDrag = overwrittenAngularDrag;
            placePoint.OnPlaceEvent -= OnPlace;
            placePoint.Place(screw);
            placePoint.OnPlaceEvent += OnPlace;
            placePoint.enabled = true;
            OnFinishScrew?.Invoke();
            screw = null;
            screwBody = null;
            Destroy(screwJoint);
        }
    }
}
