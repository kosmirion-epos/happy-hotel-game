using NaughtyAttributes;
using UnityEngine;

public class AmmoRefillOnBelt : MonoBehaviour
{
    [SerializeField] private Ammo ammo;
    [SerializeField][Tag] private string playerBeltTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other?.CompareTag(playerBeltTag) == true)
            ammo.Refill();
    }
}
