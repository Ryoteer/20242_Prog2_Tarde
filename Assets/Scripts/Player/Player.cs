using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=#6A89A7>Animation</color>")]
    [SerializeField] private string _atkName = "onAttack";
    [SerializeField] private string _castName = "onCast";
    [SerializeField] private string _isMovName = "isMoving";
    [SerializeField] private string _isGroundName = "isGrounded";
    [SerializeField] private string _jumpName = "onJump";
    [SerializeField] private string _multiAtkName = "onMultiAttack";
    [SerializeField] private string _xName = "xAxis";
    [SerializeField] private string _zName = "zAxis";

    [Header("<color=#6A89A7>Behaviours</color>")]
    [SerializeField] private int _atkDmg = 20;

    [Header("<color=#6A89A7>Camera</color>")]
    [SerializeField] private Transform _camTarget;

    public Transform GetCamTarget { get { return _camTarget; } }

    [Header("<color=#6A89A7>Inputs</color>")]
    [SerializeField] private KeyCode _atkKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _multiAtkKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _castKey = KeyCode.Mouse2;
    [SerializeField] private KeyCode _intKey = KeyCode.F;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _quitKey = KeyCode.P;

    [Header("<color=#6A89A7>Physics</color>")]
    [SerializeField] private Transform _atkOrigin;
    [SerializeField] private float _atkRayDist = 1.0f;
    [SerializeField] private float _multiAtkRayDist = 5.0f;
    [SerializeField] private LayerMask _atkMask;
    [SerializeField] private float _castRadius = 2.5f;
    [SerializeField] private Transform _intOrigin;
    [SerializeField] private float _intRayDist = 1.0f;
    [SerializeField] private LayerMask _intMask;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _jumpRayDist = 0.75f;
    [SerializeField] private LayerMask _jumpMask;
    [SerializeField] private float _movRayDist = 0.75f;
    [SerializeField] private LayerMask _movMask;
    [SerializeField] private float _movSpeed = 3.5f;

    [Header("<color=#6A89A7>VFX</color>")]
    [SerializeField] private ParticleSystem _psystem;

    private Vector3 _camForwardFix = new(), _camRightFix = new(), _dir = new(), _jumpOffset = new(), _movRayDir = new();
    private Vector3 _dirFix = new();

    private Animator _anim;
    private Rigidbody _rb;
    private Transform _camTransform;

    private Ray _atkRay, _intRay, _jumpRay, _movRay, _multiAtkRay;
    private RaycastHit _atkHit, _intHit;

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
        _camTransform = Camera.main.transform;

        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        if(!IsBlocked(_dir.x, _dir.z))
        {
            _anim.SetFloat(_xName, _dir.x);
            _anim.SetFloat(_zName, _dir.z);
        }
        else
        {
            _anim.SetFloat(_xName, 0.0f);
            _anim.SetFloat(_zName, 0.0f);
        }
        
        _anim.SetBool(_isGroundName, IsGrounded());

        _anim.SetBool(_isMovName, _dir.x != 0 || _dir.z != 0);

        if (Input.GetKeyDown(_atkKey))
        {
            _anim.SetTrigger(_atkName);
        }
        else if (Input.GetKeyDown(_multiAtkKey))
        {
            _anim.SetTrigger(_multiAtkName);
        }
        else if (Input.GetKeyDown(_castKey))
        {
            _anim.SetTrigger(_castName);
        }

        if (Input.GetKeyDown(_intKey))
        {
            Interact();
        }

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _anim.SetTrigger(_jumpName);
            Jump();
        }

        if (Input.GetKeyDown(_quitKey))
        {
            SceneLoadManager.Instance.LoadSecenAsync("MainMenu");
        }
    }

    private void FixedUpdate()
    {
        if((_dir.x != 0.0f || _dir.z != 0.0f) && !IsBlocked(_dir.x, _dir.z))
        {
            Movement(_dir);
        }
    }

    public void Attack()
    {
        _atkRay = new Ray(_atkOrigin.position, transform.forward);

        if (Physics.Raycast(_atkRay, out _atkHit, _atkRayDist, _atkMask))
        {
            if (_atkHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_atkDmg);
            }
        }
    }

    public void Cast()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _castRadius);
        _psystem.Play();

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_atkDmg * 3);
            }
        }
    }

    private void Interact()
    {
        _intRay = new Ray(_intOrigin.position, transform.forward);

        if(Physics.SphereCast(_intRay, .25f, out _intHit, _intRayDist, _intMask))
        {
            if(_intHit.collider.TryGetComponent<ButtonBehaviour>(out ButtonBehaviour intObj))
            {
                intObj.OnPress(name);
            }
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(Vector3 dir)
    {
        _camForwardFix = _camTransform.forward;
        _camRightFix = _camTransform.right;

        _camForwardFix.y = 0.0f;
        _camRightFix.y = 0.0f;

        Rotate(_camForwardFix);

        _dirFix = (_camRightFix * dir.x + _camForwardFix * dir.z).normalized;

        _rb.MovePosition(transform.position + _dirFix * _movSpeed * Time.fixedDeltaTime);
    }

    public void MultiAttack()
    {
        _multiAtkRay = new Ray(_intOrigin.position, transform.forward);

        RaycastHit[] hitObjs = Physics.SphereCastAll(_multiAtkRay, .25f, _multiAtkRayDist, _atkMask);

        foreach (RaycastHit obj in hitObjs)
        {
            if (obj.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_atkDmg * 2);
            }
        }
    }   
    
    private void Rotate(Vector3 dir)
    {
        transform.forward = dir;
    }

    private bool IsGrounded()
    {
        _jumpOffset = new Vector3(transform.position.x, transform.position.y + 0.125f, transform.position.z);

        _jumpRay = new Ray(_jumpOffset, -transform.up);

        return Physics.Raycast(_jumpRay, _jumpRayDist, _jumpMask);
    }

    private bool IsBlocked(float x, float z)
    {
        _movRayDir = (transform.right * x + transform.forward * z);

        _movRay = new Ray(transform.position, _movRayDir);

        return Physics.Raycast(_movRay, _movRayDist, _movMask);
    }    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_jumpRay);
    }
}
