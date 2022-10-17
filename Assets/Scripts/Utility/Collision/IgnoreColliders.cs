using System.Collections.Generic;
using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{
    [SerializeField] private List<Collider> collidersToIgnore;

    private void Awake()
    {
        var ownCollider = GetComponent<Collider>();

        foreach (var c in collidersToIgnore)
        {
            Physics.IgnoreCollision(ownCollider, c);
            Physics.IgnoreCollision(c, ownCollider);
        }
    }
}
