using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderDispalySingle : MonoBehaviour
{

    [SerializeField]
    float speed = 0.8f;

    [SerializeField]
    Transform drum;

    public int value { get; private set; }

    public void Set(int _value)
    {
        value = _value;
    }


    private void Update()
    {
        if (drum.transform.localRotation == Quaternion.Euler(value * 36, 0, 0)) return;

        TurnToTarget();
    }


    void TurnToTarget()
    {
        drum.transform.localRotation = Quaternion.Lerp(drum.transform.localRotation, Quaternion.Euler(value * 36, 0, 0), speed * Time.deltaTime);
    }

}
