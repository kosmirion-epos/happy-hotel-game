using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class OverheatLimiter : ActionLimiter
{
    [SerializeField] private Heat heat;

    protected override bool LimitsAction() => heat.RequiresCooling;
}
