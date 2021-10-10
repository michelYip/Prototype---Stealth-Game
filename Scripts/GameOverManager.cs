using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;

public class GameOverManager : MonoBehaviour
{
	#region Exposed

	private static GameOverManager m_instance;

    [Header ("Game Over Property")]
	[SerializeField] private VideoPlayer m_hackVideo;
    [SerializeField] private float m_timeBeforeGameOver = 3f;
    [SerializeField] private GameObject m_gameOverScreen;
    [SerializeField] private Transform m_enemyManager;

    [Header ("Recovery Property")]
    [SerializeField] private float m_recoveryTime = 2f;

    [Header ("Game Sound")]
    [SerializeField] private AudioSource m_ambientSound;
    [SerializeField] private AudioSource m_playerFoundSound;

    #endregion

    #region Unity API
    void Awake()
    {
        if (m_instance != null && m_instance != this)
            Destroy(gameObject);
        m_instance = this;

        _playerFound = false;
        _gameOverStartTime = Time.time;
        m_gameOverScreen.SetActive(false);
    }

	private void Start()
	{
        _enemies = new AbstractEnemyBehaviour[m_enemyManager.childCount];
        for (int i = 0; i < _enemies.Length; i++)
		{
            _enemies[i] = m_enemyManager.GetChild(i).GetComponentInChildren<AbstractEnemyBehaviour>();
		}
	}

	void Update()
    {
        if (_gameOver)
		{
            Time.timeScale = 0f;
            m_gameOverScreen.SetActive(true);
            Cursor.visible = true;
        }
        else
		{
            if (!_playerFound)
            {
                m_ambientSound.pitch = 1f;
                m_playerFoundSound.pitch = 0f;
                m_hackVideo.targetCameraAlpha = 0f;
            }
            else
            {
                m_ambientSound.pitch = 0f;
                m_playerFoundSound.pitch = 1.25f;
                float alpha = (Time.time - _gameOverStartTime) / m_timeBeforeGameOver;
                m_hackVideo.targetCameraAlpha = Mathf.Lerp(0f, 0.75f, alpha);
                if (alpha >= 1f)
                    _gameOver = true;

                bool checkPlayerFound = false;
                for (int i = 0; i < _enemies.Length; i++)
				{
                    if (_enemies[i].Target != null)
					{
                        if (_enemies[i].PlayerFound && _enemies[i].CanSeePlayer(_enemies[i].Target))
                        {
                            checkPlayerFound = true;
                            break;
                        }
                    }
				}
                _playerFound = checkPlayerFound;
            }
        }
    }
    #endregion

    #region Main Methods

    public static GameOverManager Instance { get => m_instance; set => m_instance = value; }
	public bool PlayerFound { get => _playerFound; set => _playerFound = value; }
	public float GameOverStartTime { get => _gameOverStartTime; set => _gameOverStartTime = value; }
	public bool GameOver { get => _gameOver; set => _gameOver = value; }

	#endregion

	#region Privates

	private bool _playerFound;
	private float _gameOverStartTime;

    private bool _gameOver;
    public AbstractEnemyBehaviour[] _enemies;

	#endregion
}
