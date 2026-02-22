using UnityEngine;
using UnityEngine.AI;

public class MoveAgent_exercize : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private NavMeshPath _navMeshPath;
    [SerializeField] private LineRenderer _lineRenderer;

    private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
        _meshAgent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleAgent();
            HandleRenderer();
        }
        else if (Input.GetButton("Fire3"))
        {
            _meshAgent.isStopped = true;
            HandleAgent();
            HandleRenderer();
        }
        else
        {
            _meshAgent.isStopped = false;
        }
    }

    public void HandleRenderer()
    {
        if (_meshAgent.hasPath)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = _meshAgent.path.corners.Length;
            _lineRenderer.SetPositions(_meshAgent.path.corners);
            //_lineRenderer.startColor = Color.green;
            //_lineRenderer.endColor = Color.green;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        if (_navMeshPath.status == NavMeshPathStatus.PathPartial)
        {
            _lineRenderer.startColor = Color.yellow;
            _lineRenderer.endColor = Color.yellow;
        }
        else if (_navMeshPath.status == NavMeshPathStatus.PathInvalid)
        {
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
        }
        else if (_navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            _lineRenderer.startColor = Color.green;
            _lineRenderer.endColor = Color.green;
        }
    }
    public void HandleAgent()
    {
        Ray pointToRayMouse = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(pointToRayMouse, out RaycastHit hit))
        {
            _navMeshPath = new NavMeshPath();
            _meshAgent.CalculatePath(hit.point, _navMeshPath);
            _meshAgent.SetDestination(hit.point);
        }
    }


}
