using Autohand;
using UnityEngine;

public class PlayerBelt : MonoBehaviour
{
    [SerializeField] private AutoHandPlayer player;
    [SerializeField] private float relativeHeight;

    private Vector3 offset;

    private void Awake()
    {
        offset = transform.localPosition;
    }

    private void Update()
    {
        transform.position = player.transform.position + relativeHeight * (player.headCamera.transform.position - player.transform.position) + offset;
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.headCamera.transform.forward, Vector3.up), Vector3.up);
    }
}
