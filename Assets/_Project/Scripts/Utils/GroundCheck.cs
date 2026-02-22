using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private float _probeDistance = 0.1f;
    [SerializeField] private LayerMask _layerGroundMask;

    [SerializeField] private PlayerController _playerController;

    private void Awake()
    {
        if (_playerController == null) _playerController = GetComponentInParent<PlayerController>();
        if (_layerGroundMask == 0) _layerGroundMask = LayerMask.GetMask("Ground");
    }


    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position, Vector3.down, out hit, _probeDistance, _layerGroundMask);

        _playerController.isGrounded = grounded;

        if (grounded)
        {
            _playerController.surfaceTag = hit.collider.tag; // lettura del Tag del Ground su cui si trova il player ....
        }
     
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _probeDistance);
    }

}
