using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    #region Exposed

    [SerializeField] private AudioSource m_footstep;
    [Range(0f,3f)]
    [SerializeField] private float m_footstepPitchModifier;

    [SerializeField] private AudioSource m_jumpStart;
    [SerializeField] private AudioSource m_jumpLand;

    #endregion

    #region Unity API
    void Awake()
    {
        _state = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<CharacterMovementController>();
        //m_jumpStart= 1f;
        //m_jumpLand = 1f;
    }

    void Update()
    {
        if (GameOverManager.Instance == null || !GameOverManager.Instance.GameOver)
		{
            switch (_state.CurrentState)
            {
                case State.LOCOMOTION:
                case State.SPRINT:
                case State.CROUCH:
                case State.WALLRUN:
                    m_footstep.pitch = (_controller.Velocity.magnitude / _controller.MoveSpeed) * m_footstepPitchModifier;
                    break;
                case State.SLIDE:
                    m_footstep.pitch = 0;
                    break;
                case State.JUMP:
                    m_footstep.pitch = 0;
                    break;
                default:
                    break;
            }
        }
        else
		{
            m_footstep.pitch = 0f;
            m_jumpStart.Pause();
            m_jumpLand.Pause();
        }
		
	}
    #endregion

    #region Main Methods

    public void PlaySFX(string sfx)
	{
		switch (sfx)
		{
            case "jumpStart":
                m_jumpStart.Play();
                break;
            case "jumpLand":
                m_jumpLand.Play();
                break;
		}
	}

    #endregion

    #region Privates

    private PlayerStateMachine _state;
    private CharacterMovementController _controller;

    #endregion
}
