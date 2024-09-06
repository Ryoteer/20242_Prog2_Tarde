using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=#6A89A7>Animation</color>")]
    [SerializeField] private string _atkName = "onAttack";
    [SerializeField] private string _isMovName = "isMoving";
    [SerializeField] private string _xName = "xAxis";
    [SerializeField] private string _zName = "zAxis";

    [Header("<color=#6A89A7>Inputs</color>")]
    [SerializeField] private KeyCode _atkKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("<color=#6A89A7>Physics</color>")]
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _movSpeed = 3.5f;

    private bool _isOnAir = false;
    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new();

    private Animator _anim;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        //_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //_rb.angularDrag = 1f;

        GameManager.Instance.Player = this;
    }

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        _anim.SetFloat(_xName, _xAxis);
        _anim.SetFloat(_zName, _zAxis);

        _anim.SetBool(_isMovName, _xAxis != 0 || _zAxis != 0);

        if (Input.GetKeyDown(_atkKey))
        {
            _anim.SetTrigger(_atkName);
        }

        if (Input.GetKeyDown(_jumpKey) && !_isOnAir)
        {
            _isOnAir = true;
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0.0f || _zAxis != 0.0f)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(float x, float z)
    {
        _dir = (transform.right * x + transform.forward * z).normalized;

        //_rb.velocity
        //_rb.AddForce(transform.position + _dir * _movSpeed, ForceMode.Force);

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    public void Attack()
    {
        Debug.Log($"<color=#6A89A7>{name}</color>: Japish.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 30) _isOnAir = false;
    }
}
