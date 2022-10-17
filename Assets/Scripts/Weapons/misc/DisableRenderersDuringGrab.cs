using Autohand;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class DisableRenderersDuringGrab : MonoBehaviour
{
    [SerializeField][Tag] private string grabbableTag;
    [SerializeField] private List<Renderer> renderers;

    public void OnRelease(Hand _, Grabbable grabbable)
    {
        if (grabbable.CompareTag(grabbableTag))
            foreach (var r in renderers)
                r.enabled = true;
    }

    public void OnGrab(Hand _, Grabbable grabbable)
    {
        if (grabbable.CompareTag(grabbableTag))
            foreach (var r in renderers)
                r.enabled = false;
    }
}
