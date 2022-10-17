using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolByShaking : MonoBehaviour
{
    [SerializeField] private Heat heat;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private ScopedValue<float> coolingRate;
    [SerializeField] private ScopedValue<float> overheatingCoolingRate;
    [SerializeField] private ScopedValue<float> velocityAveragingDuration;

    private class TimestampedVelocity
    {
        public float time;
        public Vector3 velocity;
    }

    private Queue<TimestampedVelocity> velocities;
    private Vector3 averageVelocity;
    private Vector3 lastVelocity;

    private void Awake()
    {
        if (!rigidbody)
            rigidbody = GetComponent<Rigidbody>();

        velocities = new();
        velocities.Enqueue(new TimestampedVelocity() { time = Time.fixedTime, velocity = rigidbody.velocity });
        averageVelocity = rigidbody.velocity;
    }

    private void FixedUpdate()
    {
        StartCoroutine(_coolingStep());
    }

    private IEnumerator _coolingStep()
    {
        yield return new WaitForFixedUpdate();

        velocities.Enqueue(new TimestampedVelocity() { time = Time.fixedTime, velocity = rigidbody.velocity });
        averageVelocity = averageVelocity / (velocities.Count - 1) * velocities.Count;
        averageVelocity += rigidbody.velocity / velocities.Count;

        while (Time.fixedTime - velocities.Peek().time > velocityAveragingDuration.Value)
        {
            averageVelocity -= velocities.Dequeue().velocity / velocities.Count;
            averageVelocity = averageVelocity / (velocities.Count + 1) * velocities.Count;
        }

        heat.Cool(
            averageVelocity.magnitude
            * Vector3.Distance(rigidbody.velocity, lastVelocity)
            * (heat.RequiresCooling ? overheatingCoolingRate.Value : coolingRate.Value)
            * Time.fixedDeltaTime
        );
        lastVelocity = rigidbody.velocity;
    }
}
