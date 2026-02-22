using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private NavMeshPath _navMeshPath;

    private Ray ray;
    private RaycastHit hit;

    private float x;
    private float y;
    private float z;
    private float velocitySpeed;
    private Vector3 _currentDirection;

    private Camera _camera;

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

        x = _meshAgent.velocity.x;
        y = _meshAgent.velocity.y;
        z = _meshAgent.velocity.z;
        velocitySpeed = x + z;

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
            _meshAgent.destination = hit.point;
        }
    }

    public void FootStepSound()
    {
        if (string.IsNullOrEmpty(surfaceTag)) return;
        //List<AudioClip> list = null;

        if (!isRunning)
        {
            //if (surfaceTag == "Grass")
            //    list = _grassWalkFootSteps;
            //else if (surfaceTag == "Wood")
            //    list = _woodWalkFootSteps;
            //else if (surfaceTag == "Dirty")
            //    list = _dirtyWalkFootSteps;
        }
        else
        {
            //if (surfaceTag == "Grass")
            //    list = _grassRunFootSteps;
            //else if (surfaceTag == "Wood")
            //    list = _woodRunFootSteps;
            //else if (surfaceTag == "Dirty")
            //    list = _dirtyRunFootSteps;
        }

        //if (list == null || list.Count == 0) return;
        //int index = Random.Range(0, list.Count);
        //_audioSource.PlayOneShot(list[index]);

    }



}
