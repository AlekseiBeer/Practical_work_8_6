using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : Moving
{
    [SerializeField] private Transform _lookAtPointTarget;

    private float _distanceToTargetStart;
    private float _distanceToTargetCurrent;


    void Start()
    {
        _distanceToTargetStart = Vector3.Distance(transform.position, _lookAtPointTarget.position);
        InitializeMovement();
    }

    // Update is called once per frame
    void Update()
    {
        Speed = _distanceToTargetStart * Mathf.Pow(Vector3.Distance(transform.position, _lookAtPointTarget.position) / _distanceToTargetStart, 2);
        UpdateLookAt(_lookAtPointTarget);
        Move();
    }
}