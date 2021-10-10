using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkBehaviour : MonoBehaviour
{
    #region Exposed

    [SerializeField] private Transform m_lookAtTarget;

    #endregion

    #region Unity API
    void Awake()
    {
        _controller = GetComponent<CharacterMovementController>();
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        
    }

	private void OnAnimatorIK(int layerIndex)
	{
		if (_animator)
		{
            _animator.SetLookAtWeight(1);
            _animator.SetLookAtPosition(m_lookAtTarget.position);
		}
	}

    #endregion

    #region Main Methods
    #endregion

    #region Privates

    private CharacterMovementController _controller;
	private Animator _animator;
    private PlayerStateMachine _stateMachine;

    #endregion
}
