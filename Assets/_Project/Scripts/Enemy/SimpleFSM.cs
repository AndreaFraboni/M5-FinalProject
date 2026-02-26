using System;
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

    [SerializeField] private float _fov = 90f; 
    [SerializeField] private float _viewDistance = 10f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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

    }

    private void ChaseUpdate()
    {

    }

    private void WanderUpdate()
    {

    }

    private bool CanSeeTargetInFov()
    {
        if (_target == null) return false;

        Vector3 forward = transform.forward;
        Vector3 dirToPlayer = (_target.position - transform.position);

        float distance = Vector3.Distance(forward, dirToPlayer); 
        if (dirToPlayer.magnitude > _viewDistance) return false; // player fuori dalla distanza massima di visuale dell'enemy

        Vector3 dir = dirToPlayer.normalized;
        float angle = Vector3.Angle(forward, dir);
        if (angle > _fov * 0.5f) return false; // la direzione del player è ad un angolo fuori dall'ampiezza visuale dell'enemy rispetto al suo forward


        return true; // se tutto non false alllora il player è nel fov
    }




}
