using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	#region Exposed
	#endregion

	#region Unity API

	private void Awake()
	{
		Cursor.visible = true;
	}

	#endregion

	#region Main Methods

	public void LoadNewScene(string sceneName)
	{
        SceneManager.LoadScene(sceneName);
	}

    public void QuitGame()
	{
        Application.Quit();
	}

    #endregion

    #region Privates
    #endregion
}
