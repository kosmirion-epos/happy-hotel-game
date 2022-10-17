using UnityEngine;

public class RoundEndDisplay : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject roundWon;
    [SerializeField] private GlobalValue<GameState> state;

    public void OnRoundEnd()
    {
        if (state.Value == GameState.RunWillEnd)
            gameOver.SetActive(true);
        else
            roundWon.SetActive(true);
    }
}
