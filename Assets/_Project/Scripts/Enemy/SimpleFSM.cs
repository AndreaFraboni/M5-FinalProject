using System.Collections;
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

    [SerializeField] private STATE _state = STATE.WANDER;

    [SerializeField] private Transform _target;

    [SerializeField] private float _wanderRadius = 10f;
    [SerializeField] private float _visonDistance = 10f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch(_state)
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

    }

    private void ChaseUpdate()
    {

    }

    private void WanderUpdate()
    {

    }

    private bool CanSeeTarget()
    {
        if (_target == null) return false;

        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= _visonDistance) return true;

        return false;
    }




}
