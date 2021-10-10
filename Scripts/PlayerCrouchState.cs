using UnityEngine;

public class PlayerCrouchState : MonoBehaviour, StateInterface
{
    #region Exposed

    [SerializeField] private float m_crouchSpeedModifier = 0.35f;
    [SerializeField] private Vector3 m_colliderCenterPosition = new Vector3(0, 0.3f, 0);

    #endregion

    #region Unity API
    private void Awake()
    {
        _controller = GetComponent<CharacterMovementController>();
        _camera = GetComponent<PlayerCameraController>();
    }

    #endregion

    #region Main Methods

    public void DoInit()
    {
        _controller.IsSprinting = false;
        _controller.IsCrouching = true;

        _controller.SetBool("IsSprinting", false);
        _controller.SetBool("IsCrouching", true);

        _camera.SwitchCamera("CROUCH");
    }

    public void DoUpdate()
    {
        Vector3 MoveDir = _controller.MovePlayer();
        _controller.AdjustCollider(m_colliderCenterPosition, 0.4f, 0.05f);
        //_controller.LerpTransformLocalPosition(_controller.CameraLookAt, m_lookAtTargetPosition, m_transitionDuration);

        _controller.SetFloat("MoveX", _controller.GetMovementInput().x);
        _controller.SetFloat("MoveZ", _controller.GetMovementInput().z);

        float speedModifier = _controller.GetSlopeSpeedModifier(MoveDir);
        _controller.Velocity = new Vector3(_controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.x * m_crouchSpeedModifier * speedModifier,
                                _controller.Velocity.y,
                                _controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.z * m_crouchSpeedModifier * speedModifier);

        _controller.AdjustHeightToFloor();
    }

    public void DoExit()
    {
        _controller.IsCrouching = false;
    }

    #endregion

    #region Privates

    private CharacterMovementController _controller;
    private PlayerCameraController _camera;


    #endregion
}