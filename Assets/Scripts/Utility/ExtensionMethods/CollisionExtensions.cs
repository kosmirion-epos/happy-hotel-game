using System.Collections.Generic;
using UnityEngine;

public static class CollisionExtensions
{
    public static Vector3 GetAverageContactPosition(this Collision collision)
    {
        Vector3 hitLocation = Vector3.zero;
        List<ContactPoint> contacts = new();

        collision.GetContacts(contacts);

        foreach (var c in contacts)
            hitLocation += c.point;

        return hitLocation / contacts.Count;
    }
}
