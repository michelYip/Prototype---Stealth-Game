using UnityEngine;

public class PlayerJumpState : MonoBehaviour, StateInterface
{
    #region Exposed
    #endregion

    #region Unity API
    void Awake()
    {
        _controller = GetComponent<CharacterMovementController>();
        _camera = GetComponent<PlayerCameraController>();
        _sprint = GetComponent<PlayerSprintState>();
        _audio = GetComponent<PlayerAudioManager>();
    }

    #endregion

    #region Main Methods

    public void DoInit()
    {
        _audio.PlaySFX("jumpStart");
        if (DoJump)
        {
            _controller.SetTrigger("JumpTrigger");
            if(!_controller.IsSprinting)
                _currentVelocityY = _controller.JumpHeightLocomotion * _controller.JumpHeightLocomotion;
            else
                _currentVelocityY = _controller.JumpHeightLocomotion * _controller.JumpHeightSprint;
        }
		else
        {
            _controller.SetBool("IsFalling", true);
            _currentVelocityY = 0f;
        }

        _camera.SwitchCamera("LOCOMOTION");
    }

    public void DoUpdate()
    {
        Vector3 MoveDir = _controller.MovePlayer();

        _controller.SetFloat("MoveX", _controller.GetMovementInput().x);
        _controller.SetFloat("MoveY", _currentVelocityY);
        _controller.SetFloat("MoveZ", _controller.GetMovementInput().z);
        //ClampMagnitude
        _controller.SetFloat("LocomotionMagnitude", Vector3.ClampMagnitude(new Vector3(_controller.GetMovementInput().x, 0f, _controller.GetMovementInput().z),1f).magnitude);

        if (!_controller.IsSprinting)
            _controller.Velocity = new Vector3(_controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.x,
                                _currentVelocityY,
                                _controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.z);
        else
            _controller.Velocity = new Vector3(_controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.x * _sprint.SprintSpeedModifier,
                                _currentVelocityY,
                                _controller.MoveSpeed * _controller.GetMovementInput().magnitude * MoveDir.z * _sprint.SprintSpeedModifier);

        if (_currentVelocityY >= _controller.JumpHeightLocomotion * Physics.gravity.y)
            _currentVelocityY += _controller.GravityForce * Physics.gravity.y * Time.deltaTime;

        if (_currentVelocityY < 0)
		{
            _controller.SetBool("IsFalling", true);
        }
    }

    public void DoExit()
    {
        _audio.PlaySFX("jumpLand");
    }

    public bool DoJump { get => _doJump; set => _doJump = value; }

    #endregion

    #region Privates

    private CharacterMovementController _controller;
    private PlayerCameraController _camera;
    private PlayerAudioManager _audio;

    private PlayerSprintState _sprint;
    private bool _doJump;
    private float _currentVelocityY;

	#endregion
}
