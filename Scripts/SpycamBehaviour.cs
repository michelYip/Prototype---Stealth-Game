using UnityEngine;

public class SpycamBehaviour : AbstractEnemyBehaviour
{

    #region Exposed

    [Header("Patrol Attributes")]
    [SerializeField] private Transform m_pointA;
    [SerializeField] private Transform m_pointB;
    [SerializeField] private float m_holdPositionDuration = 3f;    
    [SerializeField] private float m_rotationDuration = 5f;

	#endregion

	#region Unity API
	void Awake()
    {
        _holdPositionTimer = Time.time + m_holdPositionDuration;
        _rotationStart = Time.time;
        _startPos = m_pointA.position;
        _endPos = m_pointB.position;
        _lookTarget = _startPos;
    }

	protected override void Update()
	{
        base.Update();
        transform.LookAt(_lookTarget);
	}

	#endregion

	#region Main Methods

	protected override void Chase()
    {
        if (_target != null)
		{
            //GameOverManager.Instance.PlayerFound = CanSeePlayer(_target);
            _playerFound = CanSeePlayer(_target);
            _lookTarget = Vector3.Lerp(_lookTarget, _target.position, Time.deltaTime / 0.1f);
        }
    }

    protected override void Patrol()
    {
        _playerFound = false;
        if (Time.time > _holdPositionTimer)
		{
            if (_rotationStart < _holdPositionTimer)
                _rotationStart = Time.time;

            _lookTarget = Vector3.Lerp(_startPos, _endPos, (Time.time - _rotationStart) / m_rotationDuration);
            
            if (Time.time > _rotationStart + m_rotationDuration)
			{
                Vector3 tmp = _startPos;
                _startPos = _endPos;
                _endPos = tmp;
                _holdPositionTimer = Time.time + m_holdPositionDuration;
			}
		}
    }

    #endregion

    #region Privates

    private float _holdPositionTimer;
    private float _rotationStart;
    private Vector3 _lookTarget;
    private Vector3 _startPos;
    private Vector3 _endPos;

    #endregion
}
