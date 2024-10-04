using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("<color=#6A89A7>Cursor</color>")]
    [SerializeField] private CursorLockMode _lockMode = CursorLockMode.Locked;
    [SerializeField] private bool _isCursorVisible = false;

    [Header("<color=#6A89A7>Physics</color>")]
    [Range(.01f, 1f)][SerializeField] private float _detectionRadius = .1f;
    [SerializeField] private float _hitOffset = 0.25f;

    [Header("<color=#6A89A7>Settings</color>")]
    [Range(1f, 1000f)][SerializeField] private float _mouseSensitivity = 500f;
    [Range(.125f, 1f)][SerializeField] private float _minDistance = .25f;
    [Range(1f, 10f)][SerializeField] private float _maxDistance = 5f;
    [Range(-90f, 0f)][SerializeField] private float _minRotation = -45f;
    [Range(0f, 90f)][SerializeField] private float _maxRotation = 80f;

    private bool _isCamBlocked = false;
    private float _mouseX = 0.0f, _mouseY = 0.0f;
    private Vector3 _dir = new(), _dirTest = new(), _camPos = new();

    private Camera _cam;
    private Transform _target;

    private Ray _camRay;
    private RaycastHit _camRayHit;

    private void Start()
    {
        _target = GameManager.Instance.Player.GetCamTarget;

        _cam = Camera.main;

        Cursor.lockState = _lockMode;
        Cursor.visible = _isCursorVisible;

        transform.forward = _target.forward;

        _mouseX = transform.eulerAngles.y;
        _mouseY = transform.eulerAngles.x;
    }

    private void FixedUpdate()
    {
        _camRay = new Ray(transform.position, _dir);

        _isCamBlocked = Physics.SphereCast(_camRay, _detectionRadius, out _camRayHit, _maxDistance);
    }

    private void LateUpdate()
    {
        UpdateCamRot(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        UpdateSpringArm();
    }

    private void UpdateCamRot(float x, float y)
    {
        transform.position = _target.position;

        if (x == 0.0f && y == 0.0f) return;

        if(x != 0.0f)
        {
            _mouseX += x * _mouseSensitivity * Time.deltaTime;

            if(_mouseX > 360.0f || _mouseX < -360.0f)
            {
                _mouseX -= 360.0f * Mathf.Sign(_mouseX);
            }
        }

        if(y != 0.0f)
        {
            _mouseY += y * _mouseSensitivity * Time.deltaTime;

            _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);
        }

        transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0.0f);
    }

    private void UpdateSpringArm()
    {
        _dir = -transform.forward;

        if (_isCamBlocked)
        {
            _dirTest = (_camRayHit.point - transform.position) + (_camRayHit.normal * _hitOffset);

            if(_dirTest.sqrMagnitude <= _minDistance * _minDistance)
            {
                _camPos = transform.position + _dir * _minDistance;
            }
            else
            {
                _camPos = transform.position + _dirTest;
            }
        }
        else
        {
            _camPos = transform.position + _dir * _maxDistance;
        }

        _cam.transform.position = _camPos;
        _cam.transform.LookAt(transform.position);
    }
}
