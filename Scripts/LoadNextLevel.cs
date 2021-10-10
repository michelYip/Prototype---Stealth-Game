using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    #region Exposed

    [SerializeField] private string m_nextLevelName;
	#endregion

	#region Unity API

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SceneManager.LoadScene(m_nextLevelName);
		}
	}

	#endregion

	#region Main Methods
	#endregion

	#region Privates
	#endregion
}
