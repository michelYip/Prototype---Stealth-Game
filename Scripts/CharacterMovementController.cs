using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
	#region Exposed

	[Header ("Basic Movement Parameter And Camera Controller")]
	[SerializeField] private float m_moveSpeed = 10f;
	[SerializeField] private Transform m_cameraTransform;

	[Space(10)]
	[Header ("Jump Parameter")]
	[SerializeField] private float m_gravityForce = 2f;
	[SerializeField] private float m_jumpHeightLocomotion = 3f;
	[SerializeField] private float m_jumpHeightSprint = 4f;
	[Header("Slopes And Stairs Managing Parameters")]
	[SerializeField] private float m_maxAngle = 45f;
	[Header("Ground Detection Parameters")]
	[SerializeField] private LayerMask m_groundedLayerMask;
	[SerializeField] private float m_groundCheckDistance = 1.05f;
	[SerializeField] private float m_playerHalfHeight = 1f;
	[SerializeField] private float m_groundRayOffSet = 0.5f;

	[Header("Wall Detection Parameters")]
	[SerializeField] private LayerMask m_wallLayerMask;
	[SerializeField] private float m_wallCheckDistance = 0.5f;
	[SerializeField] private float m_wallRayOffSet = 0.2f;

	#endregion

	#region Unity API
	private void Awake()
	{
		Time.timeScale = 1f;
		Cursor.visible = false;

		_rigidBody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<CapsuleCollider>();

		if (m_cameraTransform == null)
		{
			Debug.Log("Debug.Break() : CameraTransform is not defined !");
			Debug.Break();
		}

		_velocity = _rigidBody.velocity;
	}

	private void Update()
	{
		UpdateRaycastPosition();
	}

	private void FixedUpdate()
	{
		_rigidBody.velocity = _velocity;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		foreach(Ray ray in _groundDetectRays)
			Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * m_groundCheckDistance));

		Gizmos.color = Color.blue;
		foreach (Ray ray in _ceilingDetectionRays)
			Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * m_groundCheckDistance));

		Gizmos.color = Color.yellow;
		foreach (Ray ray in _forwardWallDetectionRay)
			Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * m_wallCheckDistance));

		Gizmos.color = Color.magenta;
		foreach (Ray ray in _leftWallDetectionRays)
			Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * m_wallCheckDistance));

		Gizmos.color = Color.cyan;
		foreach (Ray ray in _rightWallDetectionRays)
			Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * m_wallCheckDistance));
	}

	#endregion

	#region Main Methods

	public Vector3 GetMovementInput()
	{
		return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	}

	public Vector3 GetMouseMovement()
	{
		return new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
	}

	public Vector3 MovePlayer()
	{
		float horizontalInput = GetMovementInput().x;
		float verticalInput = GetMovementInput().z;
		Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CameraTransform.eulerAngles.y;
		Vector3 MoveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

		if (CameraTransform.eulerAngles.y != 0f)
			transform.forward = Quaternion.Euler(0f, CameraTransform.eulerAngles.y, 0f) * Vector3.forward;

		return MoveDir;
	}

	public bool CheckIsGrounded()
	{
		for (int i = 0; i < _groundDetectRays.Length; i++)
		{
			if (Physics.Raycast(_groundDetectRays[i], m_groundCheckDistance, m_groundedLayerMask))
			{
				IsGrounded = true;
				return IsGrounded;
			}
		}
		IsGrounded = false;
		return IsGrounded;
	}

	public bool CanStand()
	{
		for (int i = 0; i < _ceilingDetectionRays.Length; i++)
		{
			if (Physics.Raycast(_ceilingDetectionRays[i], m_groundCheckDistance, m_groundedLayerMask))
			{
				return false;
			}
		}
		return true;
	}

	public Vector3 FindFloor()
	{
		RaycastHit hit;

		float floorCheckDistance = m_groundCheckDistance * 1.05f;
		float averageHeight = 0;
		Vector3 result = transform.position;
		int n = 0;
		for (int i = 0; i < _groundDetectRays.Length; i++)
		{
			if (Physics.Raycast(_groundDetectRays[i], out hit, floorCheckDistance, m_groundedLayerMask))
			{
				if (Vector3.Angle(hit.normal, Vector3.up) < m_maxAngle)
				{
					averageHeight += hit.point.y + m_playerHalfHeight;// + m_groundCheckDistance;
					n++;
				}
			}
		}
		if (n != 0)
			averageHeight /= n;
		else
			return result;
		result.y = averageHeight;// + m_playerHalfHeight;
		return result;
	}

	public Vector3 FindLeftWall()
	{
		RaycastHit hit;
		int hitCount = 0;
		Vector3 result = Vector3.zero;
		for (int i = 0; i < _leftWallDetectionRays.Length; i++)
		{
			if (Physics.Raycast(_leftWallDetectionRays[i], out hit, m_wallCheckDistance, m_wallLayerMask))
			{
				result += hit.point;
				hitCount++;
			}
		}
		if (hitCount != 0)
		{
			return result / hitCount;
		}
		return result;
	}

	public Vector3 FindRightWall()
	{
		RaycastHit hit;
		int hitCount = 0;
		Vector3 result = Vector3.zero;
		for (int i = 0; i < _rightWallDetectionRays.Length; i++)
		{
			if (Physics.Raycast(_rightWallDetectionRays[i], out hit, m_wallCheckDistance, m_wallLayerMask))
			{
				result += hit.point;
				hitCount++;
			}
		}
		if (hitCount != 0)
		{
			return result / hitCount;
		}
		return result;
	}

	public Vector3 GetLeftWallNormal()
	{
		RaycastHit hit;
		for (int i = 0; i < _rightWallDetectionRays.Length; i++)
		{
			if (Physics.Raycast(_leftWallDetectionRays[i], out hit, m_wallCheckDistance, m_wallLayerMask))
			{
				return hit.normal;
			}
		}
		return Vector3.zero;
	}

	public Vector3 GetRightWallNormal()
	{
		RaycastHit hit;
		for (int i = 0; i < _rightWallDetectionRays.Length; i++)
		{
			if (Physics.Raycast(_rightWallDetectionRays[i], out hit, m_wallCheckDistance, m_wallLayerMask))
			{
				return hit.normal;
			}
		}
		return Vector3.zero;
	}

	public float GetSlopeSpeedModifier(Vector3 MoveDirection)
	{
		RaycastHit hit;
		float floorCheckDistance = m_groundCheckDistance * 1.05f;
		float speedModifier = 0f;
		int n = 0;
		for (int i = 0; i < _groundDetectRays.Length; i++)
		{
			if (Physics.Raycast(_groundDetectRays[i], out hit, floorCheckDistance, m_groundedLayerMask))
			{
				//Debug.Log(Mathf.Lerp(0, 90, Mathf.Abs(Vector3.Dot(hit.normal, MoveDirection.normalized)))); //Calcul l'angle exacte de la surface sur laquel le Player se tient
				speedModifier += Vector3.Dot(hit.normal, MoveDirection.normalized);
				n++;
			}
		}
		if (n != 0)
			speedModifier /= n;
		else
			return 1f;

		speedModifier += 1f;

		return (speedModifier < 0.5f) ? 0f : 1f;
	}

	public void AdjustHeightToFloor()
	{
		Vector3 floorPos = FindFloor();
		if (floorPos != Vector3.zero)
		{
			//Debug.Log(floorPos);
			_rigidBody.MovePosition(new Vector3(transform.position.x, floorPos.y, transform.position.z));
			//transform.position = new Vector3(transform.position.x, floorPos.y, transform.position.z);
		}
	}

	public void AdjustCollider(Vector3 targetValue, float targetHeight, float transitionDuration)
	{
		if (transitionDuration == 0f)
		{
			Debug.Log("Can't divide by 0 !");
			return;
		}
		_collider.center = Vector3.Lerp(_collider.center, targetValue, Time.deltaTime/transitionDuration);
		_collider.height = Mathf.Lerp(_collider.height, targetHeight, Time.deltaTime / transitionDuration);
	}

	public void LerpTransformPosition(Vector3 targetValue, float transitionDuration)
	{
		if (transitionDuration == 0f)
		{
			Debug.Log("Can't divide by 0 !");
			return;
		}
		transform.position = Vector3.Lerp(transform.position, targetValue, Time.deltaTime / transitionDuration);
	}

	public void UpdateRaycastPosition()
	{
		/*
		_groundDetectRays[0] = new Ray(transform.position									     , Vector3.down);
		_groundDetectRays[1] = new Ray(transform.position + transform.forward * m_groundRayOffSet, Vector3.down);
		_groundDetectRays[2] = new Ray(transform.position + transform.right   * m_groundRayOffSet, Vector3.down);
		_groundDetectRays[3] = new Ray(transform.position - transform.forward * m_groundRayOffSet, Vector3.down);
		_groundDetectRays[4] = new Ray(transform.position - transform.right   * m_groundRayOffSet, Vector3.down);
		*/
		_groundDetectRays[0] = new Ray(transform.position, Vector3.down);
		float angleStep = 360f / (37f-1f);
		for (int i = 1; i < 37; i++)
		{
			_groundDetectRays[i] = new Ray(transform.position + (Quaternion.Euler(0, angleStep * i, 0) * transform.right) * m_groundRayOffSet, Vector3.down);
		}

		_ceilingDetectionRays[0] = new Ray(transform.position, Vector3.up);
		_ceilingDetectionRays[1] = new Ray(transform.position + transform.forward * m_groundRayOffSet, Vector3.up);
		_ceilingDetectionRays[2] = new Ray(transform.position + transform.right * m_groundRayOffSet, Vector3.up);
		_ceilingDetectionRays[3] = new Ray(transform.position - transform.forward * m_groundRayOffSet, Vector3.up);
		_ceilingDetectionRays[4] = new Ray(transform.position - transform.right * m_groundRayOffSet, Vector3.up);

		_forwardWallDetectionRay[0] = new Ray(transform.position								    , transform.forward);
		_forwardWallDetectionRay[1] = new Ray(transform.position + transform.right * m_wallRayOffSet, transform.forward);
		_forwardWallDetectionRay[2] = new Ray(transform.position - transform.right * m_wallRayOffSet, transform.forward);

		_rightWallDetectionRays[0] = new Ray(transform.position 									    , transform.right);
		_rightWallDetectionRays[1] = new Ray(transform.position + transform.forward * m_wallRayOffSet, transform.right);
		_rightWallDetectionRays[2] = new Ray(transform.position - transform.forward * m_wallRayOffSet, transform.right);
		
		_leftWallDetectionRays[0] = new Ray(transform.position 								        , -transform.right);
		_leftWallDetectionRays[1] = new Ray(transform.position + transform.forward * m_wallRayOffSet, -transform.right);
		_leftWallDetectionRays[2] = new Ray(transform.position - transform.forward * m_wallRayOffSet, -transform.right);
	}

	public void SetFloat(string param, float value)
	{
		_animator.SetFloat(param, value);
	}

	public void SetInt(string param, int value)
	{
		_animator.SetInteger(param, value);
	}

	public void SetBool(string param, bool value)
	{
		_animator.SetBool(param, value);
	}

	public void SetTrigger(string param)
	{
		_animator.SetTrigger(param);
	}

	public Vector3 Velocity { get => _velocity; set => _velocity = value; }
	public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
	public Transform CameraTransform { get => m_cameraTransform; set => m_cameraTransform = value; }
	public float MoveSpeed { get => m_moveSpeed; set => m_moveSpeed = value; }
	public float GravityForce { get => m_gravityForce; set => m_gravityForce = value; }
	public Rigidbody RigidBody { get => _rigidBody; set => _rigidBody = value; }
	public bool IsCrouching { get => _isCrouching; set => _isCrouching = value; }
	public bool IsSprinting { get => _isSprinting; set => _isSprinting = value; }
	public float JumpHeightLocomotion { get => m_jumpHeightLocomotion; set => m_jumpHeightLocomotion = value; }
	public float JumpHeightSprint { get => m_jumpHeightSprint; set => m_jumpHeightSprint = value; }
	public Animator Animator { get => _animator; set => _animator = value; }
	public bool IsSliding { get => _isSliding; set => _isSliding = value; }
	public bool IsWallRunning { get => _isWallRunning; set => _isWallRunning = value; }

	#endregion

	#region Privates

	private Rigidbody _rigidBody;
	private CapsuleCollider _collider;
	private Animator _animator;
	private Vector3 _velocity;
	private bool _isGrounded = false;
	private bool _isCrouching = false;
	private bool _isSprinting = false;
	private bool _isSliding = false;
	private bool _isWallRunning = false;

	private Ray[] _groundDetectRays = new Ray[37];
	private Ray[] _ceilingDetectionRays = new Ray[5];

	private Ray[] _forwardWallDetectionRay = new Ray[3];
	private Ray[] _leftWallDetectionRays = new Ray[3];
	private Ray[] _rightWallDetectionRays = new Ray[3];

	#endregion
}
