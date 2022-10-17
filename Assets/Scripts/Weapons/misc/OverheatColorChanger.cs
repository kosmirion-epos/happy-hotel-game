using UnityEngine;

public class OverheatColorChanger : MonoBehaviour
{
    [SerializeField] private int materialID;
    [ColorUsage(showAlpha: true, hdr: true)][SerializeField] private Color overheatColor;
    [SerializeField] private Heat heat;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private string colorName;

    private Color baseColor;

    private void Awake()
    {
        heat.OnCooledEvent += ResetColor;
        baseColor = renderer.materials[materialID].GetColor(colorName);
    }

    private void Update()
    {
        if (heat.RequiresCooling)
            renderer.materials[materialID].SetColor(colorName, Color.Lerp(baseColor, overheatColor, heat.ThresholdPercentage));
    }

    private void ResetColor() => renderer.materials[materialID].SetColor(colorName, baseColor);
}
