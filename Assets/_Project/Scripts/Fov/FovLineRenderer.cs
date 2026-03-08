using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class FovLineRenderer : MonoBehaviour
{
    [SerializeField] bool dotVersionEnabled;

    [SerializeField] Transform player;
    [SerializeField] LayerMask obstacleLayer;

    [Range(0, 360)]
    [SerializeField] float angle = 90f;
    [SerializeField] float radius = 5f;
    [SerializeField] int segments = 24;


    [SerializeField] Color normalColor = Color.green;
    [SerializeField] Color alertColor = Color.red;

    private LineRenderer _lr;

    public Transform Player => player;


    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.loop = true;
        _lr.positionCount = segments + 2;
        _lr.useWorldSpace = true;
        _lr.startWidth = 0.05f;
        _lr.endWidth = 0.05f;
    }

    void Update()
    {
        UpdateLineColor();

        DrawVisionCone();
    }

    private void UpdateLineColor()
    {
        if (IsPlayerInRange())
        {
            //_lr.SetLineColor(alertColor);
        }
        else
        {
            //_lr.SetLineColor(normalColor);
        }
    }


    void DrawVisionCone()
    {

        float startAngle = -angle * 0.5f;

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        _lr.SetPosition(0, origin);

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (angle / segments) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 point = origin + direction * radius;
            // _lr.SetPosition(i+1,point);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, radius, obstacleLayer))
            {
                point = hit.point;
            }

            _lr.SetPosition(i + 1, point);
        }

    }

    public bool IsPlayerInRange()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position);
        directionToPlayer.y = 0;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > radius) return false;

        directionToPlayer.Normalize();

        if (dotVersionEnabled)
        {
            float halfAngle = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
            float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

            if (dotProduct < halfAngle) return false;
        }
        else
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer > angle * 0.5f) return false;
        }

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer, obstacleLayer))
        {
            return false;
        }

        return true;
    }

}

