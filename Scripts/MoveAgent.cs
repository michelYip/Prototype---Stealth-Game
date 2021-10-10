using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    #region Exposed

    [SerializeField] private Transform m_target;

    #endregion

    #region Unity API
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _agent.SetDestination(m_target.position);
    }
    #endregion

    #region Main Methods
    #endregion

    #region Privates

    private NavMeshAgent _agent;

    #endregion
}
