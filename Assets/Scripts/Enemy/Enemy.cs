using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private float _chaseDist = 6.0f;
    [SerializeField] private float _atkDist = 2.0f;
    [SerializeField] private float _changeNodeDist = 0.5f;

    [Header("<color=red>Behaviours</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;
    private Transform _target, _actualNode;
    private List<Transform> _navMeshNodes = new();
    public List<Transform> NavMeshNodes 
    { 
        get { return _navMeshNodes; } 
        set { _navMeshNodes = value; } 
    }

    private NavMeshAgent _agent;

    private void Awake()
    {
        _actualHP = _maxHP;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        GameManager.Instance.Enemies.Add(this);
    }

    public void Initialize()
    {
        _target = GameManager.Instance.Player.gameObject.transform;

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    private void Update()
    {
        if (!_target)
        {
            Debug.LogError($"<color=red>NullReferenceException</color>: No asignaste un objetivo, boludo.");
            return;
        }

        if(Vector3.SqrMagnitude(transform.position - _target.position) <= (_chaseDist * _chaseDist))
        {
            if(Vector3.SqrMagnitude(transform.position - _target.position) <= (_atkDist * _atkDist))
            {
                if (!_agent.isStopped) _agent.isStopped = true;

                //Debug.Log($"<color=red>{name}</color>: Japish.");
            }
            else
            {
                if (_agent.isStopped) _agent.isStopped = false;

                _agent.SetDestination(_target.position);
            }
        }
        else
        {
            if (_agent.destination != _actualNode.position) _agent.SetDestination(_actualNode.position);

            if(Vector3.SqrMagnitude(transform.position - _actualNode.position) <= (_changeNodeDist * _changeNodeDist))
            {
                _actualNode = GetNewNode(_actualNode);

                _agent.SetDestination(_actualNode.position);
            }
        }
    }

    private Transform GetNewNode(Transform lastNode = null)
    {
        Transform newNode = _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];

        while(lastNode == newNode)
        {
            newNode = _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];
        }

        return newNode;
    }

    public void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            Debug.Log($"<color=red>{name}</color>: oh no *Explota*");

            GameManager.Instance.Enemies.Remove(this);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"<color=red>{name}</color>: Comí <color=black>{dmg}</color> puntos de daño. Me quedan <color=green>{_actualHP}</color> puntos.");
        }
    }
}
