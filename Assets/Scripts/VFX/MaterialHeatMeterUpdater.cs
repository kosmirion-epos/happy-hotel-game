using UnityEngine;

public class MaterialHeatMeterUpdater : MonoBehaviour
{
    [SerializeField] private Heat heat;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private int materialID;

    private bool requiresCooling;
    private float heatPercentage;
    private float overheatPercentage;

    private void Update()
    {
        if (heatPercentage != heat.Percentage)
        {
            renderer.materials[materialID].SetFloat("_FillPercentage", heat.Percentage);
            heatPercentage = heat.Percentage;
        }

        if (overheatPercentage != heat.ThresholdPercentage)
        {
            renderer.materials[materialID].SetFloat("_OverheatPercentage", heat.ThresholdPercentage);
            overheatPercentage = heat.ThresholdPercentage;
        }

        if (requiresCooling != heat.RequiresCooling)
        {
            renderer.materials[materialID].SetInt("_RequiresCooling", heat.RequiresCooling.ToInt());
            requiresCooling = heat.RequiresCooling;
        }
    }
}
