using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    #region Exposed

    [SerializeField] GameObject m_tutorialText;

	#endregion

	#region Unity API
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			m_tutorialText.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			m_tutorialText.SetActive(false);
		}
	}

	#endregion

	#region Main Methods
	#endregion

	#region Privates
	#endregion
}
