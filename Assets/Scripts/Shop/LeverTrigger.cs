using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class LeverTrigger : MonoBehaviour
{
    [SerializeField] bool manualBaseRotation;
    [SerializeField][ShowIf(nameof(manualBaseRotation))] float baseRotation;
    [SerializeField] float triggerThreshold;
    [SerializeField] float untriggerThreshold;
    [ShowNonSerializedField] bool triggered;
    public UnityEvent OnPull;
    public UnityEvent OnReturn;
    // Start is called before the first frame update
    void Start()
    {
        if (!manualBaseRotation)
            baseRotation = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.DeltaAngle(baseRotation, transform.localEulerAngles.z);


        if (triggered && angle < untriggerThreshold)
        {
            triggered = false;
            OnReturn.Invoke();
        }



        if (!triggered && angle > triggerThreshold)
        {
            triggered = true;
            OnPull.Invoke();
        }
    }
}
