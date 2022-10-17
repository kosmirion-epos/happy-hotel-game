using UnityEngine;
using UnityEngine.UI;

public class EndRunButtonEnabler : ExtendedBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GlobalGameState gameState;
    [SerializeField] private GlobalOpenState doorState;

    private void OnEnable()
    {
        UpdateEnabled();
    }

    public void UpdateEnabled()
    {
        AtEndOfFrame(() => button.interactable = gameState.Value == GameState.PreRound && doorState.Value == OpenState.Closed);
    }
}
