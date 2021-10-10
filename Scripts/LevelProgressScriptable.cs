using UnityEngine;

[CreateAssetMenu]
public class LevelProgressScriptable : ScriptableObject
{
    #region Exposed

    [SerializeField] private int m_numberOfKey;

	public int NumberOfKey { get => m_numberOfKey; set => m_numberOfKey = value; }

	#endregion

	#region Unity API
	#endregion

	#region Main Methods
	#endregion

	#region Privates
	#endregion
}
