using UnityEngine;

[ExecuteInEditMode]
public class EditorUpdateLookAtPoint2D : MonoBehaviour
{
    [SerializeField] private Vector3 point;

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying && transform.hasChanged && transform.position != point)
        {
            var direction = point - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
#endif
}
