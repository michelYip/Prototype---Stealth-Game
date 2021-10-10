using UnityEngine;

public abstract class AbstractEnemyBehaviour : MonoBehaviour
{
	#region Exposed

	[SerializeField] private Light m_detectionLight;
	[SerializeField] protected Transform m_origin;
	[SerializeField] protected LayerMask m_environmentLayerMask;

	#endregion

	#region Unity API

	protected virtual void Update()
    {
        if (GameOverManager.Instance.PlayerFound && _target != null && _playerFound)
		{
			m_detectionLight.color = Color.red;
			Chase();
		}
		else
		{
			_target = null;
			m_detectionLight.color = Color.yellow;
			Patrol();
		}
    }

	#endregion

	#region Main Methods

	protected abstract void Patrol();

    protected abstract void Chase();

	public bool CanSeePlayer(Transform target)
	{
		float heightOffset = 0.2f;
		int numberOfRays = 5;
		bool result = false;

		for (int i = 0; i < numberOfRays; i++)
		{
			Ray ray = new Ray(Origin.position, (target.position + Vector3.up * i * heightOffset) - Origin.position);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, EnvironmentLayerMask))
			{
				if (hit.collider.CompareTag("Player"))
				{
					result = true;
					_playerFound = true;
					return result;
				}
			}
		}
		_playerFound = result;
		return result;
	}

    //public bool PlayerFound { get => _playerFound; set => _playerFound = value; }
	public Transform Target { get => _target; set => _target = value; }
	public Transform Origin { get => m_origin; set => m_origin = value; }
	public LayerMask EnvironmentLayerMask { get => m_environmentLayerMask; set => m_environmentLayerMask = value; }
	public bool PlayerFound { get => _playerFound; set => _playerFound = value; }

	#endregion

	#region Privates

	//protected bool _playerFound = false;
	protected Transform _target;
	protected bool _playerFound;

	#endregion
}
