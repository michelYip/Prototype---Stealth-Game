using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKey : MonoBehaviour
{
    #region Exposed

    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Light _light;
    [SerializeField] private LevelProgressScriptable _currentProgress;

	#endregion

	#region Unity API

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_currentProgress.NumberOfKey++;
			_light.enabled = false;
			_particle.Pause();
			Destroy(this);
		}
	}

	#endregion

	#region Main Methods
	#endregion

	#region Privates
	#endregion
}
