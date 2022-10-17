using UnityEngine;
using UnityEngine.Events;

public class ScoreGoalEventTrigger : MonoBehaviour
{
    [SerializeField] private GlobalValue<int> score;
    [SerializeField] private GlobalValue<int> scoreGoal;
    [SerializeField] private UnityEvent onGoalReached;
    [SerializeField] private UnityEvent onGoalDropped;

    private bool reached;

    private void OnEnable()
    {
        Reset();
    }

    public void CheckEvent()
    {
        if (reached && score.Value < scoreGoal.Value)
        {
            onGoalDropped.Invoke();
            reached = false;
        }
        else if (!reached && score.Value >= scoreGoal.Value)
        {
            onGoalReached.Invoke();
            reached = true;
        }
    }

    public void Reset()
    {
        reached = false;
    }
}
