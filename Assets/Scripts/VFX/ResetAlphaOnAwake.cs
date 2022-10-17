using UnityEngine;

public class ResetAlphaOnAwake : MonoBehaviour
{
    [SerializeField] private float initialAlpha;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private int materialID;
    [SerializeField] private string colorName;

    public Material Material => renderer.materials[materialID];

    private void Awake()
    {
        var c = Material.GetColor(colorName);
        c.a = initialAlpha;
        Material.SetColor(colorName, c);
        Destroy(this);
    }
}
