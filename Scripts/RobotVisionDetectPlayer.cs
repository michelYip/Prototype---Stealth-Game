using UnityEngine;

public class RobotVisionDetectPlayer : MonoBehaviour
{
	#region Exposed
	#endregion

	#region Unity API
	void Awake()
	{
		_controller = GetComponentInParent<AbstractEnemyBehaviour>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			bool canSeePlayer = _controller.CanSeePlayer(other.transform);
			if (canSeePlayer)
			{
				//_controller.PlayerFound = true;
				if (!GameOverManager.Instance.PlayerFound)
					GameOverManager.Instance.GameOverStartTime = Time.time;

				GameOverManager.Instance.PlayerFound = true;
				_controller.Target = other.transform;
			}
		}
	}

	#endregion

	#region Main Methods
	#endregion

	#region Privates

	private AbstractEnemyBehaviour _controller;

    #endregion
}
