using UnityEngine;

public class ChargeStartProcessor : ActionProcessor
{
    [SerializeField] private Charge charge;

    protected override void HandleAction() => charge.StartCharge();
}
