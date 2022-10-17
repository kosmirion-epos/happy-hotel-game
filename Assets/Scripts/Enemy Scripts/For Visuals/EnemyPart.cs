using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [Range(0,1f)] public float initalForce = 1f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            return;


        Vector3 force = transform.right * Random.Range(1f * initalForce, 10f * initalForce) + transform.up * Random.Range(-5f * initalForce, 5f * initalForce) + transform.forward * Random.Range(-5f * initalForce, 5f * initalForce);
        rb.AddForce(force, ForceMode.Impulse);
    }
}
