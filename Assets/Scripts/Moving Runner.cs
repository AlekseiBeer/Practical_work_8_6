using UnityEngine;

public class MovingRunner : Moving
{
    public enum RUNNER_STATUS
    {
        STOPPED,
        READY,
        RUNNING,
        PASSES,
        FINISH,
        WINNER
    }

    [Header("Race Settings")]
    [SerializeField] private RaceManager _raceManager;

    [Header("Runner Settings")]
    [SerializeField] private RUNNER_STATUS _runnerStatus;
    public GameObject _rightArm;
    public GameObject _leftArm;

    public RUNNER_STATUS RunnerStatus
    { 
        get => _runnerStatus; 
        set 
        {
            _runnerStatus = value;

            if (value == RUNNER_STATUS.STOPPED || value == RUNNER_STATUS.FINISH) StopMoving();
            if (value == RUNNER_STATUS.RUNNING) StartMoving();

            UpdateRotationArm();
        } 
    }

    void Start()
    {
        _raceManager = FindObjectOfType<RaceManager>();
        if (_raceManager == null)
        {
            Debug.LogError("RaceManager не найден в сцене!");
            enabled = false;
            return;
        }

        InitializeMovement();
    }

    void Update()
    {
        Move();

        if (RunnerStatus != RUNNER_STATUS.WINNER)
        {
            if (RunnerStatus == RUNNER_STATUS.READY)
                transform.rotation = Quaternion.LookRotation(transform.position - _raceManager._currentRunner.transform.position);
            else if (RunnerStatus == RUNNER_STATUS.FINISH || RunnerStatus == RUNNER_STATUS.STOPPED)
                transform.LookAt(_raceManager._currentRunner.transform.position);
            else
                UpdateLookAt(_targetPoint);
        }
        else
        {
            _rotate = true;
            _speedRotate = new Vector3(0, 270, 0);
            UpdateRotationArm();
        }
    }

    protected override void UpdateNextTargetPoint()
    {
        if (Vector3.Distance(transform.position, _targetPoint.position) < 0.01f && RunnerStatus != RUNNER_STATUS.WINNER)
            RunnerStatus = RUNNER_STATUS.FINISH;  
    }

    void UpdateRotationArm()
    {
        switch (RunnerStatus)
        {
            case RUNNER_STATUS.READY:
                _rightArm.transform.localRotation = Quaternion.Euler(-105, 0, 0);
                break;

            case RUNNER_STATUS.PASSES:
                _rightArm.transform.localRotation = Quaternion.Euler(30, 0, 0);
                break;

            case RUNNER_STATUS.WINNER:
                _leftArm.transform.localRotation = Quaternion.Euler(120, 0, 0);
                _rightArm.transform.localRotation = Quaternion.Euler(120, 0, 0);
                break;

            default:
                _rightArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }
}