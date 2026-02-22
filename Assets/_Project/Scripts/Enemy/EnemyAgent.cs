using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agentNav;

    [SerializeField] private Transform[] waypoints;
    private STATE state;
    private int currentPoint;

    [Header("Stat for Visual Cone")]
    [SerializeField] private Transform target;
    [SerializeField] private float angularOfView;
    [SerializeField] private float sightDistance;
    [SerializeField] private LayerMask obstacle;

    [SerializeField] private int subdivision = 24;
    [SerializeField] private float interval = 0.5f;

    private LineRenderer lineRend;
    private Coroutine coneRoutine;

    private enum STATE
    {
        Patrol,
        Chasing
    }

    private void Awake()
    {
        agentNav = GetComponent<NavMeshAgent>();
        lineRend = GetComponent<LineRenderer>();

        state = STATE.Patrol;
    }

    private void OnEnable()
    {
        // Avvia UNA SOLA coroutine
        coneRoutine = StartCoroutine(CustomUpdate());
    }

    private void OnDisable()
    {
        // Stop pulito quando l'oggetto viene disabilitato/distrutto
        if (coneRoutine != null)
        {
            StopCoroutine(coneRoutine);
            coneRoutine = null;
        }
    }

    private void Update()
    {
        // Aggiorna lo stato in base alla visione
        ConeVisual();

        // FSM
        switch (state)
        {
            case STATE.Patrol:
                Patrol();
                break;

            case STATE.Chasing:
                Chase();
                break;
        }
    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            // Disegna il cono ogni "interval" secondi
            DrawConeOfViewQuaternion(subdivision);
            yield return new WaitForSeconds(interval);
        }
    }

    private void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        agentNav.SetDestination(waypoints[currentPoint].position);

        if (!agentNav.pathPending && agentNav.remainingDistance <= 0.05f)
        {
            currentPoint++;

            if (currentPoint >= waypoints.Length)
                currentPoint = 0;
        }
    }

    private void Chase()
    {
        if (target == null) return;

        agentNav.SetDestination(target.position);
    }

    public bool ConeVisual()
    {
        if (target == null)
            return false;

        Vector3 toTarget = target.position - transform.position;
        float sqrDistance = toTarget.sqrMagnitude;

        // fuori distanza massima
        if (sqrDistance > sightDistance * sightDistance)
        {
            state = STATE.Patrol;
            // Debug.Log("Non ha visto, troppo lontano");
            return false;
        }

        float distance = Mathf.Sqrt(sqrDistance);
        toTarget /= distance; // normalizza

        // fuori angolo di visione (Dot vs Cos)
        if (Vector3.Dot(transform.forward, toTarget) < Mathf.Cos(angularOfView * Mathf.Deg2Rad))
        {
            state = STATE.Patrol;
            // Debug.Log("Non ha visto, fuori angolo");
            return false;
        }

        // ostacolo tra enemy e target
        if (Physics.Linecast(transform.position, target.position, obstacle))
        {
            state = STATE.Patrol;
            // Debug.Log("Non ha visto, ostacolo");
            return false;
        }

        // visto!
        state = STATE.Chasing;
        // Debug.Log("Ha visto!");
        return true;
    }

    public void DrawConeOfViewQuaternion(int subdivisions)
    {
        lineRend.positionCount = subdivisions + 1;

        float startAngle = -angularOfView;

        // Vector3 originLine = Vector3.zero;
        Vector3 originLine = transform.position;
        Vector3 rayCastOrigin = transform.position + new Vector3(0f, 0.1f, 0f);
        Vector3 forward = transform.forward;

        lineRend.SetPosition(0, originLine);

        float deltaAngle = (2 * angularOfView / subdivisions);

        for (int i = 0; i <= subdivisions; i++)
        {
            float currentAngle = startAngle + deltaAngle * i;

            Vector3 dir =
                Quaternion.Euler(0f, currentAngle, 0f) * forward;

            Vector3 point =
                originLine + dir * sightDistance;

            if (Physics.Raycast(
                    rayCastOrigin,
                    dir,
                    out var hitInfo,
                    sightDistance,
                    obstacle))
            {
                point = hitInfo.point;
            }

            lineRend.SetPosition(i + 1, point);
        }
    }
}
