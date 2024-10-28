using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private MovingRunner[] _runners;
    [SerializeField] private GameObject _sphere;
    [SerializeField] private float _passReadyDistance = 10f;
    [SerializeField] private float _passDistance = 1.5f;

    [HideInInspector] public MovingRunner _currentRunner;
    [HideInInspector] public MovingRunner _targetRunner;
    [HideInInspector] public bool Win = false;

    private int _currentRunnerIndex = 0;
    private int _targetRunnerIndex = 1;

    void Start()
    {
        if (_runners == null || _runners.Length < 2)
        {
            Debug.LogError("Ошибка: необходимо минимум два бегуна для проведения гонки!");
            enabled = false;
            return;
        }

        InitializeRunners();
    }

    void Update()
    {
        if (_currentRunnerIndex >= _runners.Length - 1)
        {
            CheckWinCondition();
            return;
        }
            
        float distanceToTargetPoint = Vector3.Distance(_runners[_currentRunnerIndex].transform.position, _runners[_targetRunnerIndex].transform.position);

        if (distanceToTargetPoint <= _passDistance)
            PassSphereToNextRunner();
        else if (distanceToTargetPoint <= _passReadyDistance)
            SetRunnerStatus(_currentRunner, MovingRunner.RUNNER_STATUS.PASSES);
    }

    void InitializeRunners()
    {
        _currentRunner = _runners[_currentRunnerIndex];
        _targetRunner = _runners[_targetRunnerIndex];

        UpdateStatus();
    }

    private void CheckWinCondition()
    {
        foreach (var runner in _runners)
            if (runner.RunnerStatus != MovingRunner.RUNNER_STATUS.FINISH)
                return;

            WinGame();
    }

    private void PassSphereToNextRunner()
    {
        _sphere.transform.parent = _targetRunner._rightArm.transform;
        SetRunnerStatus(_currentRunner, MovingRunner.RUNNER_STATUS.FINISH);

        _currentRunner = _runners[++_currentRunnerIndex];
        if (_targetRunnerIndex < _runners.Length-1) 
            _targetRunner = _runners[++_targetRunnerIndex];

        UpdateStatus();
    }

    private void SetRunnerStatus(MovingRunner runner, MovingRunner.RUNNER_STATUS status) => runner.RunnerStatus = status;

    void UpdateStatus()
    {
        SetRunnerStatus(_currentRunner, MovingRunner.RUNNER_STATUS.RUNNING);
        if (_targetRunnerIndex < _runners.Length - 1)
            SetRunnerStatus(_targetRunner, MovingRunner.RUNNER_STATUS.READY);
    }

    void WinGame()
    {
        foreach (var runner in _runners)
            SetRunnerStatus(runner, MovingRunner.RUNNER_STATUS.WINNER);
        Debug.Log("WIN!!!!");
    }
}