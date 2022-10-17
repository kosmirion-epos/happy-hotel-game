using NaughtyAttributes;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    public Rigidbody body;

    public Transform barrelTip;
    public float hitPower = 1;
    public float recoilPower = 1;
    public float range = 100;
    public LayerMask layer;

    public GameObject impactFX;

    public AudioClip shootSound;
    public float shootVolume = 1f;

    [SerializeField][Tag] private string bodyTag;
    [SerializeField][Tag] private string headTag;

    private void Start()
    {
        if (body == null && GetComponent<Rigidbody>() != null)
            body = GetComponent<Rigidbody>();
    }

    public void Shoot()
    {
        //Play the audio sound
        if (shootSound)
            AudioSource.PlayClipAtPoint(shootSound, transform.position, shootVolume);

		if (Physics.Raycast(barrelTip.position, barrelTip.forward, out RaycastHit hit, range, (LayerMask)layer))
		{
			var hitBody = hit.transform.GetComponent<Rigidbody>();


			Instantiate(impactFX, hit.point, Quaternion.LookRotation(Vector3.Reflect(barrelTip.forward, hit.normal), Vector3.up));

			//OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
			if (hitBody != null)
			{
				Debug.DrawRay(barrelTip.position, (hit.point - barrelTip.position), Color.green, 5);

				hitBody.AddForceAtPosition(10 * hitPower * (hit.point - barrelTip.position).normalized, hit.point, ForceMode.Impulse);

            }

			if (hit.transform.CompareTag(headTag))
			{
				hit.transform.GetComponentInParent<IShootable>()?.HitHead(hit.point);
			}
			else if (hit.transform.CompareTag(bodyTag))
			{
                Debug.Log("Tried to hit the body.");
                hit.transform.GetComponentInParent<IShootable>()?.Hit(hit.point);
			}
		}
		else
			Debug.DrawRay(barrelTip.position, barrelTip.forward * range, Color.red, 1);

		//body.AddForce(5 * recoilPower * barrelTip.transform.up, ForceMode.Impulse);
    }
}