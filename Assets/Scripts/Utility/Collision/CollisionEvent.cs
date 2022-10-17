using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    public UnityEvent OnCollision;

    private void OnCollisionEnter(Collision other)
    {
        OnCollision.Invoke();
    }

}
