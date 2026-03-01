using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private NavMeshPath _navMeshPath;
    [SerializeField] private Camera _camera;

    [Header("Game UI Manager")]
    [SerializeField] private GameUIManager _GameUIManager;

    private Vector3 _currentDirection;

    public string surfaceTag;

    public bool isGrounded = false;
    public bool isRunning = false;
    public bool isPaused = false;

    public Vector3 GetDirection() => _currentDirection;

    private void Awake()
    {
        _camera = Camera.main;
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isPaused) return;

        if (Input.GetMouseButton(0))
        {
            HandleAgent();
        }
    }

    public void HandleAgent()
    {
        Ray pointToRayMouse = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(pointToRayMouse, out RaycastHit hit))
        {
            _meshAgent.SetDestination(hit.point);
        }
    }

    public void FootStepSound()
    {
  

    }

    public void DestroyGOPlayer()
    {
        _GameUIManager.GameOver();
        Destroy(gameObject);
    }

}
