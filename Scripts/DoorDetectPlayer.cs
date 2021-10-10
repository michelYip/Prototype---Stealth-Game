using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetectPlayer : MonoBehaviour
{
	#region Exposed

	[SerializeField] private int m_requiredNumberOfKeys = 0;
	[SerializeField] private LevelProgressScriptable _currentProgress;

	#endregion

	#region Unity API

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_currentProgress.NumberOfKey >= m_requiredNumberOfKeys)
		{
			if (other.CompareTag("Player"))
			{
				_animator.SetBool("character_nearby", true);
			}
		}
		
	}

	private void OnTriggerExit(Collider other)
	{
		if (_currentProgress.NumberOfKey >= m_requiredNumberOfKeys)
		{
			if (other.CompareTag("Player"))
			{
				_animator.SetBool("character_nearby", false);
			}
		}
	}

	#endregion

	#region Main Methods
	#endregion

	#region Privates
	private Animator _animator;

    #endregion
}
