using UnityEngine;

public class DischargeProcessor : ActionProcessor
{
    [SerializeField] private Charge charge;

    protected override void HandleAction() => charge.Discharge();
}
