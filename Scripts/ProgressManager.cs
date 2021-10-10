using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    #region Exposed

    [SerializeField] private LevelProgressScriptable _currentProgress;

    #endregion

    #region Unity API
    void Awake()
    {
        _currentProgress.NumberOfKey = 0;
    }
    #endregion

    #region Main Methods
    #endregion

    #region Privates
    #endregion
}
