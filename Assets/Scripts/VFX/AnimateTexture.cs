using UnityEngine;

public class AnimateTexture : MonoBehaviour
{
    [SerializeField] private int materialID;
    [SerializeField] private Vector2 scrollSpeed;
    public Vector2 ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }

    private new Renderer renderer;
    private Vector2 offset;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        offset += Time.deltaTime * ScrollSpeed;
        offset = new Vector2(offset.x % 1.0f, offset.y % 1.0f);
        renderer.materials[materialID].mainTextureOffset = offset;
    }
}
