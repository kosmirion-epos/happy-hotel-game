using NaughtyAttributes;
using UnityEngine;

public class AmmoFunnel : MonoBehaviour
{
    [SerializeField][Tag] private FillWeapon weapon;
    [SerializeField][Tag] private string ballTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ballTag) && weapon.AddBulletIfNotFull())
            Destroy(other.gameObject);
    }
}
