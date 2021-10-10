using UnityEngine;

public class PlayerSprintState : MonoBehaviour, StateInterface
{
    #region Exposed

    [SerializeField] private float m_sprintSpeedModifier = 1.5f;
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
        _controller.IsCrouching = false;
        _controller.IsSprinting = true;

        _controller.SetBool("IsCrouching", false);
        _controller.SetBool("IsSprinting", true);

        _camera.SwitchCamera("SPRINT");
    }

    public void DoUpdate()
    {
        Vector3 MoveDir = _controller.MovePlayer();
        _controller.AdjustCollider(m_colliderCenterPosition, 1.4f, 0.05f);

        _controller.SetFloat("MoveX", _controller.GetMovementInput().x);
        _controller.SetFloat("MoveZ", _controller.GetMovementInput().z); 
        _controller.SetFloat("LocomotionMagnitude", Vector3.ClampMagnitude(new Vector3(_controller.GetMovementInput().x, 0f, _controller.GetMovementInput().z), 1f).magnitude);

        float speedModifier =  _controller.GetSlopeSpeedModifier(MoveDir);
        _controller.Velocity = new Vector3(_controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.x * m_sprintSpeedModifier * speedModifier,
                                _controller.Velocity.y,
                                _controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.z * m_sprintSpeedModifier * speedModifier);
        _controller.AdjustHeightToFloor();
    }

    public void DoExit()
    {
    }
    public float SprintSpeedModifier { get => m_sprintSpeedModifier; set => m_sprintSpeedModifier = value; }

    #endregion

    #region Privates

    private CharacterMovementController _controller;
    private PlayerCameraController _camera;

	#endregion
}