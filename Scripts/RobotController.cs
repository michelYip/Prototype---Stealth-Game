using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    #region Exposed

    [SerializeField] private Transform[] _patrolTransform;
    [SerializeField] private float m_holdPositionDuration = 3f;
    [SerializeField] private bool m_isLoop;

    #endregion

    #region Unity API
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_patrolTransform.Length != 0)
        {
            _patrolPositions = new Vector3[_patrolTransform.Length];
            for (int i = 0; i < _patrolTransform.Length; i++)
            {
                _patrolPositions[i] = _patrolTransform[i].position;
            }
            _currentDestination = _patrolPositions[0];
            _currentPatrolPositionIndex = 0;
        }
        else
            _currentDestination = transform.position;

        _agent.SetDestination(_currentDestination);
    }

    void Update()
    {
        if (!_foundPlayer)
		{
            if (!_isHoldingPosition && Vector3.Distance(transform.position, _currentDestination) < 2f)
            {
                _holdPositionTimer = Time.time + m_holdPositionDuration;
                _isHoldingPosition = true;
            }
            if (_isHoldingPosition && Time.time > _holdPositionTimer)
            {
                if (!m_isLoop)
                {
                    if (!_isReturning)
                    {
                        if (_currentPatrolPositionIndex == _patrolPositions.Length - 1)
                        {
                            _currentPatrolPositionIndex--;
                            _isReturning = true;
                        }
                        else
                            _currentPatrolPositionIndex++;
                    }
                    else
                    {
                        if (_currentPatrolPositionIndex == 0)
                        {
                            _currentPatrolPositionIndex++;
                            _isReturning = false;
                        }
                        else
                            _currentPatrolPositionIndex--;
                    }
                }
                else
                {
                    _currentPatrolPositionIndex = (_currentPatrolPositionIndex == _patrolPositions.Length - 1) ? 0 : _currentPatrolPositionIndex + 1;
                }

                _isHoldingPosition = false;
                _currentDestination = _patrolPositions[_currentPatrolPositionIndex];
            }
        }
        _agent.SetDestination(_currentDestination);
    }
    #endregion

    #region Main Methods

    public Vector3 CurrentDestination { get => _currentDestination; set => _currentDestination = value; }
	public bool FoundPlayer { get => _foundPlayer; set => _foundPlayer = value; }
	public int CurrentPatrolPositionIndex { get => _currentPatrolPositionIndex; set => _currentPatrolPositionIndex = value; }
	public Vector3[] PatrolPositions { get => _patrolPositions; set => _patrolPositions = value; }

	#endregion

	#region Privates

	private NavMeshAgent _agent;
    private Vector3 _currentDestination;
    private Vector3[] _patrolPositions;
    private bool _isHoldingPosition = false;
    private bool _isReturning = false;
    private bool _foundPlayer = false;
    private int _currentPatrolPositionIndex;
    private float _holdPositionTimer;

	#endregion
}
