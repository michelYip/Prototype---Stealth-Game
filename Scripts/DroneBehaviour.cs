using UnityEngine;
using UnityEngine.AI;

public class DroneBehaviour : AbstractEnemyBehaviour
{
    #region Exposed

    [SerializeField] private Transform[] m_patrolPoints;
    [SerializeField] private float m_holdPositionDuration = 2f;
    [SerializeField] private float m_stoppingDistance = 3.5f;

    #endregion

    #region Unity API
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = m_stoppingDistance;
        _holdPositionTimer = Time.time;
        _isHoldingPosition = false;
        _patrolIndex = 0;
        _targetPosition = transform.position;
        _patrolPointPositions = new Vector3[m_patrolPoints.Length];
        for (int i = 0; i < m_patrolPoints.Length; i++)
        {
            _patrolPointPositions[i] = m_patrolPoints[i].position;
        }
        _targetPosition = _patrolPointPositions[0]; 
        _agent.SetDestination(_targetPosition);
    }

    protected override void Update()
    {
        base.Update();
        //_agent.SetDestination(_targetPosition);
    }
    #endregion

    #region Main Methods

    protected override void Chase()
    {
        if (Target != null)
		{
            //GameOverManager.Instance.PlayerFound = CanSeePlayer(_target);
            _playerFound = CanSeePlayer(_target);
            _targetPosition = _target.position;
            _agent.SetDestination(_targetPosition);
            if (!GameOverManager.Instance.PlayerFound)
            {
                _targetPosition = _patrolPointPositions[_patrolIndex];
                _agent.SetDestination(_targetPosition);
            }
        }
    }

    protected override void Patrol()
    {
        _playerFound = false;
        if (Time.time > _holdPositionTimer)
        {
            //if (Vector3.Distance(transform.position, _targetPosition) <= m_stoppingDistance)
            if (_agent.remainingDistance <= m_stoppingDistance)
            {
                _patrolIndex++;
                if (_patrolIndex >= _patrolPointPositions.Length) _patrolIndex = 0;
                _targetPosition = _patrolPointPositions[_patrolIndex];
                _holdPositionTimer = Time.time + m_holdPositionDuration;
                _agent.SetDestination(_targetPosition);
            }
        }
    }

    #endregion

    #region Privates

    private NavMeshAgent _agent;
    private int _patrolIndex;
    private float _holdPositionTimer;
    private bool _isHoldingPosition;

    private Vector3 _targetPosition;
    private Vector3[] _patrolPointPositions;

    #endregion
}
