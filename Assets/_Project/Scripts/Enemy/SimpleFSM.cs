using System;
using UnityEngine;
using UnityEngine.AI;

public class SimpleSFM : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        PATROL,
        CHASE,
        WANDER
    }

    [SerializeField] private Transform[] _wayPoints;
    private int _currentWayPoint = 0;

    private Vector3 _currentDestination;

    [SerializeField] private STATE _state = STATE.IDLE;

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _eyes;

    //[SerializeField] private float _wanderRadius = 10f;
    //[SerializeField] private float _visonDistance = 10f;

    [SerializeField][Range(0f, 180f)] private float _fov = 90f;
    [SerializeField][Range(1f, 20f)] private float _viewDistance = 10f;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] bool CanSeeTarget = false;

    [SerializeField] private STATE _initialSate;

    private void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _initialSate = _state; // Save initial behaviour STATE setted in inspector.

        if (_wayPoints != null && _wayPoints.Length > 0)
        {
            _currentDestination = _wayPoints[0].position;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 origin = _eyes.position;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(origin, origin + transform.forward * _viewDistance);

        Vector3 left = DirFromAngle(-_fov * 0.5f);
        Vector3 right = DirFromAngle(_fov * 0.5f);

        Gizmos.DrawLine(origin, origin + left * _viewDistance);
        Gizmos.DrawLine(origin, origin + right * _viewDistance);
    }

    Vector3 DirFromAngle(float angle)
    {
        angle = angle + transform.eulerAngles.y;
        float radiant = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radiant), 0f, Mathf.Cos(radiant));
    }

    private void Update()
    {
        CanSeeTarget = CanSeeTargetInFov();

        switch (_state)
        {
            case STATE.IDLE:
                IdleUpdate();
                break;
            case STATE.PATROL:
                PatrolUpdate();
                break;
            case STATE.CHASE:
                ChaseUpdate();
                break;
            case STATE.WANDER:
                WanderUpdate();
                break;
        }
    }

    public void SetState(STATE newState)
    {
        if (_state == newState) return;
        Debug.Log("CHANGE STATE From " + _state + " To " + newState);
        _state = newState;
    }

    private void IdleUpdate()
    {

    }

    private void PatrolUpdate()
    {
        if (_wayPoints == null || _wayPoints.Length == 0)
        {
            _agent.isStopped = true;
            return;
        }

        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentWayPoint++;

            if (_currentWayPoint >= _wayPoints.Length)
            {
                _currentWayPoint = 0;
            }

            _currentDestination = _wayPoints[_currentWayPoint].position;
        }

        _agent.SetDestination(_currentDestination);
    }

    private void ChaseUpdate()
    {
        if (_target == null) return;

        _agent.SetDestination(_target.position);
    }

    private void WanderUpdate()
    {

    }

    private bool CanSeeTargetInFov()
    {
        if (_target == null) return false;

        Vector3 forward = transform.forward;
        Vector3 dirToPlayer = (_target.position - transform.position);
        if (dirToPlayer.magnitude > _viewDistance)
        {
            SetState(_initialSate);
            return false; // player fuori dalla distanza massima di visuale dell'enemy
        }


        Vector3 dir = dirToPlayer.normalized;
        float angle = Vector3.Angle(forward, dir);
        if (angle > _fov * 0.5f)
        {
            SetState(_initialSate);
            return false; // il player si trova in direzione che è ad un angolo fuori dall'ampiezza visuale dell'enemy rispetto al suo forward
        }

        //if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, _viewDistance))
        // TO DO 

        CanSeeTarget = true;

        SetState(STATE.CHASE);

        return true; // se tutto non è false allora il player è nel fov
    }




}
