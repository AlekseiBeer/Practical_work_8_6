using System.Linq;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform[] _points;

    [Header("Movement Settings")]
    [SerializeField] private bool _isMoving = false;
    public float Speed = 15f;

    [Header("Rotation Settings")]
    public bool _rotate;
    public Vector3 _speedRotate;

    [HideInInspector] protected Transform _targetPoint;
    protected int _targetIndex = 1;
    protected bool _isMovingForward = true;

    void Start()
    {
        InitializeMovement();
    }

    void Update()
    {
        Move();
    }

    protected virtual void InitializeMovement()
    {
        if (_points == null || _points.Length < 2)
        {
            Debug.LogError("Ошибка: массив _points должен содержать хотя бы две точки!");
            enabled = false;
            return;
        }

        _targetPoint = _points[_targetIndex];
        UpdateLookAt(_targetPoint);
    }

    protected virtual void Move()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, Time.deltaTime * Speed);
            UpdateNextTargetPoint();
        }
        if (_rotate)
        {
            transform.Rotate(_speedRotate * Time.deltaTime);
        }  
    }

    protected virtual void UpdateNextTargetPoint()
    {
        if (Vector3.Distance(transform.position, _targetPoint.position) < 0.01f)
        {
            if (_targetPoint == _points.Last() || _targetPoint == _points.First())
                _isMovingForward = !_isMovingForward;

            _targetPoint = _points[_isMovingForward ? Mathf.Min(++_targetIndex, _points.Length - 1) : Mathf.Max(--_targetIndex, 0)];
            UpdateLookAt(_targetPoint);
        }
    }

    protected virtual void UpdateLookAt(Transform targetPoint)
    {
        transform.LookAt(targetPoint);
    }

    public void StartMoving() => _isMoving = true;
    public void StopMoving() => _isMoving = false;

    public float GetDistanceToTargetPoint() => Vector3.Distance(transform.position, _targetPoint.position);
}
