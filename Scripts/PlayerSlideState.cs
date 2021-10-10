using UnityEngine;

public class PlayerSlideState : MonoBehaviour, StateInterface
{
    #region Exposed

    [SerializeField] private Vector3 m_colliderCenterPosition = new Vector3(0, 0.2f, 0);

    [Range(7.5f, 25f)]
    [SerializeField] private float m_initialSpeed = 8.5f;
    [Range(4f, 10f)]
    [SerializeField] private float m_endSpeed = 5f;
    [SerializeField] private float m_slideDuration = 1.5f;

    #endregion

    #region Unity API
    void Awake()
    {
        _controller = GetComponent<CharacterMovementController>();
        _camera = GetComponent<PlayerCameraController>();
    }

    void Update()
    {
        
    }
    #endregion

    #region Main Methods

    public void DoInit()
    {
        _controller.SetTrigger("SlideStartTrigger");
        _controller.IsSliding = true;

        _camera.SwitchCamera("SLIDE");

        _slideDirection = _controller.Velocity.normalized;
        _slideStart = Time.time;
        _slideCancelTimer = Time.time + 0.5f;

        _canCancelSlide = false;
        _slideEnded = false;
    }

    public void DoUpdate()
    {
        _controller.AdjustCollider(m_colliderCenterPosition, 0.4f, 0.05f);
        _controller.Velocity = Vector3.Lerp(_slideDirection * m_initialSpeed, _slideDirection * m_endSpeed, (Time.time - _slideStart) / m_slideDuration);
        _controller.SetFloat("LocomotionMagnitude", Vector3.ClampMagnitude(new Vector3(_controller.GetMovementInput().x, 0f, _controller.GetMovementInput().z), 1f).magnitude);
        
        if (Time.time > _slideCancelTimer)
		{
            _canCancelSlide = true;
		}

        if (Time.time > _slideStart + m_slideDuration)
		{
            _slideEnded = true;
		}
    }

    public void DoExit()
    {
        _controller.IsSliding = false;
        _controller.SetTrigger("SlideEndTrigger");
    }

    public bool CanCancelSlide { get => _canCancelSlide; set => _canCancelSlide = value; }
	public bool SlideEnded { get => _slideEnded; set => _slideEnded = value; }

	#endregion

	#region Privates

	private CharacterMovementController _controller;
    private PlayerCameraController _camera;

    private Vector3 _slideDirection;
    private float _slideCancelTimer;
    private float _slideStart;

    private bool _canCancelSlide = false;
    private bool _slideEnded = false;

	#endregion
}
