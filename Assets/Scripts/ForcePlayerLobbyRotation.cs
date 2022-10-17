using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ForcePlayerLobbyRotation : MonoBehaviour
{
    [SerializeField] private Transform lookAt;

	[InfoBox("On means plain 180 degree rotation, off means based on transform")]
    [SerializeField] bool modeToggle;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(ForceItHopefully), 0.1f);
    }

    private void ForceItHopefully()
	{
        if (modeToggle) transform.rotation = Quaternion.Euler(0, 180, 0);
        else
		{
            var direction = lookAt.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
		}
	}
}
