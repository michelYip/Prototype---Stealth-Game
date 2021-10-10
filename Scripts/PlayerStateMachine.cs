using UnityEngine;

public class PlayerStateMachine : AbstractStateMachine
{ 
	#region Exposed
	#endregion

	#region Unity API
	protected override void Awake()
    {
		base.Awake();
		_controller = GetComponent<CharacterMovementController>();
		_locomotion = GetComponent<PlayerLocomotionState>();
		_jump = GetComponent<PlayerJumpState>();
		_crouch = GetComponent<PlayerCrouchState>();
		_sprint = GetComponent<PlayerSprintState>();
		_slide = GetComponent<PlayerSlideState>();
		_wallRun = GetComponent<PlayerWallrunState>();
    }

	private void OnGUI()
	{
		GUILayout.Button(_currentState.ToString());
	}

	#endregion

	#region Main Methods

	protected override void OnStateEnter()
	{
		switch (_currentState)
		{
			case State.LOCOMOTION:
				_locomotion.DoInit();
				break;
			case State.CROUCH:
				_crouch.DoInit();
				break;
			case State.JUMP:
				_jump.DoInit();
				break;
			case State.SPRINT:
				_sprint.DoInit();
				break;
			case State.SLIDE:
				_slide.DoInit();
				break;
			case State.WALLRUN:
				_wallRun.DoInit();
				break;
			default:
				break;
		}
	}

	protected override void OnStateExit()
	{
		switch (_currentState)
		{
			case State.LOCOMOTION:
				_locomotion.DoExit();
				break;
			case State.CROUCH:
				_crouch.DoExit();
				break;
			case State.JUMP:
				_jump.DoExit();
				break;
			case State.SPRINT:
				_sprint.DoExit();
				break;
			case State.SLIDE:
				_slide.DoExit();
				break;
			case State.WALLRUN:
				_wallRun.DoExit();
				break;
			default:
				break;
		}
	}

	protected override void OnStateUpdate()
	{
		Vector3 worldDir = new Vector3(_controller.Velocity.x, 0f, _controller.Velocity.z);
		worldDir = Vector3.ClampMagnitude(worldDir, 1f);
		switch (_currentState)
		{
			case State.LOCOMOTION:
				_locomotion.DoUpdate();
				_controller.Velocity = new Vector3(_controller.Velocity.x, 0, _controller.Velocity.z);
				if (Input.GetButtonDown("Jump"))
				{
					_jump.DoJump = true;
					TransitionToState(State.JUMP);
				}
				else if (!_controller.CheckIsGrounded())
				{
					_jump.DoJump = false;
					TransitionToState(State.JUMP);
				}
				if (Input.GetKeyDown(KeyCode.C))
					TransitionToState(State.CROUCH);
				if (Input.GetKeyDown(KeyCode.LeftShift) && Vector3.Dot(transform.forward, worldDir) > 0f) 
				{
					TransitionToState(State.SPRINT);
				}
				break;

			case State.CROUCH:
				_crouch.DoUpdate();
				if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Jump")) && _controller.CanStand())
					TransitionToState(State.LOCOMOTION);
				else if (Input.GetKeyDown(KeyCode.LeftShift) && _controller.CanStand())
					TransitionToState(State.SPRINT);
				else if (!_controller.CheckIsGrounded())
				{
					_jump.DoJump = false;
					TransitionToState(State.JUMP);
				}
				break;

			case State.JUMP:
				_jump.DoUpdate();
				if (_controller.CheckIsGrounded() && _controller.Velocity.y <= 0f)
				{
					_controller.SetTrigger("GroundedTrigger");
					_controller.SetBool("IsFalling", false);
					if (!_controller.IsSprinting)
						TransitionToState(State.LOCOMOTION);
					if (_controller.IsSprinting)
						TransitionToState(State.SPRINT);
				}
				else if (!_controller.CheckIsGrounded() && 
					_controller.IsSprinting &&
					((_controller.FindLeftWall() != Vector3.zero) || (_controller.FindRightWall() != Vector3.zero)) &&
					Input.GetButtonDown("Jump"))
				{
					TransitionToState(State.WALLRUN);
				}
				break;

			case State.SPRINT:
				_sprint.DoUpdate();
				_controller.Velocity = new Vector3(_controller.Velocity.x, 0, _controller.Velocity.z);
				if (Input.GetKeyDown(KeyCode.LeftShift) ||
					worldDir.magnitude <= 0.95f ||
					Vector3.Dot(worldDir, transform.forward) < 0f)
					TransitionToState(State.LOCOMOTION);
				else if (Input.GetButtonDown("Jump"))
				{
					_jump.DoJump = true;
					TransitionToState(State.JUMP);
				}
				else if (!_controller.CheckIsGrounded())
				{
					_jump.DoJump = false;
					TransitionToState(State.JUMP);
				}
				else if (Input.GetKeyDown(KeyCode.C))
				{
					TransitionToState(State.SLIDE);
				}

				break;

			case State.SLIDE:
				_slide.DoUpdate();
				if (!_controller.CheckIsGrounded())
				{
					_jump.DoJump = false;
					TransitionToState(State.JUMP);
				}
				else if (_slide.SlideEnded)
				{
					float locomotionMagnitude = Vector3.ClampMagnitude(new Vector3(_controller.GetMovementInput().x, 0f, _controller.GetMovementInput().z), 1f).magnitude;
					if (!_controller.CanStand())
						TransitionToState(State.CROUCH);
					else
					{
						if (locomotionMagnitude < 0.5f)
							TransitionToState(State.CROUCH);
						else
							TransitionToState(State.SPRINT);
					}
					
				}

				break;

			case State.WALLRUN:
				_wallRun.DoUpdate();
				if (Input.GetButtonDown("Jump"))
				{
					_jump.DoJump = true;
					TransitionToState(State.JUMP);
				}
				else if (!_controller.IsWallRunning)
				{
					_jump.DoJump = false;
					TransitionToState(State.JUMP);
				}
				break;

			default:
				break;
		}
	}

	#endregion

	#region Privates

	private CharacterMovementController _controller;

	private PlayerLocomotionState _locomotion;
	private PlayerJumpState _jump;
	private PlayerCrouchState _crouch;
	private PlayerSprintState _sprint;
	private PlayerSlideState _slide;
	private PlayerWallrunState _wallRun;

	#endregion
}
