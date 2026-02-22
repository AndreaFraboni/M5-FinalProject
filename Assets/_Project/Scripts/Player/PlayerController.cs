using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _smooth = 10f;
    [SerializeField] private float _rotationSpeed = 5f;

    public string surfaceTag;

    private Rigidbody _rb;

    private float _h, _v;

    private Vector3 _currentDirection;

    private Camera _cam;

    public bool isGrounded = false;
    public bool isRunning = false;

    public Vector3 GetDirection() => _currentDirection;

    private void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
    }

    void Update()
    {
        CheckInput();
        CheckRun();
    }

    private void FixedUpdate()
    {
        Move();
        Rotation();
    }

    private void CheckInput()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        Vector3 targetDirection = Vector3.zero;
        targetDirection = _cam.transform.forward * _v + _cam.transform.right * _h;
        targetDirection.y = 0f;

        if (targetDirection.magnitude > 0.01f) targetDirection.Normalize();

        _currentDirection = Vector3.Lerp(_currentDirection, targetDirection, _smooth * Time.deltaTime);
    }

    private void CheckRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    private void Move()
    {
        if (isRunning)
        {
            _rb.MovePosition(transform.position + _currentDirection * ((_speed * 2) * Time.deltaTime));
        }
        else
        {
            _rb.MovePosition(transform.position + _currentDirection * (_speed * Time.deltaTime));
        }
    }

    private void Rotation()
    {
        if (_currentDirection != Vector3.zero) //transform.forward = move; 
        {
            Quaternion _rotation = Quaternion.LookRotation(_currentDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, Time.deltaTime * _rotationSpeed);
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
