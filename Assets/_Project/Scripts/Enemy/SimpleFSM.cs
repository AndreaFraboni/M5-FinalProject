using System;
using System.Collections;
using System.Threading;
using Unity.Mathematics;
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
    [SerializeField] private STATE _initialSate;

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _eyes;

    [SerializeField] private float _wanderRadius = 5f;
    [SerializeField] private float _wanderRepatTime = 1f;
    [SerializeField] private float _timer = 0f;
    private Coroutine _wanderSearchRoutine;
    private Vector3 _StartingAgentPos;
    private int _failedSearches = 0;
    private int _maxFailedSearches = 3;

    [SerializeField] private float _rotateInterval = 3f;
    private float _rotateTimer = 0f;
    private float _rotationSpeed = 30f;

    [SerializeField][Range(0f, 180f)] private float _fov = 90f;
    [SerializeField][Range(1f, 20f)] private float _viewDistance = 10f;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] bool CanSeeTarget = false;

    private void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        _StartingAgentPos = _agent.transform.position;
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

        if (_state == STATE.WANDER && _wanderSearchRoutine != null)
        {
            StopCoroutine(_wanderSearchRoutine);
            _wanderSearchRoutine = null;
            _timer = 0f;
        }

        _state = newState;
    }

    private void IdleUpdate()
    {
        _rotateTimer += Time.deltaTime;

        if (_rotateTimer >= _rotateInterval)
        {
            _rotateTimer = 0f;
            RotateAgent();
        }
    }

    private void RotateAgent()
    {
        Quaternion target = Quaternion.LookRotation(-transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, _rotationSpeed * Time.deltaTime);
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
        if (_wanderSearchRoutine != null) return;
        if (_agent.pathPending) return;
        if (_agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance) return;

        _timer += Time.deltaTime;
        if (_timer >= _wanderRepatTime)
        {
            _timer = 0f;
            if (!NextRandomDestination())
            {
                StartCoroutine(SearchDestination());
            }
        }
    }

    private IEnumerator SearchDestination()
    {
        _failedSearches = 0;

        while (!NextRandomDestination())
        {
            _failedSearches++;

            if (_failedSearches >= _maxFailedSearches)
            {
                _agent.SetDestination(_StartingAgentPos);
                _wanderSearchRoutine = null;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
        _failedSearches = 0;
        _wanderSearchRoutine = null;
    }

    private bool NextRandomDestination()
    {
        NavMeshHit hit;
        Vector3 randomVect = new Vector3(UnityEngine.Random.Range(1f, _wanderRadius), 0f, UnityEngine.Random.Range(1f, _wanderRadius));
        randomVect += transform.position;
        if (NavMesh.SamplePosition(randomVect, out hit, _wanderRadius, NavMesh.AllAreas))
        {
            Debug.Log("OK !! Destinazione VALIDA !!");
            _currentDestination = hit.position;
            _agent.SetDestination(_currentDestination);
            return true;
        }
        Debug.Log("Destinazione NON VALIDA !!");
        return false;
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
