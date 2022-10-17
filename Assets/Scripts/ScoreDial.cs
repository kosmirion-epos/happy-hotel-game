using UnityEngine;
using UnityEngine.Events;

public class ScoreDial : MonoBehaviour
{
    [SerializeField] private GlobalInt score;
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private int digit;
    [SerializeField] private UnityEvent onValueChanged;

    private int _lastValue;
    private int _currentValue => score.Value / Mathf.FloorToInt(Mathf.Pow(10, digit)) % 10;

    private void Awake()
    {
        _lastValue = _currentValue;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(_currentValue * 36, 0, 0),
            speed * Time.deltaTime
        );

        if (_lastValue != _currentValue)
            onValueChanged.Invoke();

        _lastValue = _currentValue;
    }
}
