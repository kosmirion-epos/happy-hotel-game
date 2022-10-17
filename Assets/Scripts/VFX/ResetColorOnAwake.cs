using UnityEngine;

public class ResetColorOnAwake : MonoBehaviour
{
    [ColorUsage(showAlpha: false, hdr: true)][SerializeField] private Color initialColor;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private int materialID;
    [SerializeField] private string colorName;

    public Material Material => renderer.materials[materialID];

    private void Awake()
    {
        Material.SetColor(colorName, initialColor);
        Destroy(this);
    }
}
